using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuNews.Modals.DTOs.Response;

namespace FuNews.Modals.DTOs.Request.ReportRequest
{
	public class ReportRequestModel
	{
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string? GroupBy { get; set; }
		public List<ReportItem>? ReportItems { get; set; }
	}

}
