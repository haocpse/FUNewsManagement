using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FUNewsManagement.Models;
using FUNews.BLL.InterfaceService;
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

        // // POST: /Tag/Create
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create(TagResponse model)
        // {
        //     if (!ModelState.IsValid)
        //         return View(model);
        //
        //     // Giả sử ITagService.CreateAsync nhận TagResponse
        //     await _tagService.CreateAsync(model);
        //     return RedirectToAction(nameof(Index));
        // }

        // GET: /Tag/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
                return NotFound();

            return View(tag);
        }

        // // POST: /Tag/Edit/5
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(TagResponse model)
        // {
        //     if (!ModelState.IsValid)
        //         return View(model);
        //
        //     await _tagService.UpdateAsync(model);
        //     return RedirectToAction(nameof(Index));
        // }

        // GET: /Tag/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
                return NotFound();

            return View(tag);
        }

        // POST: /Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tagService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // public IActionResult Error()
        // {
        //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }
    }
}
