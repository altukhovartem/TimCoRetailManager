﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
	[Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
		{
            SaleData date = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();
            date.SaveSale(sale, userId);
		}

        //public List<ProductModel> Get()
        //{
        //    ProductData data = new ProductData();
        //    return data.GetProducts();
        //}
    }
}
