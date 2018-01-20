using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

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

            // =====
            // Fonts
            // =====
            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font fontArial13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);

            // line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            var SysSettingsData = from d in db.SysSettings
                                  select d;

            if (SysSettingsData.Any())
            {
                // ===========
             // Header Page
             // ===========
                PdfPTable headerPage = new PdfPTable(2);
                float[] widthsCellsHeaderPage = new float[] { 100f, 100f };
                headerPage.SetWidths(widthsCellsHeaderPage);
                headerPage.WidthPercentage = 100;
                headerPage.AddCell(new PdfPCell(new Phrase(SysSettingsData.FirstOrDefault().Company, fontArial17Bold)) { Border = 0 });
                headerPage.AddCell(new PdfPCell(new Phrase("INDIVIDUAL CUSTOMER INFORMATION SHEET", fontArial17Bold)) { Border=0 });

                headerPage.AddCell(new PdfPCell(new Phrase(SysSettingsData.FirstOrDefault().SoftwareVersion, fontArial11)) {  PaddingTop = 5f, Border = 0 });
                headerPage.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) {  PaddingTop = 5f, Border = 0 });

                headerPage.AddCell(new PdfPCell(new Phrase(SysSettingsData.FirstOrDefault().SoftwareDeveloper, fontArial11)) {  PaddingTop = 5f, Border = 0 });
                headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) {  PaddingTop = 5f, Border = 0 });
                document.Add(headerPage);

                //document.Add(line);


                // =====
                // Space
                // =====
                PdfPTable spaceTable = new PdfPTable(1);
                float[] widthCellsSpaceTable = new float[] { 100f };
                spaceTable.SetWidths(widthCellsSpaceTable);
                spaceTable.WidthPercentage = 100;
                spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) {  PaddingTop = 5f });
                
            }

            var Customer = from d in db.MstCustomers
                           where d.Id == Convert.ToInt32(id)
                           && d.IsLocked == true
                           select d;
            if (Customer.Any())
            {
                String CustomerCode = Customer.FirstOrDefault().CustomerCode;
                String LastName = Customer.FirstOrDefault().LastName;
                String FirstName = Customer.FirstOrDefault().FirstName;
                String MiddleName = Customer.FirstOrDefault().MiddleName;
                String CivilStatus = Customer.FirstOrDefault().CivilStatus;
                String Gender = Customer.FirstOrDefault().Gender;
                String BirthDate = Customer.FirstOrDefault().BirthDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String TIN = Customer.FirstOrDefault().TIN;
                String IdType = Customer.FirstOrDefault().IdType;
                String IdNumber = Customer.FirstOrDefault().IdNumber;
                String Address = Customer.FirstOrDefault().Address;
                String City = Customer.FirstOrDefault().City;
                String Province = Customer.FirstOrDefault().Province;
                String Country = Customer.FirstOrDefault().Country;
                String ZipCode = Customer.FirstOrDefault().ZipCode;
                String EmailAddress = Customer.FirstOrDefault().EmailAddress;
                String TelephoneNumber = Customer.FirstOrDefault().TelephoneNumber;
                String MobileNumber = Customer.FirstOrDefault().MobileNumber;
                String Employer = Customer.FirstOrDefault().Employer;
                String EmployerIndustry = Customer.FirstOrDefault().EmployerIndustry;
                String NoOfYearsEmployed = Customer.FirstOrDefault().NoOfYearsEmployed.ToString();
                String Position = Customer.FirstOrDefault().Position;
                String EmploymentStatus = Customer.FirstOrDefault().EmploymentStatus;
                String EmployerAddress = Customer.FirstOrDefault().EmployerAddress;
                String EmployerCity = Customer.FirstOrDefault().EmployerCity;
                String EmployerProvince = Customer.FirstOrDefault().EmployerProvince;
                String EmployerCountry = Customer.FirstOrDefault().EmployerCountry;
                String EmployerZipCode = Customer.FirstOrDefault().EmploymentStatus;
                String EmployerTelephoneNumber = Customer.FirstOrDefault().EmployerTelephoneNumber;
                String EmployerMobileNumber = Customer.FirstOrDefault().EmployerMobileNumber;
                String Remarks = Customer.FirstOrDefault().Remarks;
                String Status = Customer.FirstOrDefault().Status;
                String Picture = Customer.FirstOrDefault().Picture;

                PdfPTable tableCustomer = new PdfPTable(6);
                float[] widthscellsTablePurchaseOrder = new float[] { 50f, 50f, 50f, 50f,50f,50f };
                tableCustomer.SetWidths(widthscellsTablePurchaseOrder);
                tableCustomer.WidthPercentage = 100;
                

                //Image
                string imagepath = Server.MapPath("~/Data/innosofticon.png");
                Image logo = Image.GetInstance(imagepath);
                logo.ScalePercent(50f);
                PdfPCell imageCell = new PdfPCell(logo);
                //tableCustomer.AddCell(new PdfPCell(new Phrase("   ", fontArial11Bold)) { Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                //tableCustomer.AddCell(new PdfPCell(new Phrase(Picture, fontArial11Bold)) { Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
                //tableCustomer.AddCell(new PdfPCell(new Phrase("   ", fontArial11Bold)) { Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });

                tableCustomer.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 2, Border = 0 });
                tableCustomer.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 2, Border = 0 });
                tableCustomer.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 1, Border = 0 });
                tableCustomer.AddCell(new PdfPCell(imageCell) { HorizontalAlignment = 2, Colspan=1, Border=0});
                
                tableCustomer.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 6, Border = 0 });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Last Name ", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase("First Name", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Midle Name", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(LastName, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(FirstName, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(MiddleName, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });


                tableCustomer.AddCell(new PdfPCell(new Phrase("License No.", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Sex", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Civil Status", fontArial9Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Birth Date", fontArial9Bold)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(IdNumber, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(Gender, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(CivilStatus, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(BirthDate, fontArial11)) { Border = 0, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });


                tableCustomer.AddCell(new PdfPCell(new Phrase("Residence & Postal Address", fontArial9Bold)) { Border = 0, Colspan = 4, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Zip Code", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(Address, fontArial11)) { Border = 0, Colspan = 4, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(ZipCode, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });


                tableCustomer.AddCell(new PdfPCell(new Phrase("City", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Province", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Country", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(City, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(Province, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(Country, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });


                tableCustomer.AddCell(new PdfPCell(new Phrase("Telephone Number", fontArial9Bold)) { Border = 0, Colspan = 3, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Mobile Phone No.", fontArial9Bold)) { Border = 0, Colspan = 3, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(TelephoneNumber, fontArial11)) { Border = 0, Colspan = 3, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(MobileNumber, fontArial11)) { Border = 0, Colspan = 3, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });


                tableCustomer.AddCell(new PdfPCell(new Phrase("Email Address", fontArial9Bold)) { Border = 0, Colspan = 3, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase("TIN", fontArial9Bold)) { Border = 0, Colspan = 3, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(EmailAddress, fontArial11)) { Border = 0, Colspan = 3, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });
                tableCustomer.AddCell(new PdfPCell(new Phrase(TIN, fontArial11)) { Border = 0, Colspan = 3, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f,  });

                document.Add(line);

                tableCustomer.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 6,Border = 0 });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Employement Information", fontArial13Bold)) { Colspan = 6,Border = 0 });
                tableCustomer.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 6,Border = 0 });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Name of Realty Firm", fontArial11Bold)) { Border = 0, Colspan = 6 , PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(Employer, fontArial11)) { Border = 0, Colspan = 6 , PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Position", fontArial11Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Number of Years Employed", fontArial11Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Status", fontArial11Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                tableCustomer.AddCell(new PdfPCell(new Phrase(Position, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(NoOfYearsEmployed, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(EmploymentStatus, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Office Address", fontArial11Bold)) { Border = 0, Colspan = 4, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Zip Code", fontArial11Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, });
                tableCustomer.AddCell(new PdfPCell(new Phrase(EmployerAddress, fontArial11)) { Border = 0, Colspan = 4, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, });
                tableCustomer.AddCell(new PdfPCell(new Phrase(EmployerZipCode, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f, });

                tableCustomer.AddCell(new PdfPCell(new Phrase("City", fontArial11Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Province", fontArial11Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Country", fontArial11Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(EmployerCity, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(EmployerProvince, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(EmployerCountry, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                tableCustomer.AddCell(new PdfPCell(new Phrase("Telephone Number", fontArial11Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase("Mobile Nubmer", fontArial11Bold)) { Border = 0, Colspan = 2,  PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase("   ", fontArial11Bold)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(EmployerTelephoneNumber, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase(EmployerMobileNumber, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableCustomer.AddCell(new PdfPCell(new Phrase("   ", fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(tableCustomer);

                //document.Add(spaceTable);
            }



            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        // GET: pdf/broker/5
        //[HttpGet, Route("Broker/{id}")]
        public ActionResult PdfBroker(int id)
        {
            // ==============================
            // PDF Settings and Customization
            // ==============================
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            // =====
            // Fonts
            // =====
            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font fontArial13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);

            // line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            var SysSettingsData = from d in db.SysSettings
                                  select d;

            if (SysSettingsData.Any())
            {
                // ===========
                // Header Page
                // ===========
                PdfPTable headerPage = new PdfPTable(2);
                float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
                headerPage.SetWidths(widthsCellsHeaderPage);
                headerPage.WidthPercentage = 100;
                headerPage.AddCell(new PdfPCell(new Phrase(SysSettingsData.FirstOrDefault().Company, fontArial17Bold)) { Border = 0 });
                headerPage.AddCell(new PdfPCell(new Phrase("BROKER ACCREDITATION FORM", fontArial17Bold)) { Border = 0 });

                headerPage.AddCell(new PdfPCell(new Phrase(SysSettingsData.FirstOrDefault().SoftwareVersion, fontArial11)) { PaddingTop = 5f, Border = 0 });
                headerPage.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { PaddingTop = 5f, Border = 0 });

                headerPage.AddCell(new PdfPCell(new Phrase(SysSettingsData.FirstOrDefault().SoftwareDeveloper, fontArial11)) { PaddingTop = 5f, Border = 0 });
                headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { PaddingTop = 5f, Border = 0 });
                document.Add(headerPage);

                document.Add(line);


                // =====
                // Space
                // =====
                PdfPTable spaceTable = new PdfPTable(1);
                float[] widthCellsSpaceTable = new float[] { 100f };
                spaceTable.SetWidths(widthCellsSpaceTable);
                spaceTable.WidthPercentage = 100;
                spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f });

            }

            var Broker = from d in db.MstBrokers
                         where d.Id == Convert.ToInt32(id)
                         && d.IsLocked == true
                         select d;
            if (Broker.Any())
            {
                String BrokerCode = Broker.FirstOrDefault().BrokerCode;
                String LastName = Broker.FirstOrDefault().LastName;
                String FirstName = Broker.FirstOrDefault().FirstName;
                String MiddleName = Broker.FirstOrDefault().MiddleName;
                String LicenseNumber = Broker.FirstOrDefault().LicenseNumber;
                DateTime BirthDate = Broker.FirstOrDefault().BirthDate;/*.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);*/
                String CivilStatus = Broker.FirstOrDefault().CivilStatus;
                String Gender = Broker.FirstOrDefault().Gender;
                String Address = Broker.FirstOrDefault().Address;
                String TelephoneNumber = Broker.FirstOrDefault().TelephoneNumber;
                String MobileNumber = Broker.FirstOrDefault().MobileNumber;
                String Religion = Broker.FirstOrDefault().Religion;
                String EmailAddress = Broker.FirstOrDefault().EmailAddress;
                String Facebook = Broker.FirstOrDefault().Facebook;
                String TIN = Broker.FirstOrDefault().TIN;
                String RealtyFirm = Broker.FirstOrDefault().RealtyFirm;
                String RealtyFirmAddress = Broker.FirstOrDefault().RealtyFirmAddress;
                String RealtyFirmTelephoneNumber = Broker.FirstOrDefault().RealtyFirmTelephoneNumber;
                String RealtyFirmMobileNumber = Broker.FirstOrDefault().RealtyFirmMobileNumber;
                String RealtyFirmFaxNumber = Broker.FirstOrDefault().RealtyFirmFaxNumber;
                String RealtyFirmEmailAddress = Broker.FirstOrDefault().RealtyFirmEmailAddress;
                String RealtyFirmWebsite = Broker.FirstOrDefault().RealtyFirmWebsite;
                String RealtyFirmTIN = Broker.FirstOrDefault().RealtyFirmTIN;
                String Organization = Broker.FirstOrDefault().Organization;
                String Remarks = Broker.FirstOrDefault().Remarks;
                String Status = Broker.FirstOrDefault().Status;

                PdfPTable tableBroker = new PdfPTable(6);
                float[] widthscellsTablePurchaseOrder = new float[] { 50f, 50f, 50f, 50f, 50f, 50f };
                tableBroker.SetWidths(widthscellsTablePurchaseOrder);
                tableBroker.WidthPercentage = 100;


                //Image
                string imagepath = Server.MapPath("~/Data/innosofticon.png");
                Image logo = Image.GetInstance(imagepath);
                logo.ScalePercent(50f);
                PdfPCell imageCell = new PdfPCell(logo);


                tableBroker.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 2, Border = 0 });
                tableBroker.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 2, Border = 0 });
                tableBroker.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 1, Border = 0 });
                tableBroker.AddCell(new PdfPCell(imageCell) { HorizontalAlignment = 2, Colspan = 1, Border = 0 });


                tableBroker.AddCell(new PdfPCell(new Phrase("Last Name ", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("First Name", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Midle Name", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(LastName, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(FirstName, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(MiddleName, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });


                DateTime now = DateTime.Today;
                int age = now.Year - BirthDate.Year;
                if (now < BirthDate.AddYears(age)) age--;

                tableBroker.AddCell(new PdfPCell(new Phrase("License No.", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Age", fontArial9Bold)) { Border = 0, Colspan = 1, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Gender", fontArial9Bold)) { Border = 0, Colspan = 1, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Civil Status", fontArial9Bold)) { Border = 0, Colspan = 1, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Birth Date", fontArial9Bold)) { Border = 0, Colspan = 1, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(LicenseNumber, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(age.ToString(), fontArial11)) { Border = 0, Colspan = 1, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(Gender, fontArial11)) { Border = 0, Colspan = 1, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(CivilStatus, fontArial11)) { Border = 0, Colspan = 1, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(BirthDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), fontArial11)) { Border = 0, Colspan = 1, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });


                tableBroker.AddCell(new PdfPCell(new Phrase("Home Phone No.", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Mobile Phone No.", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Religion", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(TelephoneNumber, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(MobileNumber, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(Religion, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });


                tableBroker.AddCell(new PdfPCell(new Phrase("Email Address", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Facebook", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("TIN No.", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(EmailAddress, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(Facebook, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(TIN, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });


                tableBroker.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 6, Border = 0 });
                tableBroker.AddCell(new PdfPCell(new Phrase("Employement Information", fontArial13Bold)) { Colspan = 6, Border = 0 });
                tableBroker.AddCell(new PdfPCell(new Phrase("   ", fontArial17Bold)) { Colspan = 6, Border = 0 });


                tableBroker.AddCell(new PdfPCell(new Phrase("Name of Realty Firm", fontArial9Bold)) { Colspan = 6, Border = 0 });
                tableBroker.AddCell(new PdfPCell(new Phrase(RealtyFirm, fontArial11)) { Colspan = 6, Border = 0 });


                tableBroker.AddCell(new PdfPCell(new Phrase("Office Address", fontArial9Bold)) { Colspan = 6, Border = 0 });
                tableBroker.AddCell(new PdfPCell(new Phrase(RealtyFirmAddress, fontArial11)) { Colspan = 6, Border = 0 });

                tableBroker.AddCell(new PdfPCell(new Phrase("Office No.", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Mobile Phone No.", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Fax No.", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(RealtyFirmTelephoneNumber, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(RealtyFirmMobileNumber, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(RealtyFirmFaxNumber, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });


                tableBroker.AddCell(new PdfPCell(new Phrase("Office Email", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("Website ", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase("TIN No.", fontArial9Bold)) { Border = 0, Colspan = 2, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(RealtyFirmEmailAddress, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(RealtyFirmWebsite, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBroker.AddCell(new PdfPCell(new Phrase(RealtyFirmTIN, fontArial11)) { Border = 0, Colspan = 2, PaddingTop = 5f, PaddingLeft = 5f, PaddingRight = 5f });


                tableBroker.AddCell(new PdfPCell(new Phrase("Real State Organization", fontArial9Bold)) { Colspan = 6, Border = 0 });
                tableBroker.AddCell(new PdfPCell(new Phrase(Organization, fontArial11)) { Colspan = 6, Border = 0 });


                document.Add(tableBroker);
            }


            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
