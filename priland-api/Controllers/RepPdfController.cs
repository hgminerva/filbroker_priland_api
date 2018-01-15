using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace priland_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/pdf")] 
    public class RepPdfController : Controller
    {

        // GET: pdf/customer/5
        [HttpGet, Route("Customer/{id}")]
        public ActionResult PdfCustomer(int id)
        {
            return View();
        }

        // GET: pdf/broker/5
        [HttpGet, Route("Broker/{id}")]
        public ActionResult PdfBroker(int id)
        {
            return View();
        }

        // GET: pdf/checklist/5
        [HttpGet, Route("Checklist/{id}")]
        public ActionResult PdfChecklist(int id)
        {
            return View();
        }

        // GET: pdf/soldUnitProposal/5
        [HttpGet, Route("SoldUnitProposal/{id}")]
        public ActionResult PdfSoldUnitProposal(int id)
        {
            return View();
        }

        // GET: pdf/soldUnitContract/5
        [HttpGet, Route("SoldUnitContract/{id}")]
        public ActionResult PdfSoldUnitContract(int id)
        {
            return View();
        }
    }
}
