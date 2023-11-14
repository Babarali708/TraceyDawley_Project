using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TraceyDawley.HelpingClasses;
using TraceyDawley.Helping_Classes;
using TraceyDawley.HelpingClasses;
using TraceyDawley.Models;
using TraceyDawley.Repositories;
using static TraceyDawley.Helping_Classes.DtoCollection;

namespace TraceyDawley.Controllers
{
    public class AjaxController : Controller
    {
        private readonly IUserRepo userRepo;
        private readonly GeneralPurpose gp;

        public AjaxController(IUserRepo _userRepo, IHttpContextAccessor haccess)
        {
            userRepo = _userRepo;
            gp = new GeneralPurpose(haccess);
        }


        #region User

        [HttpPost]
        public async Task<IActionResult> GetSurveyResponseDataTableList(string Role = "", string email = "", int userId = -1)
        {
            var ulist = await userRepo.GetActiveUserList();
            ulist = ulist.Where(x => x.Role != 0 && x.Role != 1);
            if (!String.IsNullOrEmpty(Role))
            {
                ulist = ulist.Where(x => x.Role.ToString().Contains(Role.ToLower())).ToList();
            }
            if (userId != -1 && userId != null)
            {
                ulist = ulist.Where(x => x.Id == userId).ToList();
            }

            if (!String.IsNullOrEmpty(email))
            {
                ulist = ulist.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.Contact != null && x.Contact.ToLower().Contains(searchValue.ToLower())
                                    || x.FirstName != null && x.FirstName.ToLower().Contains(searchValue.ToLower())
                                    || x.LastName != null && x.LastName.ToLower().Contains(searchValue.ToLower())
                                    || x.MiddleName != null && x.MiddleName.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            List<UserDto> udto = new List<UserDto>();

            foreach (User u in ulist)
            {
                UserDto obj = new UserDto()
                {
                    Id = u.Id.ToString(),
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    MiddleName = u.MiddleName,
                    Email = StringCipher.Base64Encode(u.Email),
                    Contact = u.Contact,
                };

                udto.Add(obj);
            }

            return Json(new { data = udto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        [HttpPost]
        public async Task<IActionResult> GetUserById(string id)
        {
            User? u = await userRepo.GetUserById(int.Parse(id));
            if (u == null)
            {
                return Json(0);
            }

            UserDto obj = new UserDto()
            {
                Id = u.Id.ToString(),
                FirstName = u.FirstName,
                LastName = u.LastName,
                MiddleName = u.MiddleName,
                Email = u.Email,
                Contact = u.Contact,
            };

            return Json(obj);
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> ValidateEmail(string email, int id)
        {
            return Json(await userRepo.ValidateEmail(email, id));
        }

        [HttpPost]
        public async Task<IActionResult> GetSurveyDataTableList(int userId = -1)
        {
            var SurveyResponseDataList = await userRepo.GetSurveyResponseList();
            if (userId != -1 && userId != null)
            {
                SurveyResponseDataList = SurveyResponseDataList.Where(x => x.UserId == userId).ToList();
            }

            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            // Sorting logic
            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        if (sortColumnName != "UserName")
                        {
                            SurveyResponseDataList = SurveyResponseDataList.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                        }
                        else
                        {
                            SurveyResponseDataList = SurveyResponseDataList
        .OrderByDescending(x => sortColumnName == "UserName" && x.UserId != null
            ? x.UserId.ToString()
            : x.GetType().GetProperty(sortColumnName)?.GetValue(x)?.ToString())
        .ToList();
                        }
                    }
                    else
                    {
                        if (sortColumnName != "UserName")
                        {

                            SurveyResponseDataList = SurveyResponseDataList.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                        }
                        else
                        {
                            SurveyResponseDataList = SurveyResponseDataList
                                    .OrderBy(x => sortColumnName == "UserName" && x.UserId != null
                                        ? x.UserId.ToString()
                                        : x.GetType().GetProperty(sortColumnName)?.GetValue(x)?.ToString())
                                    .ToList();
                        }

                    }
                }
            }

            int totalrows = SurveyResponseDataList.Count();

            // Filtering logic
            if (!string.IsNullOrEmpty(searchValue))
            {
                SurveyResponseDataList = SurveyResponseDataList.Where(x => x.Question1 != null && x.Question1.ToLower().Contains(searchValue.ToLower()) ||
                                                       x.Question2 != null && x.Question2.ToLower().Contains(searchValue.ToLower()) ||
                                                       x.Question3 != null && x.Question3.ToLower().Contains(searchValue.ToLower()) ||
                                                       x.Question4 != null && x.Question4.ToLower().Contains(searchValue.ToLower())// ... repeat for other properties
                                            ).ToList();
            }

            int totalrowsafterfilterinig = SurveyResponseDataList.Count();

            SurveyResponseDataList = SurveyResponseDataList.Skip(start).Take(length).ToList();

            List<SurveyResponseDataDto> SurveyResponseDataListDto = new List<SurveyResponseDataDto>();

            foreach (SurveyResponseData SurveyResponseData in SurveyResponseDataList)
            {
                var UserName = "";
                if (SurveyResponseData.UserId != null)
                {
                    var getUser = await userRepo.GetUserById((int)SurveyResponseData.UserId);
                    if (getUser != null)
                    {
                        UserName = getUser.FirstName + " " + getUser.LastName;
                    }
                }
                SurveyResponseDataDto obj = new SurveyResponseDataDto()
                {
                    Id = SurveyResponseData.Id,
                    Question1 = SurveyResponseData.Question1,
                    Question2 = SurveyResponseData.Question2,
                    Question3 = SurveyResponseData.Question3,
                    Question4 = SurveyResponseData.Question4,
                    Question5 = SurveyResponseData.Question5,
                    Question6 = SurveyResponseData.Question6,
                    Question7 = SurveyResponseData.Question7,
                    Question8 = SurveyResponseData.Question8,
                    UserId = SurveyResponseData.UserId,
                    UserName = UserName
                };
                SurveyResponseDataListDto.Add(obj);
            }

            return Json(new { data = SurveyResponseDataListDto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

    }
}
