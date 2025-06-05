using Microsoft.AspNetCore.Mvc;
using FUNews.BLL.InterfaceService;
using FUNews.Modals.DTOs.Request;
using FUNews.Modals.DTOs.Response;

namespace FUNewsManagement.Controllers
{
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly ITagService _tagService;

        public TagController(ILogger<TagController> logger, ITagService tagService)
        {
            _logger = logger;
            _tagService = tagService;
        }

        // GET: /Tag
        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return View(tags);
        }

        // GET: /Tag/Create
        public IActionResult Create()
        {
            return View(new TagResponse());
        }

        // POST: /Tag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);
            await _tagService.CreateTagAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // Trả về PartialView chứa form Create (modal)
        [HttpGet]
        public IActionResult OpenCreateModal()
        {
            var model = new TagRequest();
            return PartialView("_CreateTagModal", model);
        }

        public async Task<IActionResult> GetEditModal(int id)
        {
            var tagResponse = await _tagService.GetTagByIdAsync(id);
            return PartialView("_EditTagModal", tagResponse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TagRequest request)
        {
            // if (!ModelState.IsValid)
            // {
            //     // Nếu validation lỗi, trả về lại partial để hiển thị lỗi ngay trong modal
            //     return PartialView("_EditTagModal", request);
            // }

            var updatedTag = await _tagService.UpdateTagById(request);

            // Trả về JSON chứa thông tin record đã update để JS phía client xử lý


            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tagService.DeleteTagByIdAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log exception
                return Json(new { 
                    success = false, 
                    message = "An error occurred while deleting the tag",
                    error = ex.Message // Trong môi trường production, không nên gửi chi tiết lỗi cho client 
                });
            }
        }
    }
}