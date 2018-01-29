using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Http.Headers;
using System.Web.Http.Results;
using System.Globalization;

namespace priland_api.Controllers
{
    // ======
    // Prefix
    // ======
    [RoutePrefix("api/PDF")]
    public class RepPdfController : ApiController
    {
        // ====================
        // PDF Global Variables
        // ====================
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();
        private MemoryStream workStream = new MemoryStream();
        private Document document = new Document(new Rectangle(PageSize.A3), 72, 72, 72, 72);

        // ====================
        // PDF Customized Fonts
        // ====================
        private Font fontArial5Bold = FontFactory.GetFont("Arial", 5, Font.BOLD);
        private Font fontArial5 = FontFactory.GetFont("Arial", 5);
        private Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
        private Font fontArial9 = FontFactory.GetFont("Arial", 9);
        private Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
        private Font fontArial10BoldItalic = FontFactory.GetFont("Arial", 10, Font.BOLDITALIC, BaseColor.WHITE);
        private Font fontArial10 = FontFactory.GetFont("Arial", 10);
        private Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
        private Font fontArial11 = FontFactory.GetFont("Arial", 11);
        private Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
        private Font fontArial12 = FontFactory.GetFont("Arial", 12);
        private Font fontArial13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);
        private Font fontArial13 = FontFactory.GetFont("Arial", 13);
        private Font fontArial14Bold = FontFactory.GetFont("Arial", 14, Font.BOLD);
        private Font fontArial14 = FontFactory.GetFont("Arial", 14);
        private Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
        private Font fontArial17 = FontFactory.GetFont("Arial", 17);

        // ========
        // PDF Line
        // ========
        private Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

