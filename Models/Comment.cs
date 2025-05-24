namespace SoruCevapPortal.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int? AnswerId { get; set; }
        public Answer Answer { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}