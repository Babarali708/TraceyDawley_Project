using Microsoft.EntityFrameworkCore;

namespace TraceyDawley.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<ContentFile> ContentFile { get; set; }
        public DbSet<SurveyResponseData> SurveyResponseData { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasData
           (
                new User
                {
                    Id = 1,
                    UserId = Guid.NewGuid(),
                    FirstName = "Super",
                    MiddleName = "",
                    LastName = "Admin",
                    Contact = "00000000000",
                    Email = "superadmin@gmail.com",
                    Password = "123",
                    Role = 0,
                    IsActive = 1,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    Id = 2,
                    UserId = Guid.NewGuid(),
                    FirstName = "Admin",
                    MiddleName = "",
                    LastName = "",
                    Contact = "00000000000",
                    Email = "admin@gmail.com",
                    Password = "123",
                    Role = 1,
                    IsActive = 1,
                    CreatedAt = DateTime.Now
                }
            );
                
        }
    }


}
