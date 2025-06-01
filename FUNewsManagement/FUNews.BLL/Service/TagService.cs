using AutoMapper;
using FUNews.BLL.InterfaceService;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using FUNews.Modals.DTOs.Response;

namespace FUNews.BLL.Service
{
    public class TagService(ITagRepository tagRepository, IMapper mapper)
        : BaseService<Tag, int>(tagRepository), ITagService
    {
        private readonly ITagRepository _tagRepository = tagRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<TagResponse>> GetAllTagsAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return _mapper.Map<List<TagResponse>>(tags);
        }

        public async Task<TagResponse?> GetTagByIdAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            return tag == null ? null : _mapper.Map<TagResponse>(tag);
        }

        public async Task<TagResponse?> UpdateTagById(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return null;
            await _tagRepository.UpdateAsync(tag);
            return _mapper.Map<TagResponse>(tag);
        }
    }
}