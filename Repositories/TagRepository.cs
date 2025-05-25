using Microsoft.EntityFrameworkCore;
using SoruCevapPortal.Models;

namespace SoruCevapPortal.Repositories
{
    public class TagRepository : GenericRepository<Tag>
    {
        public TagRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Tag> GetByNameAsync(string name)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Tag>> GetTagsWithQuestionCountAsync()
        {
            return await _context.Tags
                .Include(t => t.Questions)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }
        // Mevcut TagRepository.cs dosyasına eklenecek metod
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Tags.CountAsync();
        }
    }
}