using FuNews.Modals.DTOs.Request.ReportRequest;
using FuNews.Modals.DTOs.Response;
using FUNews.BLL.InterfaceService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FUNews.Web.Controllers
{
	public class ReportController : Controller
	{
		private readonly IReportService _reportService;

		public ReportController(IReportService reportService)
		{
			_reportService = reportService;
		}

		// GET: /Report/ArticleReport?startDate=...&endDate=...&groupBy=...
		public async Task<IActionResult> ArticleReport(DateTime? startDate, DateTime? endDate, string? groupBy)
		{
			var model = new ReportRequestModel
			{
				StartDate = startDate,
				EndDate = endDate,
				GroupBy = groupBy,
				ReportItems = null
			};

			// Kiểm tra tham số truyền vào
			if (!startDate.HasValue || !endDate.HasValue || string.IsNullOrEmpty(groupBy))
			{
				ModelState.AddModelError("", "Please provide startDate, endDate and groupBy parameters.");
				return View(model);
			}

			if (startDate > endDate)
			{
				ModelState.AddModelError("", "Start date must be earlier than or equal to end date");
				return View(model);
			}

			var allowedGroups = new[] { "day", "month", "year" };
			if (!allowedGroups.Contains(groupBy.ToLower()))
			{
				ModelState.AddModelError("", "Invalid groupBy value");
				return View(model);
			}

			try
			{
				var reportData = await _reportService.GetArticleReportAsync(startDate.Value, endDate.Value, groupBy);
				model.ReportItems = reportData; // ReportData nên là List<ReportItem> hoặc tương tự
			}
			catch (ArgumentException ex)
			{
				ModelState.AddModelError("", ex.Message);
			}
			catch (Exception)
			{
				ModelState.AddModelError("", "Internal server error");
			}

			return View(model);
		}
	}
}
