using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraceyDawley.Models
{
    public class SurveyResponseData :BaseModel
    {
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column(TypeName = "nvarchar(255)")]
        public string? Question1 { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? Question2 { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string? Question3 { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? Question4 { get; set; }

        [Column(TypeName = "nvarchar(355)")]
        public string? Question5 { get; set; } 
        
        [Column(TypeName = "nvarchar(355)")]
        public string? Question6 { get; set; }

        [Column(TypeName = "nvarchar(355)")]
        public string? Question7 { get; set; }

        [Column(TypeName = "nvarchar(355)")]
        public string? Question8 { get; set; }
        public int? UserId { get; set; }

    }
}
