using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using TraceyDawley.Models;
using System.Security.Claims;

namespace TraceyDawley.Helping_Classes
{
    public class GeneralPurpose
    {
        private readonly HttpContext? hcontext;
        public GeneralPurpose(IHttpContextAccessor haccess)
        {
            hcontext = haccess.HttpContext;

        }
        public User? GetUserClaims()
        {
            string? userId = hcontext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            string? firtName = hcontext?.User.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
            string? lastName = hcontext?.User.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value;
            string? middleName = hcontext?.User.Claims.FirstOrDefault(c => c.Type == "MiddleName")?.Value;
            string? email = hcontext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            string? role = hcontext?.User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            User? loggedInUser = null;
            if (userId != null)
            {
                loggedInUser = new User()
                {
                    Id = int.Parse(userId),
                    FirstName = firtName,
                    MiddleName = middleName,
                    LastName = lastName,
                    Email = email,
                    Role = Convert.ToInt32(role)
                };
            }

            return loggedInUser;
        }
        public async Task<bool> SetUserClaims(User user)
        {
            try
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("MiddleName", user.MiddleName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Role", user.Role.ToString()),
                };


                // Claim Identity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                };

                // Claim Principle
                await hcontext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static DateTime DateTimeNow()
        {
            return DateTime.Now;
        }

        public static async Task<string> UploadProfilePicture(IFormFile file, string oldProfile = "", string way = "")
        {
            try
            {
                // Define the root folder where you want to save the file
                string rootFolder = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UserProfile\");
                string fileFirstName = "userImage";
                if (!string.IsNullOrEmpty(way))
                {
                    rootFolder = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UserOtherImage\");
                    fileFirstName = "OtherImage";
                }

                if (!Directory.Exists(rootFolder))
                {
                    Directory.CreateDirectory(rootFolder);
                }

                // Generate a unique filename for the uploaded file
                string fileName = fileFirstName + DateTime.Now.Ticks + Path.GetExtension(file.FileName);

                // Combine the root folder and the generated filename
                string filePath = Path.Combine(rootFolder, fileName);

                if (!string.IsNullOrEmpty(oldProfile))
                {
                    string[] getName = oldProfile.Split("/");

                    // Check if a profile picture already exists for the user
                    string existingProfilePicturePath = Path.Combine(rootFolder, getName.Last());
                    if (System.IO.File.Exists(existingProfilePicturePath))
                    {
                        System.IO.File.Delete(existingProfilePicturePath);
                    }
                }

                //if (!string.IsNullOrEmpty(way))
                //{
                //    // Resize the image before saving
                //    using (var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
                //    {
                //        var imageEncoder = new JpegEncoder
                //        {
                //            Quality = 100 // Set JPEG quality to 100 (maximum quality)
                //        };

                //        var width = 220;
                //        var height = 220;
                //        if (image.Width > image.Height)
                //        {
                //            height = (int)((image.Height / (double)image.Width) * width);
                //        }
                //        else
                //        {
                //            width = (int)((image.Width / (double)image.Height) * height);
                //        }

                //        image.Mutate(x => x.Resize(new ResizeOptions
                //        {
                //            //Size = new Size(220, 220), // Resize to 220x220 pixels
                //            Size = new Size(width, height), // Resize to 220x220 pixels
                //            Mode = ResizeMode.Max,
                //            //Position = AnchorPositionMode.Center,
                //        }));
                //        //Save the resized image with high quality
                //        image.Save(filePath, imageEncoder);

                //        //// Resize the image to fit within 220x220
                //        //image.Mutate(x => x.Resize(new ResizeOptions
                //        //{
                //        //    Size = new Size(220, 220),
                //        //    Mode = ResizeMode.BoxPad,
                //        //    Position = AnchorPositionMode.Center,
                //        //}));

                //        //// Create a new image with the desired size and fill it with the background color
                //        //using (var newImage = new Image<Rgba32>(configuration: new Configuration(), 220, 220, Color.White))
                //        //{
                //        //    // Overlay the resized image onto the new image
                //        //    newImage.Mutate(ctx => ctx.DrawImage(image, new Point(0, 0), 1f));

                //        //    // Save the final image
                //        //    newImage.Save(filePath, imageEncoder);
                //        //}
                //    }
                //}
                //else
                //{
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                //}



                // You can save the filePath to a database or return it as needed
                Console.WriteLine($"File uploaded to: {filePath}");

                if (!string.IsNullOrEmpty(way))
                {
                    return "/UserOtherImage/" + fileName;
                }
                else
                {
                    return "/UserProfile/" + fileName;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
