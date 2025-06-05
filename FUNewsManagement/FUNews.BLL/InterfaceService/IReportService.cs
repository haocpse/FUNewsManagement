using FuNews.Modals.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FUNews.BLL.InterfaceService
{
	public interface IReportService
	{
		Task<List<ReportItem>> GetArticleReportAsync(DateTime startDate, DateTime endDate, string groupBy);
	}
}
