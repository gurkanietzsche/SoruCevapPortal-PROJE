using Microsoft.EntityFrameworkCore;
using SoruCevapPortal.Models;

namespace SoruCevapPortal.Repositories
{
    public class AnswerRepository : GenericRepository<Answer>
    {
        public AnswerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionAsync(int questionId)
        {
            return await _context.Answers
                .Include(a => a.User)
                .Include(a => a.Votes)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .Where(a => a.QuestionId == questionId && a.IsActive)
                .OrderByDescending(a => a.IsAccepted)
                .ThenByDescending(a => a.CreatedDate)
                .ToListAsync();
        }
        // Mevcut AnswerRepository.cs dosyasına eklenecek metod
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Answers.CountAsync();
        }
        public async Task<Answer> GetAnswerWithDetailsAsync(int id)
        {
            return await _context.Answers
                .Include(a => a.User)
                .Include(a => a.Votes)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
        }

        public async Task AcceptAnswerAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer != null)
            {
                var previouslyAccepted = await _context.Answers
                    .Where(a => a.QuestionId == answer.QuestionId && a.IsAccepted)
                    .FirstOrDefaultAsync();
                if (previouslyAccepted != null)
                {
                    previouslyAccepted.IsAccepted = false;
                }

                answer.IsAccepted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}