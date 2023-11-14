namespace TraceyDawley.Models
{
    public class BaseModel
    {
        public int? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
