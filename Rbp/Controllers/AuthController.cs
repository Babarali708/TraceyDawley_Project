using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TraceyDawley.Helping_Classes;
using TraceyDawley.Models;
using TraceyDawley.Repositories;
using System.Data;
using TraceyDawley.Filters;
using Newtonsoft.Json;

namespace TraceyDawley.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepo userRepo;
        private readonly GeneralPurpose gp;
        private readonly ProjectVariables projectVariables;

        public AuthController(IUserRepo _userRepo, IHttpContextAccessor haccess, IOptions<ProjectVariables> options)
        {
            userRepo = _userRepo;
            gp = new GeneralPurpose(haccess);
            projectVariables = options.Value;
        }
        public IActionResult Login(string msg = "", string color = "black")
        {
            User? loggedinUser = gp.GetUserClaims();

            if (loggedinUser != null)
            {
                if (loggedinUser.Role == 1 || loggedinUser.Role == 0)
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("addUserFormData", "Home");
            }

            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostLogin(string Email = "", string Password = "")
        {
            User? user = await userRepo.GetUserByLogin(Email, Password);

            if (user == null)
            {
                return RedirectToAction("Login", new { msg = "Incorrect Email/Password!", color = "red" });
            }

            await gp.SetUserClaims(user);

            if (user.Role == 1 || user.Role == 0)
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("addUserFormData", "Home");
                //return RedirectToAction("Index", "Home");
        }


        #region Forgot Password

        public IActionResult ForgotPassword(string msg = "", string color = "black")
        {
            ViewBag.Color = color;
            ViewBag.Message = msg;

            return View();
        }

        public async Task<IActionResult> PostForgotPassword(string Email = "")
        {
            User? u = await userRepo.GetUserByEmail(Email);

            if (u != null)
            {
                string BaseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/";

                MailSender mailSender = new MailSender(projectVariables);
                bool checkMail = await mailSender.SendForgotPasswordEmailAsync(u.Email, u.Id.ToString(), BaseUrl);

                if (checkMail == true)
                {
                    return RedirectToAction("ForgotPassword", "Auth", new { msg = "Please check your mails' inbox/spam", color = "green" });
                }
                else
                {
                    return RedirectToAction("ForgotPassword", "Auth", new { msg = "Mail sending fail!", color = "red" });
                }
            }
            else
            {
                return RedirectToAction("ForgotPassword", "Auth", new { msg = "Email does not belong to our record!!", color = "red" });
            }
        }


        public IActionResult ResetPassword(string encId = "", string t = "", string msg = "", string color = "black")
        {
            DateTime PassDate = new DateTime(Convert.ToInt64(t)).Date;
            DateTime CurrentDate = GeneralPurpose.DateTimeNow().Date;

            if (CurrentDate != PassDate)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Link expired, Please try again!", color = "red" });
            }


            ViewBag.Time = t;
            ViewBag.EncId = encId;
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PostResetPassword(string encId = "", string t = "", string NewPassword = "", string ConfirmPassword = "")
        {
            if (NewPassword != ConfirmPassword)
            {
                return RedirectToAction("ResetPassword", "Auth", new { encId = encId, t = t, msg = "Password and confirm password did not match", color = "red" });
            }

            User user = await userRepo.GetUserById(int.Parse(encId));
            user.Password = NewPassword;

            if (await userRepo.UpdateUser(user))
            {
                return RedirectToAction("Login", "Auth", new { msg = "Password reset successful, Try login", color = "green" });
            }
            else
            {
                return RedirectToAction("ResetPassword", "Auth", new { encId = encId, t = t, msg = "Somethings' wrong!", color = "red" });
            }
        }

        #endregion


        #region Signup
        public IActionResult Register(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostRegister(User _user, string _confirmPassword = "", List<IFormFile>? File=null)
        {
            if (_user.Password != _confirmPassword)
            {
                return RedirectToAction("Register", "Auth", new { msg = "Password and confirm password didn't match", color = "red" });
            }

            bool checkEmail = await userRepo.ValidateEmail(_user.Email, null); 
            if (checkEmail == false)
            {
                return RedirectToAction("Register", "Auth", new { msg = "Email already exists. Try sign in!", color = "red" });
            }

            User u = new User()
            {
                UserId = Guid.NewGuid(),
                FirstName = _user.FirstName.Trim(),
                LastName = _user.LastName.Trim(),
                MiddleName = string.IsNullOrEmpty(_user.MiddleName) ? "" : _user.MiddleName,
                Contact = _user.Contact,
                Email = _user.Email.Trim(),
                Password = _user.Password,
                Address = _user.Address,
                DateOfBirth=_user.DateOfBirth,
                Role = 3,
                IsActive = 1,
                CreatedAt = GeneralPurpose.DateTimeNow()
            };

            var addUserId = await userRepo.getAddUserId(u);

            if (addUserId == null)
            {
                return RedirectToAction("Register", "Auth", new { msg = "Somethings' wrong", color = "red" });

                //return RedirectToAction("Login", "Auth", new { msg = "Account created successfully, Please login", color = "green" });
            }

            string invalideFilesMsg = "Failed to upload file: \n";
            if (File != null)
            {
                foreach (var file in File)
                {
                    if (file.Length != 0)
                    {
                        string FileExt = System.IO.Path.GetExtension(file.FileName);

                        if (FileExt.ToLower().Equals(".jpg") || FileExt.ToLower().Equals(".png") || FileExt.ToLower().Equals(".jpeg"))
                        {
                            string image = await GeneralPurpose.UploadProfilePicture(file, "", "tile");
                            if (!string.IsNullOrEmpty(image))
                            {
                                var isUploadFile = await userRepo.AddContentFile(image, file,(int)addUserId);
                            }
                            else
                            {
                                invalideFilesMsg += file.FileName + " \n";
                            }
                        }
                        //else
                        //{
                        //    TempData["TileDto"] = JsonConvert.SerializeObject(tileDto);
                        //    return RedirectToAction("AddTile", "Auth", new { msg = "File must be type of jpg/png/jpeg", color = "red" });
                        //}
                    }
                    //else
                    //{
                    //    TempData["TileDto"] = JsonConvert.SerializeObject(tileDto);
                    //    return RedirectToAction("AddTile", "Auth", new { msg = "Corrupt file. Select other one", color = "red" });
                    //}
                }

            }

            User? user = await userRepo.GetUserById(addUserId.Value);

            await gp.SetUserClaims(user);
            return RedirectToAction("addUserFormData", "Home", new { msg = "Account created successfully", color = "green" });

        }

        #endregion


        #region Manage Profile
        [ValidationFilter(Roles = new int[] { 0, 1, 2, 3 })]
        public async Task<IActionResult> UpdateProfile(string msg = "", string color = "black")
        {
            User? u = await userRepo.GetUserById(gp.GetUserClaims().Id);

            ViewBag.User = u;
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [ValidationFilter(Roles = new int[] { 0, 1, 2, 3 })]
        public async Task<IActionResult> PostUpdateProfile(User _user)
        {
            bool checkEmail = await userRepo.ValidateEmail(_user.Email, _user.Id);

            if (checkEmail == false)
            {
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Email used by someone else, Please try another", color = "red" });
            }

            User u = await userRepo.GetUserById(_user.Id);
            if (u == null)
            {
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "No Record Found!", color = "red" });
            }
            u.FirstName = _user.FirstName.Trim();
            u.LastName = _user.LastName.Trim();
            u.MiddleName = string.IsNullOrEmpty(_user.MiddleName) ? "" : _user.MiddleName;
            u.Contact = _user.Contact;
            u.Email = _user.Email.Trim();

            if (await userRepo.UpdateUser(u))
            {
                await gp.SetUserClaims(u);
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Profile updated successfully!", color = "green" });
            }
            else
            {
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Somthings' Wrong!", color = "red" });
            }
        }

        [ValidationFilter(Roles = new int[] { 0, 1, 2, 3 })]
        public IActionResult UpdatePassword(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [ValidationFilter(Roles = new int[] { 0, 1, 2 })]
        public async Task<IActionResult> PostUpdatePassword(string oldPassword = "", string newPassword = "", string confirmPassword = "")
        {
            if (newPassword != confirmPassword)
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "New password and Confirm password did not match!", color = "red" });
            }

            User? u = await userRepo.GetUserById(gp.GetUserClaims().Id);

            if (u.Password != oldPassword)
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "Old password did not match!", color = "red" });
            }

            u.Password = newPassword;

            if (await userRepo.UpdateUser(u))
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "Password updated successfully!", color = "green" });
            }
            else
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "Somthings' wrong!", color = "red" });
            }
        }

        #endregion


        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
