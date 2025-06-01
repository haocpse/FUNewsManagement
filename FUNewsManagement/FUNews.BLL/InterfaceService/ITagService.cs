using FUNews.DAL.Entity;
using FUNews.Modals.DTOs.Response;

namespace FUNews.BLL.InterfaceService;

public interface ITagService : IBaseService<Tag, int>
{
     Task<List<TagResponse>>  GetAllTagsAsync();
     Task<TagResponse?> GetTagByIdAsync(int id);
     Task<TagResponse?> UpdateTagById(int id);
     
}