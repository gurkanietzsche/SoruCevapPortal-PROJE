namespace SoruCevapPortal.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserDTO User { get; set; }
    }

    public class CommentCreateDTO
    {
        public string Content { get; set; }
        public int AnswerId { get; set; }
    }
}