using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace priland_api.Controllers
{
    public class RepPdfController : Controller
    {
        // ================
        // Global Variables
        // ================
        public MemoryStream workStream = new MemoryStream();
        public Rectangle rectangle = new Rectangle(PageSize.A3);

        // GET: pdf/customer/5
        public ActionResult PdfCustomer(int id)
        {
            // ==============================
            // PDF Settings and Customization
            // ==============================
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            document.Add(line);


            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
