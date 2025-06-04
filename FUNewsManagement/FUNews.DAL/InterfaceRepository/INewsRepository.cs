using FuNews.Modals.DTOs.Response;
using FUNews.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL.InterfaceRepository
{
	public interface INewsRepository : IBaseRepository<NewsArticle, String>
	{
		Task<List<NewsArticle>> FindAllByDate(DateTime startDate, DateTime endDate);

		Task<List<ReportItem>> GetReportByDateAsync(DateTime startDate, DateTime endDate, string groupBy);


	}
}
