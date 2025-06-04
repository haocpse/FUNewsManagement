using AutoMapper;
using FuNews.Modals.DTOs.Response;
using FUNews.BLL.InterfaceService;
using FUNews.DAL.InterfaceRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.BLL.Service
{
	public class ReportService(INewsRepository articleRepo, IMapper mapper)
	  : IReportService
	{
		private readonly INewsRepository _articleRepo = articleRepo;
		private readonly IMapper _mapper = mapper;

		public async Task<List<ReportItem>> GetArticleReportAsync(DateTime startDate, DateTime endDate, string groupBy)
		{
			return await _articleRepo.GetReportByDateAsync(startDate, endDate, groupBy);
		}

		

	}
}