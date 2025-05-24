using Microsoft.EntityFrameworkCore;
using SoruCevapPortal.Models;

namespace SoruCevapPortal.Repositories
{
    public class QuestionRepository : GenericRepository<Question>
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Question>> GetQuestionsWithDetailsAsync()
        {
            return await _context.Questions
                .Include(q => q.User)
                .Include(q => q.Tags)
                .Include(q => q.Answers)
                .Include(q => q.Votes)
                .Where(q => q.IsActive)
                .OrderByDescending(q => q.CreatedDate)
                .ToListAsync();
        }

        public async Task<Question> GetQuestionWithDetailsAsync(int id)
        {
            return await _context.Questions
                .Include(q => q.User)
                .Include(q => q.Tags)
                .Include(q => q.Answers)
                    .ThenInclude(a => a.User)
                .Include(q => q.Answers)
                    .ThenInclude(a => a.Votes)
                .Include(q => q.Answers)
                    .ThenInclude(a => a.Comments)
                        .ThenInclude(c => c.User)
                .Include(q => q.Votes)
                .FirstOrDefaultAsync(q => q.Id == id && q.IsActive);
        }

        public async Task<IEnumerable<Question>> GetQuestionsByUserAsync(string userId)
        {
            return await _context.Questions
                .Include(q => q.Tags)
                .Include(q => q.Answers)
                .Include(q => q.Votes)
                .Where(q => q.UserId == userId && q.IsActive)
                .OrderByDescending(q => q.CreatedDate)
                .ToListAsync();
        }

        public async Task IncrementViewCountAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                question.ViewCount++;
                await _context.SaveChangesAsync();
            }
        }
    }
}