        // ============
        // PDF Customer
        // ============
        [HttpGet, Route("Customer/{id}")]
        public HttpResponseMessage PdfCustomer(string id)
        {
            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            // =============
            // Open Document
            // =============
            document.Open();

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f, Border = 0 });

            // ===============
            // Settings (Data)
            // ===============
            var sysSettings = from d in db.SysSettings
                              select d;

            if (sysSettings.Any())
            {
                // ===============
                // Company Details
                // ===============
                PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
                pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
                pdfTableCompanyDetail.WidthPercentage = 100;
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase(sysSettings.FirstOrDefault().Company, fontArial17Bold)) { Border = 0 });
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("Individual Customer Information Sheet", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
                document.Add(pdfTableCompanyDetail);
                document.Add(line);

                document.Add(spaceTable);
            }

            // ============
            // Get Customer
            // ============
            var customer = from d in db.MstCustomers
                           where d.Id == Convert.ToInt32(id)
                           select d;

            if (customer.Any())
            {
                // =============
                // Customer Data
                // =============
                String CustomerCode = customer.FirstOrDefault().CustomerCode;
                String LastName = customer.FirstOrDefault().LastName;
                String FirstName = customer.FirstOrDefault().FirstName;
                String MiddleName = customer.FirstOrDefault().MiddleName;
                String CivilStatus = customer.FirstOrDefault().CivilStatus;
                String Gender = customer.FirstOrDefault().Gender;
                String BirthDate = customer.FirstOrDefault().BirthDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String TIN = customer.FirstOrDefault().TIN;
                String IdType = customer.FirstOrDefault().IdType;
                String IdNumber = customer.FirstOrDefault().IdNumber;
                String Address = customer.FirstOrDefault().Address;
                String City = customer.FirstOrDefault().City;
                String Province = customer.FirstOrDefault().Province;
                String Country = customer.FirstOrDefault().Country;
                String ZipCode = customer.FirstOrDefault().ZipCode;
                String EmailAddress = customer.FirstOrDefault().EmailAddress;
                String TelephoneNumber = customer.FirstOrDefault().TelephoneNumber;
                String MobileNumber = customer.FirstOrDefault().MobileNumber;
                String Employer = customer.FirstOrDefault().Employer;
                String EmployerIndustry = customer.FirstOrDefault().EmployerIndustry;
                String NoOfYearsEmployed = customer.FirstOrDefault().NoOfYearsEmployed.ToString();
                String Position = customer.FirstOrDefault().Position;
                String EmploymentStatus = customer.FirstOrDefault().EmploymentStatus;
                String EmployerAddress = customer.FirstOrDefault().EmployerAddress;
                String EmployerCity = customer.FirstOrDefault().EmployerCity;
                String EmployerProvince = customer.FirstOrDefault().EmployerProvince;
                String EmployerCountry = customer.FirstOrDefault().EmployerCountry;
                String EmployerZipCode = customer.FirstOrDefault().EmploymentStatus;
                String EmployerTelephoneNumber = customer.FirstOrDefault().EmployerTelephoneNumber;
                String EmployerMobileNumber = customer.FirstOrDefault().EmployerMobileNumber;
                String Remarks = customer.FirstOrDefault().Remarks;
                String Status = customer.FirstOrDefault().Status;
                String Picture = customer.FirstOrDefault().Picture;

                // ======================
                // Table Customer (BUYER)
                // ======================
                PdfPTable pdfTableCustomerBuyer = new PdfPTable(4);
                pdfTableCustomerBuyer.SetWidths(new float[] { 100f, 50f, 50f, 100f });
                pdfTableCustomerBuyer.WidthPercentage = 100;

                pdfTableCustomerBuyer.AddCell(new PdfPCell(new Phrase("BUYER", fontArial10BoldItalic)) { Colspan = 4, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f, BackgroundColor = BaseColor.BLACK });

                Phrase lastNamePhraseLabel = new Phrase("Last Name \n\n", fontArial10Bold);
                Phrase lastNamePhraseData = new Phrase(LastName, fontArial13);
                Paragraph lastNameParagraph = new Paragraph
                {
                    lastNamePhraseLabel, lastNamePhraseData
                };

                Phrase firstNamePhraseLabel = new Phrase("First Name \n\n", fontArial10Bold);
                Phrase firstNamePhraseData = new Phrase(FirstName, fontArial13);
                Paragraph firstNameParagraph = new Paragraph
                {
                    firstNamePhraseLabel, firstNamePhraseData
                };

                Phrase middleNamePhraseLabel = new Phrase("Middle Name \n\n", fontArial10Bold);
                Phrase middleNamePhraseData = new Phrase(MiddleName, fontArial13);
                Paragraph middleNameParagraph = new Paragraph
                {
                    middleNamePhraseLabel, middleNamePhraseData
                };

                pdfTableCustomerBuyer.AddCell(new PdfPCell(lastNameParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerBuyer.AddCell(new PdfPCell(firstNameParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerBuyer.AddCell(new PdfPCell(middleNameParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase civilStatusPhraseLabel = new Phrase("Civil Status \n\n", fontArial10Bold);
                Phrase civilStatusPhraseData = new Phrase(CivilStatus, fontArial13);
                Paragraph civilStatusParagraph = new Paragraph
                {
                    civilStatusPhraseLabel, civilStatusPhraseData
                };

                Phrase genderPhraseLabel = new Phrase("Gender \n\n", fontArial10Bold);
                Phrase genderPhraseData = new Phrase(Gender, fontArial13);
                Paragraph genderParagraph = new Paragraph
                {
                    genderPhraseLabel, genderPhraseData
                };

                Phrase birthDatePhraseLabel = new Phrase("Birth Date \n\n", fontArial10Bold);
                Phrase birthDatePhraseData = new Phrase(BirthDate, fontArial13);
                Paragraph birthDateParagraph = new Paragraph
                {
                    birthDatePhraseLabel, birthDatePhraseData
                };

                Phrase citizenShipPhraseLabel = new Phrase("Citizenship \n\n", fontArial10Bold);
                Phrase citizenShipPhraseData = new Phrase(" ", fontArial13);
                Paragraph citizenShipParagraph = new Paragraph
                {
                    citizenShipPhraseLabel, citizenShipPhraseData
                };

                pdfTableCustomerBuyer.AddCell(new PdfPCell(civilStatusParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerBuyer.AddCell(new PdfPCell(genderParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerBuyer.AddCell(new PdfPCell(birthDateParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerBuyer.AddCell(new PdfPCell(citizenShipParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase taxIdentificationNoPhraseLabel = new Phrase("Tax Identification No. \n\n", fontArial10Bold);
                Phrase taxIdentificationNoPhraseData = new Phrase(TIN, fontArial13);
                Paragraph taxIdentificationNoParagraph = new Paragraph
                {
                    taxIdentificationNoPhraseLabel, taxIdentificationNoPhraseData
                };


                Phrase idTypePhraseLabel = new Phrase("ID Type \n\n", fontArial10Bold);
                Phrase idTypePhraseData = new Phrase(IdType, fontArial13);
                Paragraph idTypeParagraph = new Paragraph
                {
                    idTypePhraseLabel, idTypePhraseData
                };

                Phrase idNumberPhraseLabel = new Phrase("ID Number \n\n", fontArial10Bold);
                Phrase idNumberPhraseData = new Phrase(IdNumber, fontArial13);
                Paragraph idNumberParagraph = new Paragraph
                {
                    idNumberPhraseLabel, idNumberPhraseData
                };

                pdfTableCustomerBuyer.AddCell(new PdfPCell(taxIdentificationNoParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerBuyer.AddCell(new PdfPCell(idTypeParagraph) { PaddingTop = 3f, Colspan = 2, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerBuyer.AddCell(new PdfPCell(idNumberParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(pdfTableCustomerBuyer);

                // ================================
                // Table Customer (CONTACT DETAILS)
                // ================================
                PdfPTable pdfTableCustomerContactDetails = new PdfPTable(4);
                pdfTableCustomerContactDetails.SetWidths(new float[] { 100f, 100f, 100f, 100f });
                pdfTableCustomerContactDetails.WidthPercentage = 100;

                pdfTableCustomerContactDetails.AddCell(new PdfPCell(new Phrase("CONTACT DETAILS", fontArial10BoldItalic)) { Colspan = 4, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f, BackgroundColor = BaseColor.BLACK });

                Phrase homeAddressPhraseLabel = new Phrase("Home Address \n\n", fontArial10Bold);
                Phrase homeAddressPhraseData = new Phrase(Address, fontArial13);
                Paragraph homeAddressParagraph = new Paragraph
                {
                    homeAddressPhraseLabel, homeAddressPhraseData
                };

                pdfTableCustomerContactDetails.AddCell(new PdfPCell(homeAddressParagraph) { Colspan = 4, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase municipalityCityPhraseLabel = new Phrase("Municipality / City \n\n", fontArial10Bold);
                Phrase municipalityCityPhraseData = new Phrase(City, fontArial13);
                Paragraph municipalityCityParagraph = new Paragraph
                {
                    municipalityCityPhraseLabel, municipalityCityPhraseData
                };

                Phrase provinceStatePhraseLabel = new Phrase("Province / State \n\n", fontArial10Bold);
                Phrase provinceStatePhraseData = new Phrase(Province, fontArial13);
                Paragraph provinceStateParagraph = new Paragraph
                {
                    provinceStatePhraseLabel, provinceStatePhraseData
                };

                Phrase countryPhraseLabel = new Phrase("Country \n\n", fontArial10Bold);
                Phrase countryPhraseData = new Phrase(Country, fontArial13);
                Paragraph countryParagraph = new Paragraph
                {
                    countryPhraseLabel, countryPhraseData
                };

                Phrase postalPhraseLabel = new Phrase("Postal \n\n", fontArial10Bold);
                Phrase postalPhraseData = new Phrase(ZipCode, fontArial13);
                Paragraph postalParagraph = new Paragraph
                {
                    postalPhraseLabel, postalPhraseData
                };

                pdfTableCustomerContactDetails.AddCell(new PdfPCell(municipalityCityParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerContactDetails.AddCell(new PdfPCell(provinceStateParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerContactDetails.AddCell(new PdfPCell(countryParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerContactDetails.AddCell(new PdfPCell(postalParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase emailAddressPhraseLabel = new Phrase("Email Address \n\n", fontArial10Bold);
                Phrase emailAddressPhraseData = new Phrase(EmailAddress, fontArial13);
                Paragraph emailAddressParagraph = new Paragraph
                {
                    emailAddressPhraseLabel, emailAddressPhraseData
                };

                Phrase landLineNoPhraseLabel = new Phrase("Landline No. \n\n", fontArial10Bold);
                Phrase landLineNoPhraseData = new Phrase(TelephoneNumber, fontArial13);
                Paragraph landLineNoParagraph = new Paragraph
                {
                    landLineNoPhraseLabel, landLineNoPhraseData
                };

                Phrase mobileNoPhraseLabel = new Phrase("Mobile No. \n\n", fontArial10Bold);
                Phrase mobileNoPhraseData = new Phrase(MobileNumber, fontArial13);
                Paragraph mobileNoParagraph = new Paragraph
                {
                    mobileNoPhraseLabel, mobileNoPhraseData
                };

                document.Add(pdfTableCustomerContactDetails);

                // =============================================================
                // Email, Telephone and Mobile Numbers (BUYER'S CONTACT DETAILS)
                // =============================================================
                PdfPTable pdfTableCustomerContactEmailAndNumbers = new PdfPTable(3);
                pdfTableCustomerContactEmailAndNumbers.SetWidths(new float[] { 100f, 100f, 100f });
                pdfTableCustomerContactEmailAndNumbers.WidthPercentage = 100;

                pdfTableCustomerContactEmailAndNumbers.AddCell(new PdfPCell(emailAddressParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerContactEmailAndNumbers.AddCell(new PdfPCell(landLineNoParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerContactEmailAndNumbers.AddCell(new PdfPCell(mobileNoParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(pdfTableCustomerContactEmailAndNumbers);

                // ===================================
                // Table Customer (EMPLOYMENT DETAILS)
                // ===================================
                PdfPTable pdfTableCustomerEmploymentDetails = new PdfPTable(4);
                pdfTableCustomerEmploymentDetails.SetWidths(new float[] { 100f, 100f, 100f, 100f });
                pdfTableCustomerEmploymentDetails.WidthPercentage = 100;

                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(new Phrase("EMPLOYMENT DETAILS", fontArial10BoldItalic)) { Colspan = 4, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f, BackgroundColor = BaseColor.BLACK });

                Phrase companyNamePhraseLabel = new Phrase("Company Name \n\n", fontArial10Bold);
                Phrase companyNamePhraseData = new Phrase(Employer, fontArial13);
                Paragraph companyNameParagraph = new Paragraph
                {
                    companyNamePhraseLabel, companyNamePhraseData
                };

                Phrase natureOfBusinessPhraseLabel = new Phrase("Nature of Business \n\n", fontArial10Bold);
                Phrase natureOfBusinessPhraseData = new Phrase(EmployerIndustry, fontArial13);
                Paragraph natureOfBusinessParagraph = new Paragraph
                {
                    natureOfBusinessPhraseLabel, natureOfBusinessPhraseData
                };

                Phrase yearsEmployedInBusinessPhraseLabel = new Phrase("Years Employed / In Business \n\n", fontArial10Bold);
                Phrase yearsEmployedInBusinessPhraseData = new Phrase(NoOfYearsEmployed, fontArial13);
                Paragraph yearsEmployedInBusinessParagraph = new Paragraph
                {
                    yearsEmployedInBusinessPhraseLabel, yearsEmployedInBusinessPhraseData
                };

                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(companyNameParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(natureOfBusinessParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(yearsEmployedInBusinessParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase positionPhraseLabel = new Phrase("Position \n\n", fontArial10Bold);
                Phrase positionPhraseData = new Phrase(Position, fontArial13);
                Paragraph positionParagraph = new Paragraph
                {
                    positionPhraseLabel, positionPhraseData
                };

                Phrase employmentStatusPhraseLabel = new Phrase("Employment Status \n\n", fontArial10Bold);
                Phrase employmentStatusPhraseData = new Phrase(EmploymentStatus, fontArial13);
                Paragraph employmentStatusParagraph = new Paragraph
                {
                    employmentStatusPhraseLabel, employmentStatusPhraseData
                };

                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(positionParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(employmentStatusParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase employmentAddressPhraseLabel = new Phrase("No. Street Brgy. / Subdivision \n\n", fontArial10Bold);
                Phrase employmentAddressPhraseData = new Phrase(EmployerAddress, fontArial13);
                Paragraph employmentAddressParagraph = new Paragraph
                {
                    employmentAddressPhraseLabel, employmentAddressPhraseData
                };

                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(employmentAddressParagraph) { Colspan = 4, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase employmentMunicipalityCityPhraseLabel = new Phrase("Municipality / City \n\n", fontArial10Bold);
                Phrase employmentMunicipalityCityPhraseData = new Phrase(EmployerCity, fontArial13);
                Paragraph employmentMunicipalityCityParagraph = new Paragraph
                {
                    employmentMunicipalityCityPhraseLabel, employmentMunicipalityCityPhraseData
                };

                Phrase employmentProvinceStatePhraseLabel = new Phrase("Province / State \n\n", fontArial10Bold);
                Phrase employmentProvinceStatePhraseData = new Phrase(EmployerProvince, fontArial13);
                Paragraph employmentProvinceStateParagraph = new Paragraph
                {
                    employmentProvinceStatePhraseLabel, employmentProvinceStatePhraseData
                };

                Phrase employmentCountryPhraseLabel = new Phrase("Country \n\n", fontArial10Bold);
                Phrase employmentCountryPhraseData = new Phrase(EmployerCountry, fontArial13);
                Paragraph employmentCountryParagraph = new Paragraph
                {
                    employmentCountryPhraseLabel, employmentCountryPhraseData
                };

                Phrase employmentPostalPhraseLabel = new Phrase("Postal \n\n", fontArial10Bold);
                Phrase employmentPostalPhraseData = new Phrase(EmployerZipCode, fontArial13);
                Paragraph employmentPostalParagraph = new Paragraph
                {
                    employmentPostalPhraseLabel, employmentPostalPhraseData
                };

                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(employmentMunicipalityCityParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(employmentProvinceStateParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(employmentCountryParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerEmploymentDetails.AddCell(new PdfPCell(employmentPostalParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(pdfTableCustomerEmploymentDetails);

                Phrase employementEmailAddressPhraseLabel = new Phrase("Email Address \n\n", fontArial10Bold);
                Phrase employementEmailAddressPhraseData = new Phrase(" ", fontArial13);
                Paragraph employementEmailAddressParagraph = new Paragraph
                {
                    employementEmailAddressPhraseLabel, employementEmailAddressPhraseData
                };

                Phrase employementLandLineNoPhraseLabel = new Phrase("Landline No. \n\n", fontArial10Bold);
                Phrase employementLandLineNoPhraseData = new Phrase(EmployerTelephoneNumber, fontArial13);
                Paragraph employementLandLineNoParagraph = new Paragraph
                {
                    employementLandLineNoPhraseLabel, employementLandLineNoPhraseData
                };

                Phrase employementMobileNoPhraseLabel = new Phrase("Mobile No. \n\n", fontArial10Bold);
                Phrase employementMobileNoPhraseData = new Phrase(EmployerMobileNumber, fontArial13);
                Paragraph employementMobileNoParagraph = new Paragraph
                {
                    employementMobileNoPhraseLabel, employementMobileNoPhraseData
                };

                // ================================================================
                // Email, Telephone and Mobile Numbers (EMPLOYER'S CONTACT DETAILS)
                // ================================================================
                PdfPTable pdfTableEmploymentContactEmailAndNumbers = new PdfPTable(3);
                pdfTableEmploymentContactEmailAndNumbers.SetWidths(new float[] { 100f, 100f, 100f });
                pdfTableEmploymentContactEmailAndNumbers.WidthPercentage = 100;

                pdfTableEmploymentContactEmailAndNumbers.AddCell(new PdfPCell(employementEmailAddressParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableEmploymentContactEmailAndNumbers.AddCell(new PdfPCell(employementLandLineNoParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableEmploymentContactEmailAndNumbers.AddCell(new PdfPCell(employementMobileNoParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(pdfTableEmploymentContactEmailAndNumbers);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ===============
            // Response Stream
            // ===============
            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        // ===========
        // PDF Brooker
        // ===========
        [HttpGet, Route("Broker/{id}")]
        public HttpResponseMessage PdfBroker(string id)
        {
            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            // =============
            // Open Document
            // =============
            document.Open();

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f, Border = 0 });

            // ===============
            // Settings (Data)
            // ===============
            var sysSettings = from d in db.SysSettings
                              select d;

            if (sysSettings.Any())
            {
                // ===============
                // Company Details
                // ===============
                PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
                pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
                pdfTableCompanyDetail.WidthPercentage = 100;
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase(sysSettings.FirstOrDefault().Company, fontArial17Bold)) { Border = 0 });
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("Broker Accreditation Form", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
                document.Add(pdfTableCompanyDetail);
                document.Add(line);

                document.Add(spaceTable);
            }

            // ===========
            // Get Brooker
            // ===========
            var broker = from d in db.MstBrokers
                         where d.Id == Convert.ToInt32(id)
                         && d.IsLocked == true
                         select d;

            if (broker.Any())
            {
                // ============
                // Brooker Data
                // ============
                String BrokerCode = broker.FirstOrDefault().BrokerCode;
                String LastName = broker.FirstOrDefault().LastName;
                String FirstName = broker.FirstOrDefault().FirstName;
                String MiddleName = broker.FirstOrDefault().MiddleName;
                String LicenseNumber = broker.FirstOrDefault().LicenseNumber;
                String BirthDate = broker.FirstOrDefault().BirthDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
                String CivilStatus = broker.FirstOrDefault().CivilStatus;
                String Gender = broker.FirstOrDefault().Gender;
                String Address = broker.FirstOrDefault().Address;
                String TelephoneNumber = broker.FirstOrDefault().TelephoneNumber;
                String MobileNumber = broker.FirstOrDefault().MobileNumber;
                String Religion = broker.FirstOrDefault().Religion;
                String EmailAddress = broker.FirstOrDefault().EmailAddress;
                String Facebook = broker.FirstOrDefault().Facebook;
                String TIN = broker.FirstOrDefault().TIN;
                String RealtyFirm = broker.FirstOrDefault().RealtyFirm;
                String RealtyFirmAddress = broker.FirstOrDefault().RealtyFirmAddress;
                String RealtyFirmTelephoneNumber = broker.FirstOrDefault().RealtyFirmTelephoneNumber;
                String RealtyFirmMobileNumber = broker.FirstOrDefault().RealtyFirmMobileNumber;
                String RealtyFirmFaxNumber = broker.FirstOrDefault().RealtyFirmFaxNumber;
                String RealtyFirmEmailAddress = broker.FirstOrDefault().RealtyFirmEmailAddress;
                String RealtyFirmWebsite = broker.FirstOrDefault().RealtyFirmWebsite;
                String RealtyFirmTIN = broker.FirstOrDefault().RealtyFirmTIN;
                String Organization = broker.FirstOrDefault().Organization;
                String Remarks = broker.FirstOrDefault().Remarks;
                String Status = broker.FirstOrDefault().Status;

                // =============
                // Table Brooker
                // =============
                PdfPTable pdfTableBroker = new PdfPTable(5);
                pdfTableBroker.SetWidths(new float[] { 100f, 50f, 50f, 50f, 50f });
                pdfTableBroker.WidthPercentage = 100;

                pdfTableBroker.AddCell(new PdfPCell(new Phrase("")) { Colspan = 5, Border = 1, BorderWidthTop = 5f });

                Phrase lastNamePhraseLabel = new Phrase("Last Name \n\n", fontArial10Bold);
                Phrase lastNamePhraseData = new Phrase(LastName, fontArial13);
                Paragraph lastNameParagraph = new Paragraph
                {
                    lastNamePhraseLabel, lastNamePhraseData
                };

                Phrase firstNamePhraseLabel = new Phrase("First Name \n\n", fontArial10Bold);
                Phrase firstNamePhraseData = new Phrase(FirstName, fontArial13);
                Paragraph firstNameParagraph = new Paragraph
                {
                    firstNamePhraseLabel, firstNamePhraseData
                };

                Phrase middleNamePhraseLabel = new Phrase("Middle Name \n\n", fontArial10Bold);
                Phrase middleNamePhraseData = new Phrase(MiddleName, fontArial13);
                Paragraph middleNameParagraph = new Paragraph
                {
                    middleNamePhraseLabel, middleNamePhraseData
                };

                pdfTableBroker.AddCell(new PdfPCell(lastNameParagraph) { PaddingTop = 6f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(firstNameParagraph) { Colspan = 2, PaddingTop = 6f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(middleNameParagraph) { Colspan = 2, PaddingTop = 6f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase lincenseNoPhraseLabel = new Phrase("License No. \n\n", fontArial10Bold);
                Phrase lincenseNoPhraseData = new Phrase(LicenseNumber, fontArial13);
                Paragraph lincenseNoParagraph = new Paragraph
                {
                    lincenseNoPhraseLabel, lincenseNoPhraseData
                };

                Int32 currentYear = DateTime.Today.Year;
                Int32 birthYear = broker.FirstOrDefault().BirthDate.Year;

                Int32 getAge = currentYear - birthYear;

                Phrase agePhraseLabel = new Phrase("Age \n\n", fontArial10Bold);
                Phrase agePhraseData = new Phrase(getAge.ToString(), fontArial13);
                Paragraph ageParagraph = new Paragraph
                {
                    agePhraseLabel, agePhraseData
                };

                Phrase sexPhraseLabel = new Phrase("Sex \n\n", fontArial10Bold);
                Phrase sexPhraseData = new Phrase(Gender, fontArial13);
                Paragraph sexParagraph = new Paragraph
                {
                    sexPhraseLabel, sexPhraseData
                };

                Phrase civilStatusPhraseLabel = new Phrase("Civil Status \n\n", fontArial10Bold);
                Phrase civilStatusPhraseData = new Phrase(CivilStatus, fontArial13);
                Paragraph civilStatusParagraph = new Paragraph
                {
                    civilStatusPhraseLabel, civilStatusPhraseData
                };

                Phrase birthDatePhraseLabel = new Phrase("Birth date \n\n", fontArial10Bold);
                Phrase birthDatePhraseData = new Phrase(BirthDate, fontArial13);
                Paragraph birthDateParagraph = new Paragraph
                {
                    birthDatePhraseLabel, birthDatePhraseData
                };

                pdfTableBroker.AddCell(new PdfPCell(lincenseNoParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(ageParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(sexParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(civilStatusParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(birthDateParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase residencePostalAddressPhraseLabel = new Phrase("Residence & Postal Address \n\n", fontArial10Bold);
                Phrase residencePostalAddressPhraseData = new Phrase(Address, fontArial13);
                Paragraph residencePostalAddressParagraph = new Paragraph
                {
                    residencePostalAddressPhraseLabel, residencePostalAddressPhraseData
                };

                pdfTableBroker.AddCell(new PdfPCell(residencePostalAddressParagraph) { Colspan = 5, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase homePhoneNoPhraseLabel = new Phrase("Home Phone No. \n\n", fontArial10Bold);
                Phrase homePhoneNoPhraseData = new Phrase(TelephoneNumber, fontArial13);
                Paragraph homePhoneNoParagraph = new Paragraph
                {
                    homePhoneNoPhraseLabel, homePhoneNoPhraseData
                };

                Phrase mobilePhoneNoPhraseLabel = new Phrase("Mobile Phone No. \n\n", fontArial10Bold);
                Phrase mobilePhoneNoPhraseData = new Phrase(MobileNumber, fontArial13);
                Paragraph mobilePhoneNoParagraph = new Paragraph
                {
                    mobilePhoneNoPhraseLabel, mobilePhoneNoPhraseData
                };

                Phrase religionPhraseLabel = new Phrase("Religion \n\n", fontArial10Bold);
                Phrase religionPhraseData = new Phrase(Religion, fontArial13);
                Paragraph religionParagraph = new Paragraph
                {
                    religionPhraseLabel, religionPhraseData
                };

                pdfTableBroker.AddCell(new PdfPCell(homePhoneNoParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(mobilePhoneNoParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(religionParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase emailAddressPhraseLabel = new Phrase("Email Address \n\n", fontArial10Bold);
                Phrase emailAddressPhraseData = new Phrase(EmailAddress, fontArial13);
                Paragraph emailAddressParagraph = new Paragraph
                {
                    emailAddressPhraseLabel, emailAddressPhraseData
                };

                Phrase facebookPhraseLabel = new Phrase("Facebook \n\n", fontArial10Bold);
                Phrase facebookPhraseData = new Phrase(Facebook, fontArial13);
                Paragraph facebookParagraph = new Paragraph
                {
                    facebookPhraseLabel, facebookPhraseData
                };

                Phrase TINNoPhraseLabel = new Phrase("TIN No. \n\n", fontArial10Bold);
                Phrase TINNoPhraseData = new Phrase(TIN, fontArial13);
                Paragraph TINNoParagraph = new Paragraph
                {
                    TINNoPhraseLabel, TINNoPhraseData
                };

                pdfTableBroker.AddCell(new PdfPCell(emailAddressParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(facebookParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(TINNoParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                pdfTableBroker.AddCell(new PdfPCell(new Phrase("")) { Colspan = 5, Border = 1, BorderWidthTop = 5f });

                Phrase realtyFirmPhraseLabel = new Phrase("Name of Realty Firm \n\n", fontArial10Bold);
                Phrase realtyFirmPhraseData = new Phrase(RealtyFirm, fontArial13);
                Paragraph realtyFirmParagraph = new Paragraph
                {
                    realtyFirmPhraseLabel, realtyFirmPhraseData
                };

                Phrase officeAddressPhraseLabel = new Phrase("Office Address \n\n", fontArial10Bold);
                Phrase officeAddressPhraseData = new Phrase(RealtyFirmAddress, fontArial13);
                Paragraph officeAddressParagraph = new Paragraph
                {
                    officeAddressPhraseLabel, officeAddressPhraseData
                };

                pdfTableBroker.AddCell(new PdfPCell(realtyFirmParagraph) { Colspan = 5, PaddingTop = 6f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(officeAddressParagraph) { Colspan = 5, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase officePhoneNumberPhraseLabel = new Phrase("Office Phone No. \n\n", fontArial10Bold);
                Phrase officePhoneNumberPhraseData = new Phrase(RealtyFirmTelephoneNumber, fontArial13);
                Paragraph officePhoneNumberParagraph = new Paragraph
                {
                    officePhoneNumberPhraseLabel, officePhoneNumberPhraseData
                };

                Phrase officeMobileNumberPhraseLabel = new Phrase("Mobile Phone No. \n\n", fontArial10Bold);
                Phrase officeMobileNumberPhraseData = new Phrase(RealtyFirmMobileNumber, fontArial13);
                Paragraph officeMobileNumberParagraph = new Paragraph
                {
                    officeMobileNumberPhraseLabel, officeMobileNumberPhraseData
                };

                Phrase faxNoPhraseLabel = new Phrase("Fax No. \n\n", fontArial10Bold);
                Phrase faxNoPhraseData = new Phrase(RealtyFirmFaxNumber, fontArial13);
                Paragraph faxNoParagraph = new Paragraph
                {
                    faxNoPhraseLabel, faxNoPhraseData
                };

                pdfTableBroker.AddCell(new PdfPCell(officePhoneNumberParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(officeMobileNumberParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(faxNoParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase officeEmailAddressPhraseLabel = new Phrase("Office Email Address \n\n", fontArial10Bold);
                Phrase officeEmailAddressPhraseData = new Phrase(RealtyFirmEmailAddress, fontArial13);
                Paragraph officeEmailAddressParagraph = new Paragraph
                {
                    officeEmailAddressPhraseLabel, officeEmailAddressPhraseData
                };

                Phrase websitePhraseLabel = new Phrase("Website \n\n", fontArial10Bold);
                Phrase websitePhraseData = new Phrase(RealtyFirmWebsite, fontArial13);
                Paragraph websiteParagraph = new Paragraph
                {
                    websitePhraseLabel, websitePhraseData
                };

                Phrase officeTINNoPhraseLabel = new Phrase("TIN No. \n\n", fontArial10Bold);
                Phrase officeTINNoPhraseData = new Phrase(RealtyFirmTIN, fontArial13);
                Paragraph officeTINNoParagraph = new Paragraph
                {
                    officeTINNoPhraseLabel, officeTINNoPhraseData
                };

                pdfTableBroker.AddCell(new PdfPCell(officeEmailAddressParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(websiteParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableBroker.AddCell(new PdfPCell(officeTINNoParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase realEstateOrganizationPhraseLabel = new Phrase("Real State Organization \n\n", fontArial10Bold);
                Phrase realEstateOrganizationPhraseData = new Phrase(Organization, fontArial13);
                Paragraph realEstateOrganizationParagraph = new Paragraph
                {
                    realEstateOrganizationPhraseLabel, realEstateOrganizationPhraseData
                };

                pdfTableBroker.AddCell(new PdfPCell(realEstateOrganizationParagraph) { Colspan = 5, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(pdfTableBroker);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ===============
            // Response Stream
            // ===============
            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        // =============
        // PDF Checklist
        // =============
        [HttpGet, Route("Checklist/{id}")]
        public HttpResponseMessage PdfChecklist(string id)
        {
            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            // =============
            // Open Document
            // =============
            document.Open();

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f, Border = 0 });

            // ===============
            // Settings (Data)
            // ===============
            var sysSettings = from d in db.SysSettings
                              select d;

            if (sysSettings.Any())
            {
                // ===============
                // Company Details
                // ===============
                PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
                pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
                pdfTableCompanyDetail.WidthPercentage = 100;
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase(sysSettings.FirstOrDefault().Company, fontArial17Bold)) { Border = 0 });
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("Checklist of Requirements", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
                document.Add(pdfTableCompanyDetail);
                document.Add(line);

                document.Add(spaceTable);
            }

            // =============
            // Get Checklist
            // =============
            var checklist = from d in db.MstCheckLists
                            where d.Id == Convert.ToInt32(id)
                            select new
                            {
                                Id = d.Id,
                                ChecklistCode = d.CheckListCode,
                                Checklist = d.CheckList,
                                ChecklistDate = d.CheckListDate.ToShortDateString(),
                                ProjectId = d.ProjectId,
                                Project = d.MstProject.Project,
                                Remarks = d.Remarks,
                                Status = d.Status,
                                IsLocked = d.IsLocked,
                                CreatedBy = d.CreatedBy,
                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                UpdatedBy = d.UpdatedBy,
                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                            };

            if (checklist.Any())
            {
                // ==============
                // Checklist Data
                // ==============
                String ChecklistCode = checklist.FirstOrDefault().ChecklistCode;
                String Checklist = checklist.FirstOrDefault().Checklist;
                String ChecklistDate = checklist.FirstOrDefault().ChecklistDate;
                String Project = checklist.FirstOrDefault().Project;
                String Remarks = checklist.FirstOrDefault().Remarks;
                String Status = checklist.FirstOrDefault().Status;

                // ======================
                // Table Header Checklist
                // ======================
                PdfPTable tableChecklist = new PdfPTable(2);
                tableChecklist.SetWidths(new float[] { 10f, 90f });
                tableChecklist.WidthPercentage = 100;
                tableChecklist.AddCell(new PdfPCell(new Phrase("Code ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableChecklist.AddCell(new PdfPCell(new Phrase(ChecklistCode, fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableChecklist.AddCell(new PdfPCell(new Phrase("Checklist", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableChecklist.AddCell(new PdfPCell(new Phrase(Checklist, fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableChecklist.AddCell(new PdfPCell(new Phrase("Project", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableChecklist.AddCell(new PdfPCell(new Phrase(Project, fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableChecklist.AddCell(new PdfPCell(new Phrase("Remarks", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableChecklist.AddCell(new PdfPCell(new Phrase(Remarks, fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableChecklist.AddCell(new PdfPCell(new Phrase("Status", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                tableChecklist.AddCell(new PdfPCell(new Phrase(Status, fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(tableChecklist);

                PdfPTable checklistRequirementsItems = new PdfPTable(4);
                checklistRequirementsItems.SetWidths(new float[] { 30f, 150f, 100f, 50f });
                checklistRequirementsItems.WidthPercentage = 100;
                checklistRequirementsItems.AddCell(new PdfPCell(new Phrase("No", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                checklistRequirementsItems.AddCell(new PdfPCell(new Phrase("Requirement", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                checklistRequirementsItems.AddCell(new PdfPCell(new Phrase("Type", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 7f });
                checklistRequirementsItems.AddCell(new PdfPCell(new Phrase("With Attachments", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                checklistRequirementsItems.AddCell(new PdfPCell(new Phrase(" ", fontArial5)) { Colspan = 4, Border = 0 });

                // ==========================
                // Get Checklist Requirements
                // ==========================
                var checklistRequirementData = from d in db.MstCheckListRequirements
                                               where d.CheckListId == checklist.FirstOrDefault().Id
                                               select new
                                               {
                                                   d.CheckListId,
                                                   d.MstCheckList.CheckList,
                                                   d.RequirementNo,
                                                   d.Requirement,
                                                   d.Category,
                                                   d.Type,
                                                   d.WithAttachments
                                               };

                if (checklistRequirementData.Any())
                {
                    // ========================
                    // Get Checklist Categories
                    // ========================
                    var checklistRequirementsByCategories = from d in checklistRequirementData
                                                            group d by new
                                                            {
                                                                d.Category
                                                            } into g
                                                            select new
                                                            {
                                                                g.Key.Category
                                                            };

                    if (checklistRequirementsByCategories.Any())
                    {
                        foreach (var checklistRequirementsByCategory in checklistRequirementsByCategories)
                        {
                            checklistRequirementsItems.AddCell(new PdfPCell(new Phrase(checklistRequirementsByCategory.Category, fontArial10Bold)) { Border = 0, Colspan = 4, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 7f });

                            // =================================
                            // Get Checklist Requirements (Data)
                            // =================================
                            var checklistRequirements = from d in checklistRequirementData.OrderBy(d => d.RequirementNo)
                                                        where d.Category.Equals(checklistRequirementsByCategory.Category)
                                                        select new
                                                        {
                                                            d.RequirementNo,
                                                            d.Requirement,
                                                            d.Type,
                                                            d.WithAttachments
                                                        };

                            if (checklistRequirements.Any())
                            {
                                foreach (var checklistRequirement in checklistRequirements)
                                {
                                    String withAttachmentString = "No";
                                    if (checklistRequirement.WithAttachments)
                                    {
                                        withAttachmentString = "Yes";
                                    }

                                    checklistRequirementsItems.AddCell(new PdfPCell(new Phrase(checklistRequirement.RequirementNo.ToString(), fontArial10)) { Border = 0, HorizontalAlignment = 2, PaddingTop = .2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    checklistRequirementsItems.AddCell(new PdfPCell(new Phrase(checklistRequirement.Requirement, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = .2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    checklistRequirementsItems.AddCell(new PdfPCell(new Phrase(checklistRequirement.Type, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = .2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    checklistRequirementsItems.AddCell(new PdfPCell(new Phrase(withAttachmentString, fontArial10)) { Border = 0, HorizontalAlignment = 0, PaddingTop = .2f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                }
                            }
                        }
                    }
                }

                document.Add(spaceTable);
                document.Add(checklistRequirementsItems);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ===============
            // Response Stream
            // ===============
            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        // ======================
        // PDF Sold Unit Proposal
        // ======================
        [HttpGet, Route("SoldUnitProposal/{id}")]
        public HttpResponseMessage PdfSoldUnitProposal(int id)
        {
            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            // =============
            // Open Document
            // =============
            document.Open();

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 100f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 100;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { PaddingTop = 5f, Border = 0 });

            // ===============
            // Settings (Data)
            // ===============
            var sysSettings = from d in db.SysSettings
                              select d;

            if (sysSettings.Any())
            {
                // ===============
                // Company Details
                // ===============
                PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
                pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
                pdfTableCompanyDetail.WidthPercentage = 100;
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase(sysSettings.FirstOrDefault().Company, fontArial17Bold)) { Border = 0 });
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("Investment Proposal Summary", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
                document.Add(pdfTableCompanyDetail);
                document.Add(line);

                document.Add(spaceTable);
            }

            // =============
            // Get Sold Unit
            // =============
            var soldUnit = from d in db.TrnSoldUnits
                           where d.Id == Convert.ToInt32(id)
                           select d;

            if (soldUnit.Any())
            {
                // ==============
                // Sold Unit Data
                // ==============
                String Customer = soldUnit.FirstOrDefault().MstCustomer.FirstName + " " + soldUnit.FirstOrDefault().MstCustomer.MiddleName + " " + soldUnit.FirstOrDefault().MstCustomer.LastName;
                String Unit = soldUnit.FirstOrDefault().MstUnit.Block + " " + soldUnit.FirstOrDefault().MstUnit.Lot;
                String Project = soldUnit.FirstOrDefault().MstProject.Project;
                String TotalInvestment = soldUnit.FirstOrDefault().TotalInvestment;
                String PaymentOptions = soldUnit.FirstOrDefault().PaymentOptions;
                String Financing = soldUnit.FirstOrDefault().Financing;
                String PreparedByName = soldUnit.FirstOrDefault().MstUser2.FullName;
                String CheckByName = soldUnit.FirstOrDefault().MstUser3.FullName;
                Decimal Price = soldUnit.FirstOrDefault().Price;
                Decimal TLA = soldUnit.FirstOrDefault().MstUnit.TLA;
                Decimal TFA = soldUnit.FirstOrDefault().MstUnit.TFA;

                // ================
                // Sold Unit Header
                // ================
                PdfPTable pdfTableSoldUnitHeaderProposal = new PdfPTable(2);
                pdfTableSoldUnitHeaderProposal.SetWidths(new float[] { 20f, 80f });
                pdfTableSoldUnitHeaderProposal.WidthPercentage = 100;
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Investors Name ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase(Customer, fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase(Unit, fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Lot Area", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase(TLA.ToString("#,##0.00"), fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Floor Area", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase(TFA.ToString("#,##0.00"), fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Project", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase(Project, fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Reservation Fee", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Php ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(pdfTableSoldUnitHeaderProposal);

                document.Add(line);

                // ================
                // Total Investment
                // ================
                Paragraph totalInvestmentParagraph = new Paragraph
                {
                    TotalInvestment
                };

                document.Add(totalInvestmentParagraph);
                document.Add(line);

                // ========================
                // Flexible Payment Options
                // ========================
                Paragraph flexiblePaymentOptionsParagraph = new Paragraph
                {
                    PaymentOptions
                };

                document.Add(flexiblePaymentOptionsParagraph);
                document.Add(line);

                // =====================
                // Easy Financing Scheme
                // =====================
                Paragraph easyFinancingSchemeParagraph = new Paragraph
                {
                    Financing
                };

                document.Add(easyFinancingSchemeParagraph);
                document.Add(line);

                // ================
                // Note Footer List
                // ================
                List list = new List(List.ORDERED, 20f);
                list.SetListSymbol("\u2022");

                list.IndentationLeft = 20f;
                list.IndentationRight = 20f;

                if (sysSettings.Any())
                {
                    list.Add("The developer reserves the right to verify and correct above figures if necessary.");
                    list.Add("Installment payments must be covered with POST-DATED checks.");
                    list.Add("For all installment terms, buyers are required to submit an approve Bank Pre-Qualification in favor of " + sysSettings.FirstOrDefault().Company + " during the start of the amortization. House will be not constructed without the Pre-Qualification.");
                    list.Add("Please make check payable to " + sysSettings.FirstOrDefault().Company + ".");
                    list.Add("Registration Fees, Doc Stamp Tax, Transfer Tax and EVAT are inclusive on the Total Contract Price and subject to change base on the law mandated rates upon registration of the documents covering the purchase.");
                    list.Add("Bedroom cabinets and landscaping (except bermuda) are not included.");
                    list.Add("Prices are subject to change without prior notice unless PDCs and Contract to");
                    list.Add("House construction will commence after the payment of required equity & submission of the approved Bank Pre-Qualification in favor of " + sysSettings.FirstOrDefault().Company + ".");
                }

                // =====
                // Lists
                // =====
                var notePhrase = new Phrase("NOTE:", fontArial11Bold);
                Paragraph noteParagraph = new Paragraph
                {
                    notePhrase
                };

                document.Add(noteParagraph);
                document.Add(list);

                // ======
                // Spaces
                // ======
                document.Add(spaceTable);
                document.Add(spaceTable);
                document.Add(spaceTable);

                // ===============
                // User Signatures
                // ===============
                PdfPTable pdfTableUserSignatures = new PdfPTable(5);
                pdfTableUserSignatures.SetWidths(new float[] { 80f, 150f, 80f, 80f, 150f });
                pdfTableUserSignatures.WidthPercentage = 100;

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Prepared by: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(PreparedByName, fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Broker / Agent: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Broker & Coordinator", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ")) { Colspan = 5, Border = 0, PaddingTop = 20f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Verified by: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(CheckByName, fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Conforme: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Sales Manager", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("SIGNATURE OVER PRINTED NAME", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Investor", fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });

                document.Add(pdfTableUserSignatures);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ===============
            // Response Stream
            // ===============
            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }

        [HttpGet, Route("SoldUnitContract/{id}")]
        public HttpResponseMessage PdfSoldUnitContract(int id)
        {
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font fontArial14Bold = FontFactory.GetFont("Arial", 14, Font.BOLD);
            Font linebreak = FontFactory.GetFont("Arial", 2);

            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

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
                headerPage.AddCell(new PdfPCell(new Phrase("SoldUnitContract Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
                document.Add(headerPage);
                document.Add(line);
            }
            //query for contract
            var contract = from d in db.TrnSoldUnits
                           where d.ProjectId == Convert.ToInt32(id)
                           select new
                           {
                               Id = d.Id,
                               SoldUnitNumber = d.SoldUnitNumber,
                               SoldUnitDate = d.SoldUnitDate.ToShortDateString(),
                               ProjectId = d.ProjectId,
                               Project = d.MstProject.Project,
                               UnitId = d.UnitId,
                               TLA = d.MstUnit.TLA,
                               TFA = d.MstUnit.TFA,
                               HouseModel = d.MstUnit.MstHouseModel.HouseModel,
                               Address = d.MstCustomer.Address,
                               TIN = d.MstCustomer.TIN,
                               Prices = d.MstUnit.MstHouseModel.Price,
                               Lot = d.MstUnit.Lot,
                               Block = d.MstUnit.Block,
                               Unit = "BLOCK                             " + d.MstUnit.Block + "\n\n \n\nLOT    " + "                             " + d.MstUnit.Lot,
                               CustomerId = d.CustomerId,
                               Customer = d.MstCustomer.LastName + ", " + d.MstCustomer.FirstName + " " + d.MstCustomer.MiddleName,
                               BrokerId = d.BrokerId,
                               Broker = d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName,
                               ChecklistId = d.CheckListId,
                               Checklist = d.MstCheckList.CheckList,
                               Price = d.Price,
                               TotalInvestment = d.TotalInvestment,
                               PaymentOptions = d.PaymentOptions,
                               Financing = d.Financing,
                               Remarks = d.Remarks,
                               PreparedBy = d.PreparedBy,
                               PreparedByUser = d.MstUser2.Username,
                               CheckedBy = d.CheckedBy,
                               CheckedByUser = d.MstUser3.Username,
                               ApprovedBy = d.ApprovedBy,
                               ApprovedByUser = d.MstUser4.Username,
                               Status = d.Status,
                               IsLocked = d.IsLocked,
                               CreatedBy = d.CreatedBy,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedBy = d.UpdatedBy,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            //instances...Uses at the bottom
            Paragraph p = new Paragraph();
            Paragraph p2 = new Paragraph();
            Paragraph p3 = new Paragraph();
            Paragraph p4 = new Paragraph();
            Phrase p1 = new Phrase();

            //instances for last page DECLARE HERE FOR NEATNESS
            Paragraph p6 = new Paragraph();
            Paragraph p7 = new Paragraph();
            Paragraph p8 = new Paragraph();
            Paragraph p9 = new Paragraph();
            Paragraph p10 = new Paragraph();
            Paragraph p11 = new Paragraph();
            Paragraph p12 = new Paragraph();
            Paragraph p13 = new Paragraph();
            Paragraph p14 = new Paragraph();

            if (contract.Any())
            {
                // make a new line 
                p.Add(Chunk.NEWLINE);
                p.Add(Chunk.NEWLINE);

                String Project = contract.FirstOrDefault().Project;
                String Date = contract.FirstOrDefault().SoldUnitDate;
                String Customer = contract.FirstOrDefault().Customer;
                String Address = contract.FirstOrDefault().Address;
                String Lot = contract.FirstOrDefault().Lot;
                String Block = contract.FirstOrDefault().Block;
                String TLA = contract.FirstOrDefault().TLA.ToString();
                String TFA = contract.FirstOrDefault().TFA.ToString();
                String BlockLot = contract.FirstOrDefault().Unit.ToString();
                String Price = contract.FirstOrDefault().Price.ToString();
                String Prices = contract.FirstOrDefault().Prices.ToString();
                String HouseModel = contract.FirstOrDefault().HouseModel.ToString();
                String TotalInvestment = contract.FirstOrDefault().TotalInvestment.ToString();
                PdfPTable tableContract = new PdfPTable(2);
                float[] widthscellsTablePurchaseOrder = new float[] { 40f, 150f };
                tableContract.SetWidths(widthscellsTablePurchaseOrder);
                tableContract.WidthPercentage = 100;
                Paragraph p5 = new Paragraph();
                p.Alignment = Element.ALIGN_CENTER;

                p.Add(new Chunk("CONTRACT TO SELL", fontArial14Bold));
                p2.Add(Chunk.NEWLINE);
                p2.Add(new Chunk("KNOW ALL MEN BY THESE PRESENTS", fontArial11));
                p2.Add(Chunk.NEWLINE);
                p2.Add(Chunk.NEWLINE);
                p2.Add(new Chunk("This Contact to Sell (hereinafter referred to as the “Contract”) made and entered into this day of ", fontArial10));
                p2.Add(new Chunk(Date, fontArial12Bold));
                p2.Add(new Chunk(" at Cebu City, Cebu, Philippines, by and between:", fontArial10));
                p2.Add(Chunk.NEWLINE);
                p2.Add(Chunk.NEWLINE);
                p2.Add(new Chunk("PRILAND DEVELOPMENT CORPORATION", fontArial11Bold));
                p2.Add(new Chunk(", a corporation duly organized and existing under and by virtue of the laws of the Philippines,with principal office address at S. Jayme St., Zone Pechay, Pakna-an, Mandaue City, Cebu Philippines represented by its President Ramon Carlo T. Yap, (hereinafter referred to as the “SELLER/OWNER/DEVELOPER”);", fontArial10));
                p2.Add(Chunk.NEWLINE);
                p2.Add(Chunk.NEWLINE);
                p2.Add(new Chunk(Customer, fontArial11Bold));

                p2.Add(new Chunk(", of legal age, Filipino citizen married to", fontArial10));
                p2.Add(new Chunk(" SPOUSE ", fontArial11Bold));
                p2.Add(new Chunk("of legal age, American citizen, both residents of ", fontArial10));
                p2.Add(new Chunk(Address, fontArial11Bold));
                p2.Add(new Chunk(" Cebu, Philippines 6000(hereinafter referred to as the “BUYER”).", fontArial10));


                p3.Alignment = Element.ALIGN_CENTER;
                p3.Add(Chunk.NEWLINE);
                p3.Add(Chunk.NEWLINE);
                p3.Add(Chunk.NEWLINE);
                p3.Add(new Chunk("WITNESSETH:", fontArial14Bold));
                p4.Add(Chunk.NEWLINE);

                p4.Add(new Chunk("That for and in consideration of the sums of money to  be paid in the manner herein below specified, and the undertaking of the BUYER/S" +
                                " to fully perform and comply with all his/her/their obligations,covenants,conditions,and restrictions as herein specified and as enumerated " +
                                "in the DECLARATION OF COVENANTS,CONDITIONS AND RESTRICTIONS (attached hereto as Annex “A” and hereby made an integral part thereof), the SELLER " +
                                "hereby agrees and contracts to sell to the BUYER, and the latter hereby agree/s and contract/s to buy form the former, one(1) dwelling unit, " +
                                "situated in ____________________________________, which unit is specifically identified as (as hereinafter referred to as UNIT):", fontArial10));

                p4.Add(Chunk.NEWLINE);
                p4.Add(Chunk.NEWLINE);

                //write to pdf
                document.Add(p);
                document.Add(p2);
                document.Add(p3);
                document.Add(p4);

                //table set-up
                PdfPTable spaceTable = new PdfPTable(1);
                float[] widthCellsSpaceTable = new float[] { 100f };
                spaceTable.SetWidths(widthCellsSpaceTable);
                spaceTable.WidthPercentage = 100;
                spaceTable.AddCell(new PdfPCell(new Phrase(" ", fontArial10Bold)) { Border = 0, PaddingTop = 10f });

                tableContract.AddCell(new PdfPCell(new Phrase("Project ", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase(Project, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase("Block ", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase(Block, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase("Lot", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase(Lot, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase("Total Land Area ", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase(TLA, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase("Total Floor Area ", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase(TFA, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase("House Model ", fontArial11Bold)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });
                tableContract.AddCell(new PdfPCell(new Phrase(HouseModel, fontArial11)) { Border = 0, PaddingTop = 10f, PaddingLeft = 5f, PaddingRight = 5f });

                document.Add(tableContract);

                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The said DECLARATION OF COVENANTS,CONDITIONS AND RESTRICTIONS shall be annotated as liens and easements on the  corresponding certificate of" +
                    " title to be issued to the BUYER/S upon compliance with all his/her/their obligations as specified hereunder:", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("1.Contract Price and Manner of Payment", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The total CONTRACT PRICE for the above-described HOUSE AND LOT shall be ", fontArial11));
                p5.Add(new Chunk(Prices, fontArial11Bold));
                p5.Add(new Chunk(", included Transfer Charges and Miscellaneous Fees.)", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The net Total Contract price of ", fontArial11));
                p5.Add(new Chunk(Price, fontArial11Bold));
                p5.Add(new Chunk(", net of reservation fee of Twenty Thousand Pesos(Php 20,000.00), shall be payable in ", fontArial11));
                p5.Add(new Chunk("Thirty Six(36) ", fontArial11Bold));
                p5.Add(new Chunk(" equal monthly installment in the amount of ", fontArial11));
                p5.Add(new Chunk(" Sixty Nine Thousand Four Hundred Eighty Four Pesos and 72/100 Only (69,484.72).", fontArial11Bold));

                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Postdated checks shall be issued by the BUYER hereof to cover all the abovementioned installments. Any unpaid balance of the CONTRACT" +
                                 " PRICE shall earn an interest of 3% per month.)", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("No delay or omission in exercising any right granted to the SELLER under this Contract shall be construed as a waiver thereof and the receipt" +
                                  " by the SELLER of any payment made in a manner other than as herein provided shall not be construed as a modification of the terms hereof. " +
                                  "In the event the SELLER accepts the BUYER’s payment after due date, such payment shall include an additional sum to cover penaltieson delayed installment " +
                                  "at the rate of 3% per month of delay on the amount due. The imposition of the penalty shall be without prejudice to the availment  by the seller of any remedy " +
                                  "provided hereunder and by law. Moreover, acceptance of said payment should not be construed as condonation  of any subsequent failure, delay or default by " +
                                  "the BUYER.)", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("2.Financion Option", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Notwithstanding the amortization schedule agreed upon, the BUYER may opt to pay the remaining balance of the CONTRACT PRICE through a loan which she " +
                                "may obtain from any public or private financing institution, the feeas and charges of which shall be for their own account.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Should the loan approved for the BUYER be less than the balance of the CONTRACT PRICE, the BUYER shall pay the SELLER the amount corresponding to the" +
                                " difference upon approval of the said loan. Upon the BUYER’S availment of the said loan to the SELLER, the above schedule of payment shall be considered of no " +
                                "further effect and/or amended as the case may be.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("3. Place of Payment", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("All payments other than those covered by the postdated checks due under this Contract shall be made by the BUYER to the SELLER’s cashiers at the SELLER’s" +
                                 " office at S.Jayme St., Zone Pechay, Pakna-an, Mandaue City, Cebu Philippines, without necessity of demand or notice. Failure by the BUYER to do so shall entitle" +
                                 " the SELLER to charge penalty at the rate of 3% per month.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("4. Solidary Liability", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("If there are two or more BUYERS under this Contract, they shall be deemed solidarily liable for all the obligations of herein set forth.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("5. Application of Payments", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The SELLER shall have the right to determine the application of payments made by the BUYER. Unless otherwise indicated in the SELLER’s " +
                                 "official receipt, payments shall be applied in the following order:", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(a) To costs and expenses incurred or advance by the SELLER pursuant to the Contract", fontArial11));
                p5.Add(new Chunk("(b) To penalties", fontArial11));
                p5.Add(new Chunk("(c) To interests", fontArial11));
                p5.Add(new Chunk("(d) To the principal", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("6. Restrictions", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The BUYER shall not make any construction, alteration or renovations/additions on the UNIT without first obtaining the prior consent of the SELLER. " +
                                  "The building of the house or the renovatons/additions to be made by the BUYER shall be subject to the approval of the SELLER. To ensure the proper" +
                                  " conduct of the works, the BUYER shall post a cash bond in an amount to be fixed by the SELLER depending on the nature of the works to be undertaken," +
                                  " before commencing such works. Said bond shall be returned to the  BUYER upon completion of the construction, after deducting costs of utilities, damage " +
                                  "to the common areas and other lots, and liability to third parties, if any.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The BUYER  further agrees to strictly comply all the terms, conditions and limitations contained in the Declaration of Covenants, Conditions and Restrictions" +
                                 " for the subdivision project, a copy of which is hereto attached as Annex “A” and made integral part hereof, as well as all the rules, regulations, and restrictions as may " +
                                "now or hereafter be required by the SELLER or the Association. The BUYER further confirms that his obligations under this Contract shall survive the full payment" +
                                " of the CONTRACT PRICE and the execution of the Deed of Absolute Sale.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("7.Homeowners’ Association", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("For purposes of promoting and protecting their mutual interest and assist in their community development, the proper operation, administration" +
                                " and maintenance of the community’s facilities and utilities,cleanliness and beautification of subdivision premises, collection of garbage, " +
                                "security, fire protection, enforcement of the deed of restrictions and restrictive easements, and in general, for promoting the common benefit of" +
                                " the residents therein, the OWNER/DEVELOPER/SELLER shall initiate the organization of the homeowners’ association (referred to as the “Association”), " +
                                "which shall be a non stock, and non profit organization.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The OWNER/SELLER/DEVELOPER and its representative/s are hereby authorized and empowered by the BUYER  to organize, incorporate and register the" +
                    " Association with the Housing Land Use Regulatory Board(HLURB), the Securities and Exchange Commission(SEC), the Local Government Unit concern and other " +
                    "government agencies, and/or entities of which the BUYER becomes an automatic member upon incorporation of the Association. The BUYER therefore agree/s and " +
                    "covenants to abide by its rules and regulations and to pay the dues and assessments duty levied and imposed by the Association.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Association dues shall be assessed upon the BUYER for such purpose/s and in such time and manner as set forth in the Articles of Incorporation" +
                                " and By-Law, and in the rules and regulations to be adopted by the Association.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The BUYER shall abide the rules and regulations issued by the SELLER or the Association in connection with the use and enjoyment of the facilities" +
                                 " existing in the subdivision/village.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Only unit owner/s in good standing are entitled to vote in any meeting where the vote is required or be voted upon in any election of the " +
                                "ASSOCIATION. The voting rights of unit owner/s who are not in good standing and of the amortizing buyers shall be executed by the" +
                                " SELLER/OWNER/DEVELOPER until such time as their respective obligation to the ASSOCIATION or to the SELLER are fully satisfied. A unit owner in" +
                                " good standing is one who has fully paid for his UNIT and is not delinquent in the payment of association dues and other assessments made by the " +
                                "ASSOCIATION.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("8.Taxes", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Real Property Tax", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Real property and other taxes that may be levied on the UNIT during the effectivity of this Contract, including the corresponding subcharges " +
                                "and penalties in case of delinquency, shall be borne and paid by the BUYER from and after the title to the UNIT is registered in the BUYER’s name," +
                                " or from the date possession of the UNIT is delivered to the BUYER, whichever comes first. The BUYER shall submit to the SELLER the official" +
                                " receipts evidencing payments of such liabilities within fifteen (15) days from the date such payments are made, which shall in no case be later" +
                                " than April 15 of each year.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Should the BUYER fail to pay such taxes, the SELLER may, at its option but without any obligation on its part, pay the taxes due for and in" +
                                 " behalf of the BUYER, with right of reimbursement from the BUYER, with interest and penalties at the same rate as those charged in case of default in the " +
                                "payment of the balance of the CONTRACT PRICE. Such interest and penalties shall be computed from the time payments were made by the SELLER until the same are " +
                                "fully reimbursement by the BUYER.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Withholding Tax and Local Transfer Tax", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The withholding tax and local transfer tax or its equivalent tax on the sale of the UNIT to the BUYER shall be for the account of the SELLER.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Value-Added Tax and Documentary Stamp Tax", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The value added tax, if any, documentary stamp tax, registration fees and any all other fees and expenses(except local transfer taxes) required to transfer title to the UNIIT in the nae of the BUYER shall be for the BUYER’s account.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("9.Default", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("If the BUYER defaults in the performance of their obligations under this Contract, including but not limited to the non payment of any obligation regarding telephone, cable, " +
                                 "electric and water connections and deposits, as well as assessments, association dues and similar fees, the SELLER, at their option, may cancel and rescind this Contract upon " +
                                 "writted notice to the BUYER/S and without need of any judicial declaration to that effect. In such case, any amount paid on accunt of the UNIT by the BUYER is not entitled to reimbursement " +
                                 "if his/her payment is less than two(2) years.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The above, however, is without prejudice to  the application of the provisions of Republic Act(R.A) No. 6552, otherwise knows as the ‘Realty Installment Buyers Protection Act’ which is " +
                                 "hereby made na integral part hereof. In case of such cancellation or rescission,  the SELLER shall be at liberty to dispose  of and sell the UNIT to any other person in the same manner as if this" +
                                " Contract has never been executed or entered into.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("10.Breach of Contract", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Breach by the BUYER of any of the conditions contained herein shall have the same effect as nonpayment of the installment and other payment obligations, as provided in the preceding paragraphs.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("11.Assignment of Rights", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The BUYER hereby agrees that the SELLER shall have the right to sell, assign or transfer to one or more, purchasers, assignees or transferees any" +
                                 " and all its rights and interest under this Contract, including all its receivables due hereunder, and/or the UNIT subject hereof; Provided, however, that any such purchaser," +
                                 " assignee or transferee expressly binds itself to honor the terms and conditions of this Contract with respect to the rights of the BUYER. The BUYER likewise agrees that the SELLER" +
                                 " shall have the right to mortgage the entire subdivision project or portions thereof, including the UNIT in conformity with provision of PD 957 or BP 220; Provided, however, that upon" +
                                 "  the BUYER’s full payment of the CONTRACT PRICE, the title of the UNIT shall be delivered by the SELLER to the BUYER free from any and all kinds of liens and encumbrances.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The BUYER hereby agrees that the SELLER shall have the right to sell, assign or transfer to one or more, purchasers, assignees or transferees any" +
                                 " and all its rights and interest under this Contract, including all its receivables due hereunder, and/or the UNIT subject hereof; Provided, however, that any such purchaser," +
                                 " assignee or transferee expressly binds itself to honor the terms and conditions of this Contract with respect to the rights of the BUYER. The BUYER likewise agrees that the SELLER" +
                                 " shall have the right to mortgage the entire subdivision project or portions thereof, including the UNIT in conformity with provision of PD 957 or BP 220; Provided, however, that upon" +
                                 "  the BUYER’s full payment of the CONTRACT PRICE, the title of the UNIT shall be delivered by the SELLER to the BUYER free from any and all kinds of liens and encumbrances.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The BUYER may not assign, sell or transfer its rights under this contract, or any right or interest herein or in the UNIT, without prior written notice to and conformity of " +
                                "the SELLER. In case the SELLER approves the assignment, the BUYER shall pay the SELLER a transfer fee in the amount of P15,100.00  or such other amount as the SELLER may otherwise fix. However," +
                                " he BUYER may, without securing a formal approval from the SELLER, assign its rights and interests under this Contract in favor of the assignee bank/financial institution (not applicable) to secure " +
                                "a loan which the BUYER may obtain from said bank to finance payment of the balance of the CONTRACT PRICE for the UNIT to the SELLER. Any such purchaser, assignee or transferee expressly binds himself " +
                                "to honor the terms and conditions of this Contract with respect to the rights and interest of the SELLER.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("12. Title and Ownership of the Unit", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The SELLER shall execute or cause the execution of a separate Deed of Absolute Sale and the issuance of the Certificate of Title to the Unit in favor of the BUYER," +
                                " their successor and assigns, therby conveying to the BUYER, their successors and assign the title, rights and interests in the UNIT as soon as the following shall have been " +
                                "accomplished:", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(a)Payment in full of the CONTRACT PRICE and any ann all interests, penalties and other charges such as, but not limited to, telephone, cable, electric and water " +
                                "connections and deposits which may have accrued or which may have been advanced by the SELLER, including all other obligations of the BUYER under this Contract such as" +
                                " insurance premiums, cost of repairs, real state taxes advanced by the SELLER and bank charges or interests incidental to the BUYER’S loan or financial package;", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(b)Issuance by the Registry of Deeds of the individual Certificate of Title covering the Unit in the name of the BUYER; and", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(c)Payment of the membership fee to the Associaton, or to the SELLER if payment of such amount had been advanced by the SELLER, in such amount as shall be determined by the latter.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("In the event that the Deed of Absolute Sale is executed prior to the BUYER’s settlement of association dues, electric and water deposits, and other advances/fees as may be imposed or incurred due to the BUYER’s financing requirements, the SELLER shall not deliver the UNIT, or the Certificate of Title therefor, until such time as all of the BUYER’S payables are settled in full.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("13.Warranties", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The SELLER warrants and guarantees (a) the authenticity and validity of the title to the UNIT subject of this Contact and undertakes to defend the same against" +
                                " all claims of any and all persons and entities; (b) that the title to the UNIIT is free from liens and encumbrances, except for the mortgage, if any, referred " +
                                "to herein, those provided in the Declaration of Covenants, Conditions and Restrictions, those imposed by law, the Articles of Incorporation and By Laws of the" +
                                " Association, zoning regulations and other restrictions on the use and occupancy of the UNIT as may be imposed by government and other authorities having" +
                                " junsdiction thereon, and to other restrictions and easements of record; and (c) that the UNIT is free from and clear of tenants, occupants and squatters and " +
                                "undertakes to hold the BUYER, their successor and assigns, free and harmless from any liability or responsibility with regard to any such tenants, occupants or " +
                                "squatters, or their eviction from the UNIT.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("14. Completion of Construction of the Unit", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The SELLER projects, without any warranty or covenant, the completion of construction of the UNIT and the subdivision project within the timetable allowed by HLURB, and/or other competent authority, unless prevented by “force majeure”.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The term “force majeure” as used herein refers to any condition, event, cause or reason beyond the control of the SELLER, including but not limited to, any act of God, strikes, lockouts or other " +
                                "industrial disturbances, serious civil disturbances, unavoidable accidents, blow out, acts of the public enemy, war ,blockade, public riot, fire, flood, explosion, governmental or municipal restraint, court or " +
                                "administrative injunctions or other court or administrative orders stopping or interfering with the work progress, shortage or unavailability of equipment, materials or labor, restrictions or limitations upon the " +
                                "user thereof and/or acts of third person/s.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Should the SELLER be delayed in the construction or completion of the UNIT or the subdivision project due to force majeure, the SELLER shall" +
                                " be entitled to such additional period of time sufficient to enable it to complete the construction of the same as shall correspond to the period of delay due" +
                                " to such cause. Should any condition or, cause beyond the control of the SELLER arise which renders the completion of the UNIT or the subdivision project no " +
                                "longer possible, the SELLER shall be relieved of any obligation arising out of this Contract, except to reimburse the BUYER whatever it may have received from" +
                                " them under and by virtue of this Contract, without interest in any event at all.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The BUYER expressly agrees and accepts that the failure of the SELLER to complete the UNIT or the subdivision project within the period specified above due to any force majeure shall " +
                                "not be a ground to rescind or cancel this Contract and the SELLER have no liability whatsoever to the BUYER for such non completion, except as provided in the immediately preceding paragraph and " +
                                "Section 23 of Presidentail Decreee(PD) No. 967.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The SELLER may not be compelled to complete the construction of the UNIT priod to the BUYER’s full settlement of the downpayment and " +
                                 "any additional amounts due relative thereto, and the delivery of the postdated check to cover the BUYER’s monthly amortization payments.", fontArial11));

                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("15. Delivery of the Unit", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The possession of the Unit shall be delivered by the SELLER to the BUYER within reasonable period of time from the" +
                                " date of completion of construction of such UNIT and its related facilities. It is understood, however, that physical possession of the PROPERTY shall not" +
                                " be delivered by the SELLER to the BUYER unless the later shall have complied with all conditions and requirements prescribed for this purpose by the" +
                                " SELLER to the BUYER unless the latter shall have complied with all conditions and requirements prescribed for this purpose by the SELLER under its" +
                                " policies prevailing at the time.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Upon completion of the UNIT, the SELLER shall serve in the BUYER a written notice of turn over stating the date on which the UNIT shall be ready " +
                                "for delivery or occupancy by the BUYER. If the BUYER is not in default, the possession of the UNIT shall be delivered to them. The BUYER shall be given a " +
                                "reasonable opportunity to inspect and examine the UNIT before acceptance of the same. Provided however, that if no inspection is made on or before the date or" +
                                " within the period stated in the notice, the UNIT shall be deemed to have already been inspected by the BUYER and the same shall be considered as to have been " +
                                "completed and delivered in the date specified in the Notice.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Within the prescribed period for inspection prior to the turnover of the UNIT to the BUYER, the BUYER shall register with the SELLER their written " +
                                "complaint on any defect. Failure to so register such complaint shall be deemed an unqualified and unconditional acceptance of the UNIT by the BUYER and shall constitute" +
                                " a bar for future complaint or action on the same.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The BUYER shall be deemed to have taken possession of the UNIT in any of the following or analogous circumstances:	" +
                                "(1) on the date specified in the SELLER’s notice of turnover  and upon the BUYER’s actual or constructive receipt thereof irrespective of their " +
                                "non-occupancy of the UNIT for any reason whatsoever;		(2) when the BUYER actually occupies the UNIT;		(3) when the BUYER commences to introduce " +
                                "improvements, alterations, furnishing, etc. on the UNIT;		(4) when the BUYER takes or receives the keys to the UNIT;		" +
                                "(5) when the BUYER accepts the UNIT or when the UNIT is deemed accepted as provided herein,", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("From and after the date specified in notice of turnover, or when the BUYER takes possession of the UNIT in accordance with the immediately " +
                                "preceding paragraph, notwithstanding title to the UNIT had not been transferred to the BUYER, the BUYER shall, in place of the SELLER, observe all the" +
                                " conditions and restrictions on the UNIT and shall henceforth be liable for all risk of loss or damage to the UNIT, charges and fees for utilities and service," +
                                " taxes and homeowners’ association dues, and other related obligations and assessments pertaining to the UNIT.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The BUYER shall, before moving into the UNIT. Pay membership and other dues assessed on the UNIT by the Homeowners’" +
                                 " Association to be established in the subdivision project.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Upon moving in, the BUYER shall pay move-in fees covering the determined cost incurred by the SELLER for pedestal, " +
                                  "electrical connection and water connection of the HOUSE and LOT.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("16. Insurance", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("The BUYER shall obtain and maintain the following insurance until the BUYER has fully paid the Contrast Price and its related" +
                                " charges to the SELLER, with the SELLER or its assignee as the designated beneficiary:", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(a) Redemption Insurance – This Insurance, which cover risk in case of death of the BUYER, is subject to the Schedule of Insurance " +
                                  "in the SELLER’s Master Policy.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(b) Fire Insurance – The buyer shall obtain fire as well as allied peril insurance/s on the UNIT for an amount equivalent to at least " +
                                "the contract value of the residential unit and/or its improvements. The premiums for this coverage shall be prepared annually by the BUYER. " +
                                "The initial year’s prepayment shall, be deducted from, the Contrast proceeds, while the repayments for the succeeding years shall be collected" +
                                " together with the BUYER’s monthly installment payments.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(c) Other insurances as may be required for purposes of the BUYER’s housing loan.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("17. Miscellaneous Provisions", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(a) The BUYER warrants in full the truth of the representations made in the applications for the purchase of the UNIT subject of this Contract, " +
                                 "and any falsehood or misrepresentation stated therein shall be sufficient ground for the cancellation or rescission of this Contract.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(b) The BUYER shall notify the SELLER in writing of any change in their mailing address. Should the BUYER fails to do so, their address stated in the Contract shall remain their address for all intents and purposes.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(c) Discrepancy of less than ten percent (10%) in the approximate gross are of the UNIT as stated in the Contract, in brochures or price list than the actual area of the UNIT when completed, shall not result in an increase or decrease in the selling price.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(d) The SELLER reserves the right to construct other improvements on available, unutilized or vacant land or space surrounding or adjacent to the UNIT and hereby reserves its ownership thereof.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(d1) The SELLER may upgrade/downgrade/revise house specification as part of the exercise of its right pursuant to this Contract being developer", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(e)	In the event that the subdivision project and UNIT becomes not economically feasible such that there are adverse conditions, changes and its structure, " +
                                "or other similar factors or reasons, the SELLER may, upon written notice to the BUYER, change or alter the designe, specifications and/or the price of the UNIT or replace" +
                                " the same with a similar lot, or cancel this Contract and return in full, without interest, all payments received from the BUYER.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(f) If the sale of the UNIT hereunder constitutes “bulk buying” subject to the provisions of HLURB Administrative Order NO. 09, Series " +
                                "of 1994, or the HLURB Rules and Regulations on Bulk Buying, the BUYER hereby agrees and undertakes to comply  with the provisions of the aforesaid" +
                                " Administrative Order.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(g) The BUYER agrees to be bound by all terms and conditions on the Declaration of Restrictions for the Subdivision Project and the Articles of " +
                                "Incorporation and By Laws of the homeowners association, copies of which shall be duly finished upon request of the BUYER. The BUYER further confirms that his obligations" +
                                " under this Contract will survive upon payment of the CONTRACT PRICE and the execution of the Deed of Absolute Sale.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("(h) Any reference to any party to this Contract includes such party’s successor and assigns.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("18. Venue", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("Should the SELLER be constrained to resort to courts to project its rights or to seek redress for its grievances under this Contract," +
                                " or to defend itself against any action or proceeding instituted by the BUYER or any other party arising from this Contract or any related document," +
                                " the BUYER shall further pay the SELLER, as and by way of attorney’s fees, a sum equivalent to at least twenty percent (20%) of the total amount" +
                                " due or involved , or the amount of fifty thousand pesos (P50,000.00) whichever is higher, in addition to the cost and expenses of litigation, and " +
                                "to the actual and other damages provided hereinabove to which the SELLER shall be entitled  by law and under this Contract. Any actions or " +
                                "proceedings related to this Contract shall be brought before proper courts of Cebu City, all other venues being expressly waived.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("19. Separability Cluase", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("In case one or more of the provisions contained in this Contract to Sell shall be declared invalid, illegal or unenforceable in any " +
                    "respect by a competent authority, the validity, legality, and enforceability of the remaining provisions contained herein shall not in any way be " +
                    "affected or impaired thereby.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("20. Repealing Clase", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("This Contract cancels and supersedes all previous  Contracts between tha parties herein and this Contract shall not be considered as changed, modified," +
                              " altered or in any manner amended by acts of tolerance of the SELLER unless such changes, modifications, alterations or amendments are made in writing and signed by " +
                              "both parties to this contract.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("21. The BUYER hereby represent/s that (i) this Contract has been read, understood and accepted by them; (ii) the obligations of the BUYER hereunder and" +
                                " under the Deed of Absolute Sale, including their compliance with the Declaration of Covenants, Conditions and Restrictions constitutes legal, valid and binding obligations," +
                                " fully enforceable against them; and (iii) the BUYER has full power, authority and legal right to execute, deliver and perform this Contract and the Deed of Sale.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("IN WITNESS WHEREOF, The parties hereto signed this instrument on the date and the place hereinbefore mentioned.", fontArial11));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);
                p5.Add(new Chunk("PRILAND DEVELOPMENT CORPORATION", fontArial11Bold));
                p5.Add(Chunk.NEWLINE);
                p5.Add(Chunk.NEWLINE);


                //WRITE TO PDF
                document.Add(p5);

                //LAST PAGE
                p6.Add(new Chunk("SELLER", fontArial10));
                p7.Add(new Chunk("REPRESENTED BY", fontArial10));
                p7.Add(Chunk.NEWLINE);
                p7.Add(Chunk.NEWLINE);

                p8.Add(new Chunk("RAMON CARLO T. YAP", fontArial12Bold));
                p8.IndentationLeft = 75f;

                p8.Add(Chunk.NEWLINE);
                p9.Add(new Chunk("President", fontArial10));
                p9.IndentationLeft = 98f;

                p9.Add(Chunk.NEWLINE);
                p9.Add(Chunk.NEWLINE);
                p10.Add(new Chunk("Buyer/s", fontArial10));
                p10.Add(Chunk.NEWLINE);
                p10.Add(Chunk.NEWLINE);

                p11.Add(new Chunk(Customer, fontArial12Bold));
                p11.IndentationLeft = 78f;
                p12.Add(Chunk.NEWLINE);
                p12.Add(Chunk.NEWLINE);
                p12.Alignment = Element.ALIGN_CENTER;
                p12.Add(new Chunk("Signed in the presence of:", fontArial11));
                p12.Add(Chunk.NEWLINE);
                p12.Add(Chunk.NEWLINE);
                p12.Add(Chunk.NEWLINE);
                p12.Add(Chunk.NEWLINE);
                p13.Alignment = Element.ALIGN_CENTER;
                p13.Add(new Chunk("ACKNOWLEDGEMENT", fontArial14Bold));
                p13.Add(Chunk.NEWLINE);
                p13.Add(Chunk.NEWLINE);
                p14.Add(new Chunk("REPUBLIC OF THE PHILIPPINES", fontArial11));
                p14.Add(Chunk.NEWLINE);
                p14.Add(new Chunk("CITY OF CEBU", fontArial11));
                p14.Add(Chunk.NEWLINE);
                p14.Add(Chunk.NEWLINE);
                p14.Add(new Chunk("BEFORE ME a Notary Public for and in the above jurisdiction, this______ day of __________________, personally appeared the following:", fontArial11));


            }
            //WRITE TO PDF
            document.Add(p6);
            document.Add(p7);
            document.Add(p8);
            document.Add(p9);
            document.Add(p10);
            document.Add(p11);
            document.Add(p12);
            document.Add(p13);
            document.Add(p14);


            document.Close();

            byte[] byteInfo = workStream.ToArray();

            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(workStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = byteInfo.Length;

            ContentDispositionHeaderValue contentDisposition = null;
            if (ContentDispositionHeaderValue.TryParse("inline; filename=customer.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            return response;
        }
    }
}
