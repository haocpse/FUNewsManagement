using Microsoft.EntityFrameworkCore;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;

namespace FUNews.DAL.Repository
{
    public class TagRepository(FUNewsDbContext context) 
        : BaseRepository<Tag, int>(context), ITagRepository
    {
        public async Task<(List<Tag> Items, int TotalCount)> GetPagedAsync(
            string keyword, int pageIndex, int pageSize)
        {
            var query = _context.Tags.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(t => t.TagName != null && t.TagName.Contains(keyword));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(t => t.TagName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (Items: items, TotalCount: totalCount);
        }

        public async Task<bool> IsUsedByAnyNewsAsync(int tagId)
        {
            return await _context.NewsTags.AnyAsync(nt => nt.TagId == tagId);
        }
    }
}