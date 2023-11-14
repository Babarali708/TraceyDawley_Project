using Microsoft.EntityFrameworkCore;
using TraceyDawley.Helping_Classes;
using TraceyDawley.Models;

namespace TraceyDawley.Repositories
{
    public interface IUserRepo
    {
        Task<User?> GetUserByLogin(string email, string password);
        Task<User?> GetUserById(int id);
        Task<SurveyResponseData?> GetUserResponseById(int id);
        Task<int?> GetActiveUserCount(int role);
        Task<int?> getAddUserId(User user);
        Task<IEnumerable<User>> GetActiveUserList();
        Task<IEnumerable<SurveyResponseData>> GetSurveyResponseList();
        Task<bool> AddUser(User user);
        Task<bool> AddSurveyResponseData(SurveyResponseData SurveyResponseData);
        Task<bool> UpdateUser(User user);
        Task<bool> UpdateUserResponse(SurveyResponseData user);
        Task<bool> DeleteUser(int id);
        Task<bool> DeleteUserResponse(int id);
        Task<bool> ValidateEmail(string email, int? id);
        Task<User?> GetUserByEmail(string email);
        Task<bool> AddContentFile(string filePath, IFormFile file, int UserId);

    }

    public class UserRepo : IUserRepo 
    {
        private readonly AppDbContext context;

        public UserRepo(AppDbContext _appDbContext)
        {
            context = _appDbContext;
        }

        public async Task<User?> GetUserById(int id)
        {
            return await context.User.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == 1 && x.Role!=0 && x.Role!=1);
        }
        public async Task<SurveyResponseData?> GetUserResponseById(int id)
        {
            return await context.SurveyResponseData.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == 1);
        }
        public async Task<int?> GetActiveUserCount(int role)
        {
            return await context.User.CountAsync(x => x.IsActive == 1 && x.Role == role);
        }
        public async Task<User?> GetUserByLogin(string email, string password)
        {
            return await context.User.FirstOrDefaultAsync(x => x.IsActive == 1 && x.Email!.ToLower() == email.Trim().ToLower() && x.Password == password);
        }
        public async Task<IEnumerable<User?>> GetActiveUserList()
        {
            var User = await context.User.Where(x => x.IsActive == 1 && x.Role!=0 && x.Role!=1).OrderByDescending(x => x.Id).ToListAsync();

            return User;
        }
        public async Task<bool> AddUser(User user)
        {
            try
            {
                context.User.Add(user);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddSurveyResponseData(SurveyResponseData _SurveyResponseData)
        {
            try
            {
                context.SurveyResponseData.Add(_SurveyResponseData);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<int?> getAddUserId(User user)
        {
            try
            {
                context.User.Add(user);
                await context.SaveChangesAsync();
                return user.Id;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                User? user = await GetUserById(id);
                user!.IsActive = 0;
                return await UpdateUser(user);
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteUserResponse(int id)
        {
            try
            {
                SurveyResponseData? user = await GetUserResponseById(id);
                user!.IsActive = 0;
                return await UpdateUserResponse(user);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ValidateEmail(string email, int? id)
        {
            int emailCount = 0;

            if (id == null)
            {
                emailCount = await context.User.CountAsync(x => x.IsActive == 1 && x.Email!.ToLower() == email.ToLower().Trim());
            }
            else
            {
                emailCount = await context.User.CountAsync(x => x.IsActive == 1 && x.Id != id && x.Email!.ToLower() == email.ToLower().Trim());
            }

            return emailCount == 0;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await context.User.FirstOrDefaultAsync(x => x.Email!.ToLower() == email.Trim().ToLower() && x.IsActive == 1);
        }

        public async Task<bool> AddContentFile(string filePath, IFormFile file, int UserId)
        {
            try
            {
                ContentFile contentFile = new ContentFile
                {
                    FileName = Path.GetFileName(file.FileName),
                    FilePath = filePath,
                    FileSize = Math.Round((double)file.Length, 2),
                    FileExtension = Path.GetExtension(file.FileName),
                    UserId = UserId,
                    UploadedBy = UserId,
                    CreatedAt = GeneralPurpose.DateTimeNow(),
                    IsActive = 1,
                };

                context.ContentFile.Add(contentFile);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<IEnumerable<SurveyResponseData?>> GetSurveyResponseList()
        {
            var getResponses = await context.SurveyResponseData.Where(x => x.IsActive == 1).OrderByDescending(x => x.Id).ToListAsync();

            return getResponses;
        }

        public async Task<bool> UpdateUserResponse(SurveyResponseData user)
        {
            try
            {
                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
