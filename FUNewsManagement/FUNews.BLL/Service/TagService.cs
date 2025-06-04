using AutoMapper;
using FUNews.BLL.InterfaceService;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using FUNews.Modals.DTOs.Request;
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

        public async Task<TagResponse?> CreateTagAsync(TagRequest tagRequest)
        {
            var tag = _mapper.Map<Tag>(tagRequest);
            await _tagRepository.AddAsync(tag);
            return _mapper.Map<TagResponse>(tag);
        }

        public async Task<TagResponse?> UpdateTagById(TagRequest tagRequest)
        {
            var tag = await _tagRepository.GetByIdAsync(tagRequest.TagId);
            if (tag == null) return null;

            // Map the updated properties from tagRequest to the existing tag
            _mapper.Map(tagRequest, tag);

            await _tagRepository.UpdateAsync(tag);
            return _mapper.Map<TagResponse>(tag);
        }

        public async Task DeleteTagByIdAsync(int id)
        {
            if (await _tagRepository.ExistsAsync(id))
            {
                await _tagRepository.DeleteAsync(id);
            }
            else
            {
                throw new KeyNotFoundException($"Tag with ID {id} not found.");
            }
        }
    }
}