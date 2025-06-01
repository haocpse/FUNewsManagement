using FUNews.BLL.InterfaceService;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;

namespace FUNews.BLL.Service
{
    
    public class TagService(ITagRepository tagRepository) 
        : BaseService<Tag, int>(tagRepository), ITagService
    {
        
        private readonly ITagRepository _tagRepository = tagRepository;

        
    }
}