using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using priland_api.Models;

namespace priland_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/MstUnitPrice")]
    public class MstUnitPriceController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        //List per Unit Id
        [HttpGet, Route("ListPerUnitId/{id}")]
        public List<MstUnitPrice> GetMstUnitPricePerUnitId(string id)
        {
            var MstUnitPriceData = from d in db.MstUnitPrices
                                   where d.UnitId == Convert.ToInt32(id)
                                   orderby d.PriceDate descending
                                   select new MstUnitPrice
                                   {

                                      Id = d.Id,
                                      UnitId = d.UnitId,
                                      PriceDate = d.PriceDate.ToShortDateString(),
                                      Price = d.Price
                                   };
            return MstUnitPriceData.ToList();
        }
    }
}
