namespace SoruCevapPortal.DTOs
{
    public class VoteDTO
    {
        public bool IsUpvote { get; set; }
        public int? QuestionId { get; set; }
        public int? AnswerId { get; set; }
    }
}