using Microsoft.EntityFrameworkCore;
using SoruCevapPortal.Models;

namespace SoruCevapPortal.Repositories
{
    public class CommentRepository : GenericRepository<Comment>
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetCommentsByAnswerAsync(int answerId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.AnswerId == answerId && c.IsActive)
                .OrderBy(c => c.CreatedDate)
                .ToListAsync();
        }
    }
}