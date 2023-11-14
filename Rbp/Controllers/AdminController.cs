using Microsoft.AspNetCore.Mvc;
using TraceyDawley.Filters;
using TraceyDawley.Helping_Classes;
using TraceyDawley.Models;
using TraceyDawley.Repositories;
using System.Data;
using System.Xml.Serialization;
using static TraceyDawley.Helping_Classes.DtoCollection;
using TraceyDawley.HelpingClasses;

namespace TraceyDawley.Controllers
{
    [ExceptionFilter]
    [ValidationFilter(Roles = new int[] { 1,0,3,2 })]
    public class AdminController : Controller
    {
        private readonly IUserRepo userRepo;
        public AdminController(IUserRepo _userRepo)
        {
            userRepo = _userRepo;
        }
        public async Task<IActionResult> Index(string msg = "", string color = "black")
        {
            ViewBag.CustomerCount = await userRepo.GetActiveUserCount(3);

            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }

        #region Manage User
        public IActionResult AddUser(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostAddUser(User _user)
        {
            _user.UserId = Guid.NewGuid();
            _user.FirstName = _user.FirstName.Trim();
            _user.LastName = _user.LastName.Trim();
            _user.MiddleName = string.IsNullOrEmpty(_user.MiddleName) ? "" : _user.MiddleName;
            _user.Email = _user.Email.Trim();
            //_user.Password = _user.Password.Trim();
            _user.Address = _user.Address;
            _user.DateOfBirth=_user.DateOfBirth;
            //_user.Role = _user.Role;
            _user.Role = 3;
            _user.IsActive = 1;
            _user.CreatedAt = GeneralPurpose.DateTimeNow();

            if (!await userRepo.AddUser(_user))
            {

                return RedirectToAction("AddUser", "Admin", new { Role = _user.Role, msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddUser", "Admin", new { Role = _user.Role, msg = "Record Inserted Successfully", color = "green" });
        }

        public IActionResult ViewUser(int Role,int way=-1, string msg = "", string color = "black")
        {
            ViewBag.userId = way;
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostUpdateUser(User _user)
        {
            User? user = await userRepo.GetUserById(_user.Id);
            if (user == null)
            {
                return RedirectToAction("ViewUser", "Admin", new { msg = "Record not found", color = "red" });
            }
            user.FirstName = _user.FirstName.Trim();
            user.MiddleName = string.IsNullOrEmpty(_user.MiddleName) ? "" : _user.MiddleName;
            user.LastName = _user.LastName.Trim();
            user.Email = _user.Email.Trim();
            user.Contact = _user.Contact;
            user.Address = _user.Address;
            user.DateOfBirth = _user.DateOfBirth;

            if (await userRepo.UpdateUser(user))
            {
                    return RedirectToAction("ViewUser", "Admin", new { Role = user.Role, msg = "User updated successfully", color = "green" });
                
            }
            return RedirectToAction("ViewUser", "Admin", new { Role = user.Role, msg = "Somethings' wrong", color = "red" });
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            User? user = await userRepo.GetUserById(int.Parse(id));
            if (!await userRepo.DeleteUser(int.Parse(id)))
            {
                return RedirectToAction("ViewUser", "Admin", new { Role = user.Role, msg = "Somethings' wrong", color = "red" });

            }

            return RedirectToAction("ViewUser", "Admin", new { Role = user.Role, msg = "Record deleted successfully!", color = "green" });
        }

        #endregion

        #region Manage Surveys

        public async Task<IActionResult> ViewSurveyFeedbacks(string msg = "", string color = "black")
        {
            var getUserFromSurveyFeedbacks = await userRepo.GetActiveUserList();
            if(getUserFromSurveyFeedbacks.Count()>0)
            {
                ViewBag.SurveyFeedbacks = getUserFromSurveyFeedbacks;
            }
            else
            {
                ViewBag.SurveyFeedbacks = null;
            }
            ViewBag.Message = msg;
            ViewBag.Color = color;
            return View();
        }

        public async Task<IActionResult> DeleteResponse(int? id)
        {
            SurveyResponseData? getUserResponse = await userRepo.GetUserResponseById((int)id);
            if (!await userRepo.DeleteUserResponse((int)id))
            {
                return RedirectToAction("ViewSurveyFeedbacks", "Admin", new {msg = "Somethings' wrong", color = "red" });

            }

            return RedirectToAction("ViewSurveyFeedbacks", "Admin", new { msg = "Record deleted successfully!", color = "green" });
        }
        #endregion

        public async Task<IActionResult> generateXmlFile(int? userId=-1,int? id=-1)
        {
            try
            {
                var getSurveyFeedbacks = await userRepo.GetSurveyResponseList();
                if(userId!=-1&&userId!=null)
                {
                    getSurveyFeedbacks= getSurveyFeedbacks.Where(x=> x.UserId==userId).ToList();
                }
                if(id!=null && id != -1)
                {
                    getSurveyFeedbacks = getSurveyFeedbacks.Where(x=>x.Id==id).ToList();
                }
                List<Survey> surveyXmlDataList = new List<Survey>();

                foreach (SurveyResponseData surveyResponseData in getSurveyFeedbacks)
                {
                    var userData = new UserData();
                    var surveyResponse = new SurveyResponse();

                    if (surveyResponseData.UserId != null)
                    {
                        var getUser = await userRepo.GetUserById((int)surveyResponseData.UserId);
                        if (getUser != null)
                        {
                            userData.Name = getUser.FirstName + " " + getUser.LastName;
                            userData.Email = StringCipher.Base64Encode(getUser.Email);
                        }
                    }

                    surveyResponse.Question1 = surveyResponseData.Question1;
                    surveyResponse.Question2 = surveyResponseData.Question2;
                    surveyResponse.Question3 = surveyResponseData.Question3;
                    surveyResponse.Question4 = surveyResponseData.Question4;
                    surveyResponse.Question5 = surveyResponseData.Question5;
                    surveyResponse.Question6 = surveyResponseData.Question6;
                    surveyResponse.Question7 = surveyResponseData.Question7;
                    surveyResponse.Question8 = surveyResponseData.Question8;

                    var surveyXmlData = new Survey
                    {
                        User = userData,
                        SurveyResponse = surveyResponse
                    };

                    surveyXmlDataList.Add(surveyXmlData);
                }

                if (surveyXmlDataList.Any())
                {
                    // Serialize the list to XML
                    var serializer = new XmlSerializer(typeof(List<Survey>));
                    var xmlString = new StringWriter();
                    serializer.Serialize(xmlString, surveyXmlDataList);

                    System.IO.File.WriteAllText("SurveyFeedbacks.xml", xmlString.ToString());

                    if (System.IO.File.Exists("SurveyFeedbacks.xml"))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes("SurveyFeedbacks.xml");
                        string fileName = "SurveyFeedbacks.xml";

                        // Delete the file after reading its content
                        System.IO.File.Delete("SurveyFeedbacks.xml");

                        return File(fileBytes, "application/xml", fileName);
                    }
                   else
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes("SurveyFeedbacks.xml");
                        string fileName = "SurveyFeedbacks.xml";

                        return File(fileBytes, "application/xml", fileName);
                    }

                    //return Content(xmlString.ToString(), "application/xml"); //it is use for to view xml file on new tab
                }
                else
                {
                    ViewBag.Message = "No survey feedbacks available.";
                    ViewBag.Color = "red";
                    return RedirectToAction("ViewSurveyFeedbacks", "Admin", new { msg = "No survey feedbacks available.", color = "red" });

                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error occurred: {ex.Message}";
                return RedirectToAction("ViewSurveyFeedbacks", "Admin", new { msg = "something went wrong", color = "red" });

            }
        }


        //public async Task<IActionResult> generateXmlFile()
        //{
        //    try
        //    {
        //        var getSurveyFeedbacks = await userRepo.GetSurveyResponseList();
        //        List<SurveyResponse> SurveyResponseList = new List<SurveyResponse>();

        //        foreach (SurveyResponseData SurveyResponseData in getSurveyFeedbacks)
        //        {
        //            var UserName = "";
        //            var UserEmail = "";
        //            if (SurveyResponseData.UserId != null)
        //            {
        //                var getUser = await userRepo.GetUserById((int)SurveyResponseData.UserId);
        //                if (getUser != null)
        //                {
        //                    UserName = getUser.FirstName + " " + getUser.LastName;
        //                    UserEmail = StringCipher.Base64Encode(getUser.Email);
        //                }
        //            }
        //            SurveyResponse obj = new SurveyResponse()
        //            {
        //                UserName = UserName,
        //                UserEmail= UserEmail,
        //                Question1 = SurveyResponseData.Question1,
        //                Question2 = SurveyResponseData.Question2,
        //                Question3 = SurveyResponseData.Question3,
        //                Question4 = SurveyResponseData.Question4,
        //                Question5 = SurveyResponseData.Question5,
        //                Question6 = SurveyResponseData.Question6,
        //                Question7 = SurveyResponseData.Question7,
        //                Question8 = SurveyResponseData.Question8,

        //            };
        //            SurveyResponseList.Add(obj);
        //        }


        //        if (getSurveyFeedbacks.Any())
        //        {
        //            // Serialize the list to XML
        //            var serializer = new XmlSerializer(typeof(List<SurveyResponse>)); 
        //            var xmlString = new StringWriter();
        //            serializer.Serialize(xmlString, SurveyResponseList);

        //            System.IO.File.WriteAllText("SurveyFeedbacks.xml", xmlString.ToString());

        //            return Content(xmlString.ToString(), "application/xml");
        //        }
        //        else
        //        {

        //            ViewBag.Message = "No survey feedbacks available.";
        //            ViewBag.Color = "red"; // You can change the color to indicate an error state
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //       var errorMessage = $"Error occurred: {ex.Message}";
        //        return View();
        //    }
        //}


    }
}
