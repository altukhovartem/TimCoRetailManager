using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SaleController : ControllerBase
	{
		private readonly IConfiguration _config;

		public SaleController(IConfiguration config)
		{
			this._config = config;
		}

		[Authorize(Roles = "Cashier")]
		public void Post(SaleModel sale)
		{
			SaleData date = new SaleData(_config);
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			date.SaveSale(sale, userId);
		}

		[Authorize(Roles = "Admin,Manager")]
		[Route("GetSalesReport")]
		public List<SaleReportModel> GetSalesReport()
		{
			SaleData data = new SaleData(_config);
			return data.GetSaleReport();
		}
	}
}
