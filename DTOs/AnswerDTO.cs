
namespace SoruCevapPortal.DTOs
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsAccepted { get; set; }
        public UserDTO User { get; set; }
        public int VoteCount { get; set; }
        public int CommentCount { get; set; }
    }

    public class AnswerCreateDTO
    {
        public string Content { get; set; }
        public int QuestionId { get; set; }
    }

    public class AnswerUpdateDTO
    {
        public string Content { get; set; }
    }
}