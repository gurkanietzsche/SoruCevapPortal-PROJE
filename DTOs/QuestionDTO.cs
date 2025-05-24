
namespace SoruCevapPortal.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int ViewCount { get; set; }
        public UserDTO User { get; set; }
        public List<TagDTO> Tags { get; set; }
        public int VoteCount { get; set; }
        public int AnswerCount { get; set; }
    }

    public class QuestionCreateDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
    }

    public class QuestionUpdateDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
    }
}