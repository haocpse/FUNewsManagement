using FUNews.Modals.DTOs.Request;
using FUNews.Modals.DTOs.Response;

namespace FUNews.BLL.InterfaceService;

public interface ITagService 
{
     Task<List<TagResponse>>  GetAllTagsAsync();
     Task<TagResponse?> GetTagByIdAsync(int id);
     Task<TagResponse?> CreateTagAsync(TagRequest tagRequest);
     Task<TagResponse?> UpdateTagById(TagRequest request);
     Task DeleteTagByIdAsync(int id);
}