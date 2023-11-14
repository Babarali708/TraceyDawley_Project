using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TraceyDawley.Models
{
    public class User: BaseModel
    {
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? FirstName { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? LastName { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? MiddleName { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? Contact { get; set; }
        
        [Column(TypeName = "nvarchar(255)")]
        public string? Address { get; set; }
        
        [Column(TypeName = "nvarchar(255)")]
        public string? DateOfBirth { get; set; }

        [Column(TypeName = "nvarchar(355)")]
        public string? Email { get; set; }
        public string? encEmail { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Password { get; set; }
        public int? Role { get; set; }
    }
}
