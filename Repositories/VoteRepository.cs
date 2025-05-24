using Microsoft.EntityFrameworkCore;
using SoruCevapPortal.Models;

namespace SoruCevapPortal.Repositories
{
    public class VoteRepository : GenericRepository<Vote>
    {
        public VoteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Vote> GetUserVoteAsync(string userId, int? questionId, int? answerId)
        {
            return await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == userId &&
                                        v.QuestionId == questionId &&
                                        v.AnswerId == answerId);
        }

        public async Task<int> GetVoteCountAsync(int? questionId, int? answerId)
        {
            var votes = await _context.Votes
                .Where(v => v.QuestionId == questionId && v.AnswerId == answerId)
                .ToListAsync();

            return votes.Sum(v => v.IsUpvote ? 1 : -1);
        }
    }
}