namespace SoruCevapPortal.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public bool IsUpvote { get; set; }
        public DateTime VoteDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int? QuestionId { get; set; }
        public Question Question { get; set; }
        public int? AnswerId { get; set; }
        public Answer Answer { get; set; }
    }
}