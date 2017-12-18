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
    [RoutePrefix("api/TrnSoldUnitRequirement")]
    public class TrnSoldUnitRequirementController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        //List
        [HttpGet, Route("List")]
        public List<TrnSoldUnitRequirement> GetTrnSoldUnitRequirement()
        {
            var TrnSoldUnitRequirementData = from d in db.TrnSoldUnitRequirements
                                 select new TrnSoldUnitRequirement
                                 {
                                     Id = d.Id,
                                 };
            return TrnSoldUnitRequirementData.ToList();
        }
    }
}
