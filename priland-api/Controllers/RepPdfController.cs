using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Http.Headers;
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
        private Font fontArial12BoldItalic = FontFactory.GetFont("Arial", 12, Font.BOLDITALIC);
        private Font fontArial12 = FontFactory.GetFont("Arial", 12);
        private Font fontArial12Italic = FontFactory.GetFont("Arial", 12, Font.ITALIC);
        private Font fontArial13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD);
        private Font fontArial13 = FontFactory.GetFont("Arial", 13);
        private Font fontArial14Bold = FontFactory.GetFont("Arial", 14, Font.BOLD);
        private Font fontArial14 = FontFactory.GetFont("Arial", 14);
        private Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
        private Font fontArial17 = FontFactory.GetFont("Arial", 17);
        private Font CourierNew11 = FontFactory.GetFont("Courier", 11);

        // ========
        // PDF Line
        // ========
        private Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

        // =====================
        // Convert To Money Word
        // =====================
        public static String GetMoneyWord(String input)
        {
            String decimals = "";
            if (input.Contains("."))
            {
                decimals = input.Substring(input.IndexOf(".") + 1);
                input = input.Remove(input.IndexOf("."));
            }

            String strWords = GetMoreThanThousandNumberWords(input);
            if (decimals.Length > 0)
            {
                if (Convert.ToDecimal(decimals) > 0)
                {
                    String getFirstRoundedDecimals = new String(decimals.Take(2).ToArray());
                    strWords += " Pesos And " + GetMoreThanThousandNumberWords(getFirstRoundedDecimals) + " Cents Only";
                }
                else
                {
                    strWords += " Pesos Only";
                }
            }
            else
            {
                strWords += " Pesos Only";
            }

            return strWords;
        }

        // ===================================
        // Get More Than Thousand Number Words
        // ===================================
        private static String GetMoreThanThousandNumberWords(string input)
        {
            try
            {
                String[] seperators = { "", " Thousand ", " Million ", " Billion " };

                int i = 0;

                String strWords = "";

                while (input.Length > 0)
                {
                    String _3digits = input.Length < 3 ? input : input.Substring(input.Length - 3);
                    input = input.Length < 3 ? "" : input.Remove(input.Length - 3);

                    Int32 no = Int32.Parse(_3digits);
                    _3digits = GetHundredNumberWords(no);

                    _3digits += seperators[i];
                    strWords = _3digits + strWords;

                    i++;
                }

                return strWords;
            }
            catch
            {
                return "Invalid Amount";
            }
        }

        // =====================================
        // Get From Ones to Hundred Number Words
        // =====================================
        private static String GetHundredNumberWords(Int32 no)
        {
            String[] Ones =
            {
                "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
                "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen"
            };

            String[] Tens = { "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            String word = "";

            if (no > 99 && no < 1000)
            {
                Int32 i = no / 100;
                word = word + Ones[i - 1] + " Hundred ";
                no = no % 100;
            }

            if (no > 19 && no < 100)
            {
                Int32 i = no / 10;
                word = word + Tens[i - 1] + " ";
                no = no % 10;
            }

            if (no > 0 && no < 20)
            {
                word = word + Ones[no - 1];
            }

            return word;
        }

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
                // ============
                // Company Logo
                // ============
                var projectLogo = from d in db.MstProjects
                                  select d;

                Image logo = Image.GetInstance(projectLogo.FirstOrDefault().ProjectLogo);
                logo.ScaleToFit(1000f, 60f);

                PdfPTable pdfTableCompanyLogo = new PdfPTable(1);
                pdfTableCompanyLogo.SetWidths(new float[] { 100f });
                pdfTableCompanyLogo.WidthPercentage = 100;
                pdfTableCompanyLogo.AddCell(new PdfPCell(logo) { Border = 0 });
                document.Add(pdfTableCompanyLogo);

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
                String SpouseLastName = String.IsNullOrEmpty(customer.FirstOrDefault().SpouseLastName) == true ? " " : customer.FirstOrDefault().SpouseLastName;
                String SpouseFirstName = String.IsNullOrEmpty(customer.FirstOrDefault().SpouseFirstName) == true ? " " : customer.FirstOrDefault().SpouseFirstName;
                String SpouseMiddleName = String.IsNullOrEmpty(customer.FirstOrDefault().SpouseMiddleName) == true ? " " : customer.FirstOrDefault().SpouseMiddleName;
                String SpouseBirthDate = String.IsNullOrEmpty(customer.FirstOrDefault().SpouseBirthDate.ToString()) == true ? " " : customer.FirstOrDefault().SpouseBirthDate.ToString();
                String SpouseCitizen = String.IsNullOrEmpty(customer.FirstOrDefault().SpouseCitizen) == true ? " " : customer.FirstOrDefault().SpouseCitizen;
                String SpouseTIN = String.IsNullOrEmpty(customer.FirstOrDefault().SpouseTIN) == true ? " " : customer.FirstOrDefault().SpouseTIN;
                String SpouseEmployer = String.IsNullOrEmpty(customer.FirstOrDefault().SpouseEmployer) == true ? " " : customer.FirstOrDefault().SpouseEmployer;

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

                // =====================================
                // Table Customer (Spouse's Information)
                // =====================================
                PdfPTable pdfTableCustomerSpouseInformation = new PdfPTable(4);
                pdfTableCustomerSpouseInformation.SetWidths(new float[] { 100f, 100f, 100f, 100f });
                pdfTableCustomerSpouseInformation.WidthPercentage = 100;

                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(new Phrase("SPOUSE'S INFORMATION", fontArial10BoldItalic)) { Colspan = 4, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f, BackgroundColor = BaseColor.BLACK });

                Phrase spouseTitlePhraseLabel = new Phrase("Title \n\n", fontArial10Bold);
                Phrase spouseTitlePhraseData = new Phrase(" ", fontArial13);
                Paragraph titleParagraph = new Paragraph
                {
                    spouseTitlePhraseLabel, spouseTitlePhraseData
                };

                Phrase spouseFirstNamePhraseLabel = new Phrase("First Name \n\n", fontArial10Bold);
                Phrase spouseFirstNamePhraseData = new Phrase(SpouseFirstName, fontArial13);
                Paragraph spouseFirstNameParagraph = new Paragraph
                {
                    spouseFirstNamePhraseLabel, spouseFirstNamePhraseData
                };

                Phrase spouseMiddleNamePhraseLabel = new Phrase("Middle Name \n\n", fontArial10Bold);
                Phrase spouseMiddleNamePhraseData = new Phrase(SpouseMiddleName, fontArial13);
                Paragraph spouseMiddleNamePhraseDataParagraph = new Paragraph
                {
                    spouseMiddleNamePhraseLabel, spouseMiddleNamePhraseData
                };

                Phrase spouseLastNamePhraseLabel = new Phrase("Last Name \n\n", fontArial10Bold);
                Phrase spouseLastNamePhraseData = new Phrase(SpouseLastName, fontArial13);
                Paragraph spouseLastNamePhraseDataParagraph = new Paragraph
                {
                    spouseLastNamePhraseLabel, spouseLastNamePhraseData
                };

                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(titleParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseFirstNameParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseMiddleNamePhraseDataParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseLastNamePhraseDataParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase spouseBirthdayPhraseLabel = new Phrase("Birthday \n\n", fontArial10Bold);
                Phrase spouseBirthdayPhraseData = new Phrase(SpouseBirthDate, fontArial13);
                Paragraph spouseBirthdayParagraph = new Paragraph
                {
                    spouseBirthdayPhraseLabel, spouseBirthdayPhraseData
                };

                Phrase spouseCivilStatusPhraseLabel = new Phrase("Civil Status \n\n", fontArial10Bold);
                Phrase spouseCivilStatusPhraseData = new Phrase("", fontArial13);
                Paragraph spouseCivilStatusParagraph = new Paragraph
                {
                    spouseCivilStatusPhraseLabel, spouseCivilStatusPhraseData
                };

                Phrase spouseSexPhraseLabel = new Phrase("Sex \n\n", fontArial10Bold);
                Phrase spouseSexPhraseData = new Phrase(" ", fontArial13);
                Paragraph spouseSexParagraph = new Paragraph
                {
                    spouseSexPhraseLabel, spouseSexPhraseData
                };

                Phrase spouseCitizenshipPhraseLabel = new Phrase("Citizenship \n\n", fontArial10Bold);
                Phrase spouseCitizenshipPhraseData = new Phrase(SpouseCitizen, fontArial13);
                Paragraph spouseCitizenshipParagraph = new Paragraph
                {
                    spouseCitizenshipPhraseLabel, spouseCitizenshipPhraseData
                };

                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseBirthdayParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseCivilStatusParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseSexParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseCitizenshipParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase spouseAddressPhraseLabel = new Phrase("Address \n\n", fontArial10Bold);
                Phrase spouseAddressPhraseData = new Phrase(" ", fontArial13);
                Paragraph spouseAddressParagraph = new Paragraph
                {
                    spouseAddressPhraseLabel, spouseAddressPhraseData
                };

                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseAddressParagraph) { Colspan = 4, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                Phrase spouseContactNumberPhraseLabel = new Phrase("Contact Number \n\n", fontArial10Bold);
                Phrase spouseContactNumberPhraseData = new Phrase(" ", fontArial13);
                Paragraph spouseContactNumberCityParagraph = new Paragraph
                {
                    spouseContactNumberPhraseLabel, spouseContactNumberPhraseData
                };

                Phrase spouseMobilePhraseLabel = new Phrase("Mobile \n\n", fontArial10Bold);
                Phrase spouseMobilePhraseData = new Phrase("", fontArial13);
                Paragraph spouseMobileParagraph = new Paragraph
                {
                    spouseMobilePhraseLabel, spouseMobilePhraseData
                };

                Phrase spouseNameOfCompanyPhraseLabel = new Phrase("Name of Company \n\n", fontArial10Bold);
                Phrase spouseNameOfCompanyPhraseData = new Phrase(" ", fontArial13);
                Paragraph spouseNameOfCompanyParagraph = new Paragraph
                {
                    spouseMobilePhraseLabel, spouseMobilePhraseData
                };

                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseContactNumberCityParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseMobileParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseNameOfCompanyParagraph) { Colspan = 2, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });


                Phrase spouseCompanyAddressPhraseLabel = new Phrase("Company Address \n\n", fontArial10Bold);
                Phrase spouseCompanyAddressPhraseData = new Phrase(" ", fontArial13);
                Paragraph spouseCompanyAddressParagraph = new Paragraph
                {
                    spouseCompanyAddressPhraseLabel, spouseCompanyAddressPhraseData
                };

                Phrase spouseSalaryPhraseLabel = new Phrase("Salary \n\n", fontArial10Bold);
                Phrase spouseSalaryPhraseData = new Phrase(" ", fontArial13);
                Paragraph spouseSalaryParagraph = new Paragraph
                {
                    spouseSalaryPhraseLabel, spouseSalaryPhraseData
                };

                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseCompanyAddressParagraph) { Colspan = 3, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseSalaryParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });


                Phrase spousePositionPhraseLabel = new Phrase("Position \n\n", fontArial10Bold);
                Phrase spousePositionPhraseData = new Phrase(" ", fontArial13);
                Paragraph spousePositionParagraph = new Paragraph
                {
                    spousePositionPhraseLabel, spousePositionPhraseData
                };

                Phrase spouseNoOfYearsPhraseLabel = new Phrase("No. of Years \n\n", fontArial10Bold);
                Phrase spouseNoOfYearsPhraseData = new Phrase(" ", fontArial13);
                Paragraph spouseNoOfYearseNoParagraph = new Paragraph
                {
                    spouseNoOfYearsPhraseLabel, spouseNoOfYearsPhraseData
                };

                Phrase spouseCancelledCardPhraseLabel = new Phrase("Cancelled Card \n\n", fontArial10Bold);
                Phrase spouseCancelledCardPhraseData = new Phrase(" ", fontArial13);
                Paragraph spouseCancelledCardParagraph = new Paragraph
                {
                    spouseCancelledCardPhraseLabel, spouseCancelledCardPhraseData
                };

                Phrase spouseExistingLoanPhraseLabel = new Phrase("Existing Loan \n\n", fontArial10Bold);
                Phrase spouseExistingLoanPhraseData = new Phrase(" ", fontArial13);
                Paragraph spouseExistingLoanParagraph = new Paragraph
                {
                    spouseExistingLoanPhraseLabel, spouseExistingLoanPhraseData
                };

                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spousePositionParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseNoOfYearseNoParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseCancelledCardParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });
                pdfTableCustomerSpouseInformation.AddCell(new PdfPCell(spouseExistingLoanParagraph) { PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f, PaddingBottom = 6f });

                document.Add(pdfTableCustomerSpouseInformation);
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

            //if (sysSettings.Any())
            //{
            //    // ===============
            //    // Company Details
            //    // ===============
            //    PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
            //    pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
            //    pdfTableCompanyDetail.WidthPercentage = 100;
            //    pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase(sysSettings.FirstOrDefault().Company, fontArial17Bold)) { Border = 0 });
            //    pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("Investment Proposal Summary", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            //    document.Add(pdfTableCompanyDetail);
            //    document.Add(line);

            //    document.Add(spaceTable);
            //}

            // =============
            // Get Sold Unit
            // =============
            var soldUnit = from d in db.TrnSoldUnits
                           where d.Id == Convert.ToInt32(id)
                           select d;

            if (soldUnit.Any())
            {
                Image logo = Image.GetInstance(soldUnit.FirstOrDefault().MstProject.ProjectLogo);
                logo.ScaleToFit(1000f, 60f);

                PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
                pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
                pdfTableCompanyDetail.WidthPercentage = 100;
                pdfTableCompanyDetail.AddCell(new PdfPCell(logo) { Border = 0 });
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("Investment Proposal Summary", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
                document.Add(pdfTableCompanyDetail);
                document.Add(line);

                document.Add(spaceTable);

                // ==============
                // Sold Unit Data
                // ==============
                String Customer = soldUnit.FirstOrDefault().MstCustomer.FirstName + " " + soldUnit.FirstOrDefault().MstCustomer.MiddleName + " " + soldUnit.FirstOrDefault().MstCustomer.LastName;
                String Unit = "Block " + soldUnit.FirstOrDefault().MstUnit.Block + " Lot " + soldUnit.FirstOrDefault().MstUnit.Lot;
                String Project = soldUnit.FirstOrDefault().MstProject.Project;
                String TotalInvestment = soldUnit.FirstOrDefault().TotalInvestment;
                String PaymentOptions = soldUnit.FirstOrDefault().PaymentOptions;
                String Financing = soldUnit.FirstOrDefault().Financing;
                String PreparedByName = soldUnit.FirstOrDefault().MstUser3.FullName;
                String CheckByName = soldUnit.FirstOrDefault().MstUser.FullName;
                string ApprovedByName = soldUnit.FirstOrDefault().MstUser1.FullName;
                String Broker = soldUnit.FirstOrDefault().MstBroker.FirstName + " " + soldUnit.FirstOrDefault().MstBroker.MiddleName + " " + soldUnit.FirstOrDefault().MstBroker.LastName;
                String Agent = soldUnit.FirstOrDefault().Agent;
                String BrokerCoordinator = soldUnit.FirstOrDefault().BrokerCoordinator;
                Decimal Price = soldUnit.FirstOrDefault().Price;
                Decimal TLA = soldUnit.FirstOrDefault().MstUnit.TLA;
                Decimal TFA = soldUnit.FirstOrDefault().MstUnit.TFA;
                Decimal Reservation = soldUnit.FirstOrDefault().Reservation;

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
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase(TLA.ToString("#,##0.00") + " sq. m.", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Floor Area", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase(TFA.ToString("#,##0.00") + " sq. m.", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Project", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase(Project, fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Reservation Fee", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSoldUnitHeaderProposal.AddCell(new PdfPCell(new Phrase("Php " + Reservation.ToString("#,##0.00"), fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(pdfTableSoldUnitHeaderProposal);

                document.Add(line);

                // ================
                // Total Investment
                // ================
                var totalInvestmentPhrase = new Phrase("TOTAL INVESTMENT", fontArial11Bold);
                var totalInvestmentDataPhrase = new Phrase(TotalInvestment, CourierNew11);

                Paragraph totalInvestmentHeaderParagraph = new Paragraph
                {
                    totalInvestmentPhrase
                };

                Paragraph totalInvestmentParagraph = new Paragraph
                {
                    totalInvestmentDataPhrase
                };

                totalInvestmentHeaderParagraph.IndentationLeft = 5f;
                totalInvestmentHeaderParagraph.IndentationRight = 5f;
                document.Add(totalInvestmentHeaderParagraph);
                totalInvestmentParagraph.IndentationLeft = 20f;
                totalInvestmentParagraph.IndentationRight = 20f;
                document.Add(totalInvestmentParagraph);
                document.Add(line);

                // ========================
                // Flexible Payment Options
                // ========================
                var flexiblePaymentOptionsPhrase = new Phrase("FLEXIBLE PAYMENT OPTIONS", fontArial11Bold);
                var PaymentOptionsDataPhrase = new Phrase(PaymentOptions, CourierNew11);

                Paragraph totalInvestmentHeaderHeaderParagraph = new Paragraph
                {
                    flexiblePaymentOptionsPhrase
                };

                Paragraph flexiblePaymentOptionsParagraph = new Paragraph
                {
                    PaymentOptionsDataPhrase
                };

                totalInvestmentHeaderHeaderParagraph.IndentationLeft = 5f;
                totalInvestmentHeaderHeaderParagraph.IndentationRight = 5f;
                document.Add(totalInvestmentHeaderHeaderParagraph);
                flexiblePaymentOptionsParagraph.IndentationLeft = 20f;
                flexiblePaymentOptionsParagraph.IndentationRight = 20f;

                document.Add(flexiblePaymentOptionsParagraph);

                document.Add(spaceTable);

                var soldUnitEquitySchedules = from d in db.TrnSoldUnitEquitySchedules
                                              orderby d.PaymentDate ascending
                                              where d.SoldUnitId == Convert.ToInt32(id)
                                              select d;

                if (soldUnitEquitySchedules.Any())
                {
                    PdfPTable scheduleTable = new PdfPTable(6);
                    scheduleTable.SetWidths(new float[] { 60f, 60f, 60f, 60f, 150f, 150f });
                    scheduleTable.WidthPercentage = 100;

                    scheduleTable.AddCell(PhraseCell(new Phrase("Payment Date", fontArial11Bold), 1, 1));
                    scheduleTable.AddCell(PhraseCell(new Phrase("Amortization", fontArial11Bold), 1, 1));
                    scheduleTable.AddCell(PhraseCell(new Phrase("Check Number", fontArial11Bold), 1, 1));
                    scheduleTable.AddCell(PhraseCell(new Phrase("Check Date", fontArial11Bold), 1, 1));
                    scheduleTable.AddCell(PhraseCell(new Phrase("Check Bank", fontArial11Bold), 1, 1));
                    scheduleTable.AddCell(PhraseCell(new Phrase("Remarks", fontArial11Bold), 1, 1));

                    foreach (var schedule in soldUnitEquitySchedules)
                    {
                        String paymentDate = schedule.PaymentDate.ToShortDateString();
                        String amortization = schedule.Amortization.ToString("0.00");
                        String checkNumber = schedule.CheckNumber;
                        String checkDate = schedule.CheckDate == null ? "" : schedule.CheckDate.Value.ToShortDateString();
                        String checkBank = schedule.CheckBank;
                        String remarks = schedule.Remarks;

                        scheduleTable.AddCell(PhraseCell(new Phrase(paymentDate, fontArial11Bold), 2, 1));
                        scheduleTable.AddCell(PhraseCell(new Phrase(amortization, fontArial11Bold), 2, 1));
                        scheduleTable.AddCell(PhraseCell(new Phrase(checkNumber, fontArial11Bold), 0, 1));
                        scheduleTable.AddCell(PhraseCell(new Phrase(checkDate, fontArial11Bold), 2, 1));
                        scheduleTable.AddCell(PhraseCell(new Phrase(checkBank, fontArial11Bold), 0, 1));
                        scheduleTable.AddCell(PhraseCell(new Phrase(remarks, fontArial11Bold), 0, 1));
                    }

                    document.Add(scheduleTable);
                }

                document.Add(line);

                // =====================
                // Easy Financing Scheme
                // =====================
                var easyFinancingSchemePhrase = new Phrase("EASY FINANCING SCHEME", fontArial11Bold);
                var financingDataPhrase = new Phrase(Financing, CourierNew11);

                Paragraph easyFinancingSchemeHeaderParagraph = new Paragraph
                {
                    easyFinancingSchemePhrase
                };

                Paragraph easyFinancingSchemeParagraph = new Paragraph
                {
                    financingDataPhrase
                };

                easyFinancingSchemeHeaderParagraph.IndentationLeft = 5f;
                easyFinancingSchemeHeaderParagraph.IndentationRight = 5f;
                document.Add(easyFinancingSchemeHeaderParagraph);
                easyFinancingSchemeParagraph.IndentationLeft = 20f;
                easyFinancingSchemeParagraph.IndentationRight = 20f;
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

                noteParagraph.IndentationLeft = 5f;
                noteParagraph.IndentationRight = 5f;
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
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Agent: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(Agent, fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ")) { Colspan = 5, Border = 0, PaddingTop = 30f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Broker Coordinator: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(BrokerCoordinator, fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Broker: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(Broker, fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ")) { Colspan = 5, Border = 0, PaddingTop = 30f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Verified by: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(CheckByName, fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Conforme: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(Customer, fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("SIGNATURE OVER PRINTED NAME", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Investor", fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ")) { Colspan = 5, Border = 0, PaddingTop = 30f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase("Approved by: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(ApprovedByName, fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });

                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableUserSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingLeft = 5f, PaddingRight = 5f });

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

        // ======================
        // PDF Sold Unit Contract
        // ======================
        [HttpGet, Route("SoldUnitContract/{id}")]
        public HttpResponseMessage PdfSoldUnitContract(int id)
        {
            Font updateFontArial10 = FontFactory.GetFont("Arial", 7);
            Font updateFontArial10Bold = FontFactory.GetFont("Arial", 7, Font.BOLD);
            Font updateFontArial10BoldItalic = FontFactory.GetFont("Arial", 5, Font.BOLDITALIC, BaseColor.WHITE);
            Font updateFontArial12Bold = FontFactory.GetFont("Arial", 7, Font.BOLD);
            Font updateFontArial12BoldItalic = FontFactory.GetFont("Arial", 7, Font.BOLDITALIC);
            Font updateFontArial12 = FontFactory.GetFont("Arial", 7);
            Font updateFontArial12Italic = FontFactory.GetFont("Arial", 7, Font.ITALIC);
            Font updateFontArial17Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font updateFontArial11Bold = FontFactory.GetFont("Arial", 7, Font.BOLD);

            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.SetPageSize(new Rectangle(612, 1728));
            document.SetMargins(30f, 30f, 10f, 10f);

            // =============
            // Open Document
            // =============
            document.Open();

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 5f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 80;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", updateFontArial10Bold)) { PaddingTop = 1f, Border = 0 });

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
                //PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
                //pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
                //pdfTableCompanyDetail.WidthPercentage = 100;
                //pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase(sysSettings.FirstOrDefault().Company, fontArial17Bold)) { Border = 0 });
                //pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("Contract to Sell", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 }); 
                //document.Add(pdfTableCompanyDetail);
                //document.Add(line);

                //document.Add(spaceTable);
            }

            // =============
            // Get Sold Unit
            // =============
            var soldUnit = from d in db.TrnSoldUnits
                           where d.Id == Convert.ToInt32(id)
                           select d;

            if (soldUnit.Any())
            {
                Image logo = Image.GetInstance(soldUnit.FirstOrDefault().MstProject.ProjectLogo);
                logo.ScaleToFit(1000f, 60f);

                PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
                pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
                pdfTableCompanyDetail.WidthPercentage = 100;
                pdfTableCompanyDetail.AddCell(new PdfPCell(logo) { Border = 0 });
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("Contract to Sell", updateFontArial17Bold)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 2 });
                document.Add(pdfTableCompanyDetail);
                document.Add(line);

                document.Add(spaceTable);


                Paragraph p1 = new Paragraph
                {
                    new Chunk("KNOW ALL MEN BY THESE PRESENTS", updateFontArial12)
                };

                p1.SetLeading(7f, 0);
                document.Add(p1);
                document.Add(spaceTable);

                Phrase p2Phrase = new Phrase(
                    "This Contact to Sell (hereinafter referred to as the “Contract”) made and entered into this day of " + soldUnit.FirstOrDefault().SoldUnitDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + "at Cebu City, Cebu, Philippines, by and between:", updateFontArial12);


                Paragraph p2 = new Paragraph
                {
                    p2Phrase
                };

                p2.SetLeading(7f, 0);


                p2.FirstLineIndent = 40f;
                document.Add(p2);
                document.Add(spaceTable);

                if (sysSettings.Any())
                {
                    Phrase p3Phrase = new Phrase(sysSettings.FirstOrDefault().Company, updateFontArial12Bold);
                    Phrase p3Phrase2 = new Phrase(
                        ", a corporation duly organized and existing under and by virtue of the laws of the Philippines,"
                        + " with principal office address at Priland Development Corporation 18th Floor, BPI Cebu Corporation Center Cor. Archbishop Reyes & Luzon Ave. Cebu Business Park, Cebu City 6000,"
                        + " represented in this contract by ________________, now and hereinafter referred to as the “SELLER”;", updateFontArial12);

                    Paragraph p3 = new Paragraph
                    {
                        p3Phrase, p3Phrase2
                    };
                    p3.SetLeading(7f, 0);

                    p3.Alignment = Element.ALIGN_JUSTIFIED;
                    p3.IndentationLeft = 80f;
                    p3.IndentationRight = 80f;
                    document.Add(p3);
                }

                document.Add(spaceTable);

                String Customer = soldUnit.FirstOrDefault().MstCustomer.FirstName + " " + soldUnit.FirstOrDefault().MstCustomer.MiddleName + " " + soldUnit.FirstOrDefault().MstCustomer.LastName;
                String Citizen = soldUnit.FirstOrDefault().MstCustomer.Citizen;
                String Spouse = soldUnit.FirstOrDefault().MstCustomer.SpouseFirstName + " " + soldUnit.FirstOrDefault().MstCustomer.SpouseMiddleName + " " + soldUnit.FirstOrDefault().MstCustomer.SpouseLastName;
                String SpouseCitizen = soldUnit.FirstOrDefault().MstCustomer.SpouseCitizen;
                String Address = soldUnit.FirstOrDefault().MstCustomer.Address + " " +
                                 soldUnit.FirstOrDefault().MstCustomer.City + " " +
                                 soldUnit.FirstOrDefault().MstCustomer.Province + " " +
                                 soldUnit.FirstOrDefault().MstCustomer.Country;
                String CivilStatus = soldUnit.FirstOrDefault().MstCustomer.CivilStatus;

                if (CivilStatus.Equals("MARRIED"))
                {
                    Phrase p4Phrase = new Phrase("-and-", updateFontArial12);
                    Paragraph p4 = new Paragraph
                    {
                        p4Phrase
                    };

                    p4.SetLeading(7f, 0);

                    p4.Alignment = 1;
                    document.Add(p4);
                    document.Add(spaceTable);

                    Phrase p5Phrase = new Phrase(Customer, updateFontArial12Bold);
                    Phrase p5Phrase1 = new Phrase(", of legal age, " + Citizen + " married to ", updateFontArial12);
                    Phrase p5Phrase2 = new Phrase(Spouse, updateFontArial12);
                    Phrase p5Phrase3 = new Phrase(", and with residence and postal address at ", updateFontArial12);
                    Phrase p5Phrase4 = new Phrase(Address, updateFontArial12);
                    Phrase p5Phrase5 = new Phrase(", hereinafter referred as the “BUYER”.", updateFontArial12);

                    Paragraph p5 = new Paragraph {
                                    p5Phrase, p5Phrase1, p5Phrase2, p5Phrase3, p5Phrase4, p5Phrase5
                                };

                    p5.SetLeading(7f, 0);
                    p5.Alignment = Element.ALIGN_JUSTIFIED;
                    p5.IndentationLeft = 80f;
                    p5.IndentationRight = 80f;

                    document.Add(p5);
                    document.Add(spaceTable);
                }
                else
                {
                    Phrase p4Phrase = new Phrase("-and-", updateFontArial12);
                    Paragraph p4 = new Paragraph
                    {
                        p4Phrase
                    };

                    p4.SetLeading(7f, 0);
                    p4.Alignment = 1;
                    document.Add(p4);
                    document.Add(spaceTable);

                    Phrase p5Phrase = new Phrase(Customer, updateFontArial12Bold);
                    Phrase p5Phrase1 = new Phrase(", of legal age, " + Citizen, updateFontArial12);
                    Phrase p5Phrase3 = new Phrase(", with residence and postal address at ", updateFontArial12);
                    Phrase p5Phrase4 = new Phrase(Address, updateFontArial12);
                    Phrase p5Phrase5 = new Phrase(", hereinafter referred as the “BUYER”.", updateFontArial12);

                    Paragraph p5 = new Paragraph {
                                    p5Phrase, p5Phrase1, p5Phrase3, p5Phrase4, p5Phrase5
                                };
                    p5.SetLeading(7f, 0);
                    p5.Alignment = Element.ALIGN_JUSTIFIED;
                    p5.IndentationLeft = 80f;
                    p5.IndentationRight = 80f;

                    document.Add(p5);
                    document.Add(spaceTable);
                }

                var coOwners = from d in db.TrnSoldUnitCoOwners
                               where d.SoldUnitId == Convert.ToInt32(id)
                               select d;

                if (coOwners.Any())
                {
                    foreach (var coOwner in coOwners)
                    {
                        String coOwnerCustomer = coOwner.MstCustomer.FirstName + " " + coOwner.MstCustomer.MiddleName + " " + coOwner.MstCustomer.LastName;
                        String coOwnerSpouse = coOwner.MstCustomer.SpouseFirstName + " " + coOwner.MstCustomer.SpouseMiddleName + " " + coOwner.MstCustomer.SpouseLastName;

                        if (coOwner.MstCustomer.CivilStatus.Equals("MARRIED"))
                        {
                            Phrase p4Phrase = new Phrase("-and-", updateFontArial12);
                            Paragraph p4 = new Paragraph
                            {
                                p4Phrase
                            };
                            p4.SetLeading(7f, 0);
                            p4.Alignment = 1;
                            document.Add(p4);
                            document.Add(spaceTable);

                            Phrase p5Phrase = new Phrase(coOwnerCustomer, updateFontArial12Bold);
                            Phrase p5Phrase1 = new Phrase(", of legal age, " + Citizen + " married to ", updateFontArial12);
                            Phrase p5Phrase2 = new Phrase(coOwnerSpouse, updateFontArial12);
                            Phrase p5Phrase3 = new Phrase(", and with residence and postal address at ", updateFontArial12);
                            Phrase p5Phrase4 = new Phrase(coOwner.MstCustomer.Address, updateFontArial12);
                            Phrase p5Phrase5 = new Phrase(", hereinafter referred as the “BUYER”.", updateFontArial12);

                            Paragraph p5 = new Paragraph {
                                    p5Phrase, p5Phrase1, p5Phrase2, p5Phrase3, p5Phrase4, p5Phrase5
                                };
                            p5.SetLeading(7f, 0);

                            p5.Alignment = Element.ALIGN_JUSTIFIED;
                            p5.IndentationLeft = 80f;
                            p5.IndentationRight = 80f;

                            document.Add(p5);
                            document.Add(spaceTable);
                        }
                        else
                        {
                            Phrase p4Phrase = new Phrase("-and-", updateFontArial12);
                            Paragraph p4 = new Paragraph
                            {
                                p4Phrase
                            };
                            p4.SetLeading(7f, 0);

                            p4.Alignment = 1;
                            document.Add(p4);
                            document.Add(spaceTable);

                            Phrase p5Phrase = new Phrase(coOwnerCustomer, updateFontArial12Bold);
                            Phrase p5Phrase1 = new Phrase(", of legal age, " + Citizen, updateFontArial12);
                            Phrase p5Phrase3 = new Phrase(", with residence and postal address at ", updateFontArial12);
                            Phrase p5Phrase4 = new Phrase(coOwner.MstCustomer.Address, updateFontArial12);
                            Phrase p5Phrase5 = new Phrase(", hereinafter referred as the “BUYER”.", updateFontArial12);

                            Paragraph p5 = new Paragraph {
                                p5Phrase, p5Phrase1, p5Phrase3, p5Phrase4, p5Phrase5
                            };
                            p5.SetLeading(7f, 0);

                            p5.Alignment = Element.ALIGN_JUSTIFIED;
                            p5.IndentationLeft = 80f;
                            p5.IndentationRight = 80f;

                            document.Add(p5);
                            document.Add(spaceTable);
                        }
                    }
                }

                Phrase p6Phrase = new Phrase("WITNESSETH:", updateFontArial12);
                Paragraph p6 = new Paragraph
                {
                    p6Phrase
                };

                p6.SetLeading(7f, 0);
                p6.Alignment = 1;
                document.Add(p6);
                document.Add(spaceTable);

                Phrase p7Phrase = new Phrase("That for and in consideration of the sums of money to  be paid in the manner herein below specified, and the undertaking of the BUYER/S"
                    + " to fully perform and comply with all his/her/their obligations,covenants,conditions,and restrictions as herein specified and as enumerated"
                    + " in the DECLARATION OF COVENANTS,CONDITIONS AND RESTRICTIONS (attached hereto as Annex “A” and hereby made an integral part thereof), the SELLER"
                    + " hereby agrees and contracts to sell to the BUYER, and the latter hereby agree/s and contract/s to buy form the former, one(1) dwelling unit,"
                    + " situated in " + soldUnit.FirstOrDefault().MstProject.Address + ", which unit is specifically identified as (as hereinafter referred to as UNIT):", updateFontArial12);
                Paragraph p7 = new Paragraph
                {
                    p7Phrase
                };

                p7.SetLeading(7f, 0);
                p7.FirstLineIndent = 20f;
                p7.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(p7);
                document.Add(spaceTable);

                // ======================
                // Project / Unit Details
                // ======================
                PdfPTable pdfTableProjectContract = new PdfPTable(3);
                pdfTableProjectContract.SetWidths(new float[] { 120f, 20f, 200f });
                pdfTableProjectContract.WidthPercentage = 80;
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase("Project", updateFontArial12)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(":", updateFontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().MstProject.Project, updateFontArial12Bold)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase("Floor", updateFontArial12)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(":", updateFontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().MstUnit.Block, updateFontArial12Bold)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase("Unit", updateFontArial12)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(":", updateFontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().MstUnit.Lot, updateFontArial12Bold)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase("Lot Area", updateFontArial12)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(":", updateFontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().MstUnit.TLA.ToString("#,##0.00") + " square meters", updateFontArial12Bold)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase("House Model", updateFontArial12)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(":", updateFontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().MstUnit.MstHouseModel.HouseModel, updateFontArial12Bold)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase("Floor Area", updateFontArial12)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(":", updateFontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableProjectContract.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().MstUnit.TFA.ToString("#,##0.00") + " square meters", updateFontArial12Bold)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(pdfTableProjectContract);
                document.Add(spaceTable);

                Phrase p8Phrase = new Phrase("The said DECLARATION OF COVENANTS, CONDITIONS AND RESTRICTIONS shall be annotated as liens and easements on the  corresponding certificate of"
                    + " title to be issued to the BUYER/S upon compliance with all his/her/their obligations as specified hereunder:", updateFontArial12);
                Paragraph p8 = new Paragraph
                {
                    p8Phrase
                };

                p8.SetLeading(7f, 0);
                p8.Alignment = Element.ALIGN_JUSTIFIED;
                p8.FirstLineIndent = 40f;
                document.Add(p8);
                document.Add(spaceTable);

                Phrase p9Phrase = new Phrase("1. Contract Price and Manner of Payment", updateFontArial12BoldItalic);
                Paragraph p9 = new Paragraph
                {
                    p9Phrase
                };

                p9.SetLeading(7f, 0);
                document.Add(p9);
                document.Add(spaceTable);

                Decimal price = soldUnit.FirstOrDefault().Price;
                GetMoneyWord(price.ToString());

                Phrase p10Phrase = new Phrase("a.	The TOTAL CONTRACT PRICE for the above-described HOUSE AND LOT/UNIT shall be ", updateFontArial12);
                Phrase p10Phrase1 = new Phrase(GetMoneyWord(price.ToString()) + " (Php " + price.ToString("#,##0.00") + ")", updateFontArial12Bold);
                Phrase p10Phrase2 = new Phrase(", breakdown as follows: ", updateFontArial12);
                Paragraph p10 = new Paragraph
                {
                    p10Phrase, p10Phrase1, p10Phrase2
                };

                p10.SetLeading(7f, 0);
                p10.Alignment = Element.ALIGN_JUSTIFIED;
                p10.FirstLineIndent = 40f;
                document.Add(p10);
                document.Add(spaceTable);

                // ==========
                // Breakdowns
                // ==========
                PdfPTable pdfTableBreakdown = new PdfPTable(2);
                pdfTableBreakdown.SetWidths(new float[] { 120f, 200f });
                pdfTableBreakdown.WidthPercentage = 80;
                pdfTableBreakdown.AddCell(new PdfPCell(new Phrase("Particulars", updateFontArial12Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableBreakdown.AddCell(new PdfPCell(new Phrase("Amount", updateFontArial12Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableBreakdown.AddCell(new PdfPCell(new Phrase("Selling Price", updateFontArial12)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableBreakdown.AddCell(new PdfPCell(new Phrase(price.ToString("#,##0.00"), updateFontArial12Bold)) { HorizontalAlignment = 2, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableBreakdown.AddCell(new PdfPCell(new Phrase("Value Added Tax (VAT)", updateFontArial12)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableBreakdown.AddCell(new PdfPCell(new Phrase("0", updateFontArial12Bold)) { HorizontalAlignment = 2, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableBreakdown.AddCell(new PdfPCell(new Phrase("Processing costs", updateFontArial12)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableBreakdown.AddCell(new PdfPCell(new Phrase("0", updateFontArial12Bold)) { HorizontalAlignment = 2, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(pdfTableBreakdown);
                document.Add(spaceTable);

                decimal netEquity = soldUnit.FirstOrDefault().NetEquity;
                decimal netEquityPayments = soldUnit.FirstOrDefault().NetEquityNoOfPayments;
                decimal netEquityAmortization = soldUnit.FirstOrDefault().NetEquityAmortization;
                decimal netEquityInterest = soldUnit.FirstOrDefault().NetEquityInterest;
                decimal balance = soldUnit.FirstOrDefault().Balance;

                decimal equitySpotPayment1 = soldUnit.FirstOrDefault().EquitySpotPayment1;
                decimal equitySpotPayment2 = soldUnit.FirstOrDefault().EquitySpotPayment2;
                decimal equitySpotPayment3 = soldUnit.FirstOrDefault().EquitySpotPayment3;

                int payment1 = 1;
                int payment2 = Convert.ToInt32(netEquityPayments / 2);
                int payment3 = Convert.ToInt32(netEquityPayments);

                int equitySpotPayment1Pos = soldUnit.FirstOrDefault().EquitySpotPayment1Pos == 0 ? payment1 : soldUnit.FirstOrDefault().EquitySpotPayment1Pos;
                int equitySpotPayment2Pos = soldUnit.FirstOrDefault().EquitySpotPayment2Pos == 0 ? payment2 : soldUnit.FirstOrDefault().EquitySpotPayment2Pos;
                int equitySpotPayment3Pos = soldUnit.FirstOrDefault().EquitySpotPayment3Pos == 0 ? payment3 : soldUnit.FirstOrDefault().EquitySpotPayment3Pos;

                // Price
                //Phrase p11Phrase1 = new Phrase("The Total Contract price of ", fontArial12);
                //Phrase p11Phrase2 = new Phrase(GetMoneyWord(price.ToString()) + " (Php " + price.ToString("#,##0.00") + "), ", fontArial12Bold);
                //Phrase p11Phrase3 = new Phrase("shall be paid.  ", fontArial12);

                //Paragraph p11a = new Paragraph {
                //    p11Phrase1, p11Phrase2, p11Phrase3
                //};
                //p11a.Alignment = Element.ALIGN_JUSTIFIED;
                //p11a.FirstLineIndent = 80f;
                //document.Add(p11a);

                Paragraph downpaymentParagraph = new Paragraph();

                //Equity
                if (netEquity > 0)
                {
                    if (equitySpotPayment1 + equitySpotPayment2 + equitySpotPayment3 == 0)
                    {
                        Phrase p11Phrase4 = new Phrase("Net of RESERVATION and DISCOUNT is ", updateFontArial12);
                        Phrase p11Phrase4a = new Phrase(GetMoneyWord(netEquity.ToString()) + " (Php " + netEquity.ToString("#,##0.00") + ").  ", updateFontArial12Bold);

                        if (netEquityPayments > 0)
                        {
                            Phrase p11Phrase5 = new Phrase("Payable in " + netEquityPayments.ToString("0") + " months at ", updateFontArial12);
                            Phrase p11Phrase6 = new Phrase(GetMoneyWord(netEquityAmortization.ToString()) + " (Php " + netEquityAmortization.ToString("#,##0.00") + ").  ", updateFontArial12);

                            if (netEquityInterest > 0)
                            {
                                Phrase p11Phrase7 = new Phrase("Having an interest of " + netEquityInterest.ToString("0") + "%. ", updateFontArial12);

                                Paragraph p11c = new Paragraph { p11Phrase5, p11Phrase6, p11Phrase7 };
                                p11c.SetLeading(7f, 0);
                                p11c.Alignment = Element.ALIGN_JUSTIFIED;
                                p11c.FirstLineIndent = 40f;

                                downpaymentParagraph.Add(p11c);
                            }
                            else
                            {
                                Paragraph p11b = new Paragraph { p11Phrase5, p11Phrase6 };
                                p11b.SetLeading(7f, 0);
                                p11b.Alignment = Element.ALIGN_JUSTIFIED;
                                p11b.FirstLineIndent = 40f;

                                downpaymentParagraph.Add(p11b);
                            }
                        }
                        else
                        {
                            Paragraph p11a = new Paragraph { };
                            p11a.SetLeading(7f, 0);
                            p11a.Alignment = Element.ALIGN_JUSTIFIED;
                            p11a.FirstLineIndent = 40f;

                            downpaymentParagraph.Add(p11a);
                        }
                    }
                    else
                    {
                        Phrase p11Phrase4 = new Phrase("Net of RESERVATION, DISCOUNT and all SPOT PAYMENTS is ", updateFontArial12);
                        Phrase p11Phrase4a = new Phrase(GetMoneyWord(netEquity.ToString()) + " (Php " + netEquity.ToString("#,##0.00") + ").  ", updateFontArial12Bold);

                        if (netEquityPayments > 0)
                        {
                            Phrase p11Phrase5 = new Phrase("Payable in " + netEquityPayments.ToString("0") + " months at ", updateFontArial12);
                            Phrase p11Phrase6 = new Phrase(GetMoneyWord(netEquityAmortization.ToString()) + " (Php " + netEquityAmortization.ToString("#,##0.00") + ").  With ", updateFontArial12);
                            Phrase p11Phrase7 = new Phrase("", updateFontArial12);
                            Phrase p11Phrase8 = new Phrase("", updateFontArial12);
                            Phrase p11Phrase9 = new Phrase("", updateFontArial12);
                            Phrase p11Phrase10 = new Phrase("", updateFontArial12);
                            Phrase p11Phrase11 = new Phrase("", updateFontArial12);
                            Phrase p11Phrase12 = new Phrase("", updateFontArial12);

                            if (equitySpotPayment1 > 0)
                            {
                                p11Phrase7 = new Phrase(GetMoneyWord(equitySpotPayment1.ToString()) + " (Php " + equitySpotPayment1.ToString("#,##0.00") + ") on the " + equitySpotPayment1Pos.ToString("0") + "st payment.  ", updateFontArial12);
                            }
                            if (equitySpotPayment2 > 0)
                            {
                                //decimal payment2 = netEquityPayments / 2;
                                if (equitySpotPayment1 > 0) p11Phrase8 = new Phrase("And ", updateFontArial12);
                                p11Phrase9 = new Phrase(GetMoneyWord(equitySpotPayment2.ToString()) + " (Php " + equitySpotPayment2.ToString("#,##0.00") + ") on the " + equitySpotPayment2Pos.ToString("0") + "th payment.  ", updateFontArial12);
                            }
                            if (equitySpotPayment3 > 0)
                            {
                                //decimal payment3 = netEquityPayments;
                                if (equitySpotPayment1 + equitySpotPayment2 > 0) p11Phrase10 = new Phrase("And ", updateFontArial12);
                                p11Phrase11 = new Phrase(GetMoneyWord(equitySpotPayment3.ToString()) + " (Php " + equitySpotPayment3.ToString("#,##0.00") + ") on the " + equitySpotPayment3Pos.ToString("0") + "th payment.  ", updateFontArial12);
                            }

                            if (netEquityInterest > 0)
                            {
                                p11Phrase12 = new Phrase("Having an interest of " + netEquityInterest.ToString("0") + "%. ", updateFontArial12);
                            }

                            Paragraph p11c = new Paragraph { p11Phrase5,
                                                             p11Phrase6,
                                                             p11Phrase7,
                                                             p11Phrase8,
                                                             p11Phrase9,
                                                             p11Phrase10,
                                                             p11Phrase11,
                                                             p11Phrase12};
                            p11c.SetLeading(7f, 0);
                            p11c.Alignment = Element.ALIGN_JUSTIFIED;
                            p11c.FirstLineIndent = 40f;

                            downpaymentParagraph.Add(p11c);
                        }
                        else
                        {
                            Paragraph p11a = new Paragraph { };
                            p11a.SetLeading(7f, 0);
                            p11a.Alignment = Element.ALIGN_JUSTIFIED;
                            p11a.FirstLineIndent = 40f;

                            downpaymentParagraph.Add(p11a);
                        }
                    }

                }

                //document.Add(spaceTable);

                Paragraph balanceParagraph = new Paragraph();



                // Balance
                if (balance > 0)
                {
                    Phrase p11Phrase8 = new Phrase("The remaining BALANCE of ", updateFontArial12);
                    Phrase p11Phrase8a = new Phrase(GetMoneyWord(balance.ToString()) + " (Php " + balance.ToString("#,##0.00") + ") ", updateFontArial12Bold);
                    Phrase p11Phrase9 = new Phrase("through preferred financing instrument.  ", updateFontArial12);

                    Paragraph p11d = new Paragraph { p11Phrase8, p11Phrase8a, p11Phrase9 };
                    p11d.SetLeading(7f, 0);
                    p11d.Alignment = Element.ALIGN_JUSTIFIED;
                    p11d.FirstLineIndent = 40f;

                    balanceParagraph.Add(p11d);
                }

                //document.Add(spaceTable);

                Phrase p11Phrase = new Phrase("b.	The TOTAL CONTRACT PRICE  shall be payable as follows", updateFontArial12);
                Paragraph p11 = new Paragraph
                {
                    p11Phrase
                };

                p11.SetLeading(7f, 0);
                p11.Alignment = Element.ALIGN_JUSTIFIED;
                p11.FirstLineIndent = 40f;
                document.Add(p11);
                document.Add(spaceTable);

                // ==============
                // Contract Price
                // ==============
                PdfPTable pdfTableContractPrice = new PdfPTable(3);
                pdfTableContractPrice.SetWidths(new float[] { 100f, 100f, 200f });
                pdfTableContractPrice.WidthPercentage = 100;
                pdfTableContractPrice.AddCell(new PdfPCell(new Phrase("Particulars", updateFontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(new Phrase("Amount (Php)", updateFontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(new Phrase("Payment Terms", updateFontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(new Phrase("Reservation Fee", updateFontArial10)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().Reservation.ToString("#,##0.00"), updateFontArial10)) { HorizontalAlignment = 2, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(new Phrase("", updateFontArial10)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(new Phrase("Downpayment", updateFontArial10)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().NetEquity.ToString("#,##0.00"), updateFontArial10)) { HorizontalAlignment = 2, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(downpaymentParagraph) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(new Phrase("Balance", updateFontArial10)) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().Balance.ToString("#,##0.00"), updateFontArial10)) { HorizontalAlignment = 2, PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableContractPrice.AddCell(new PdfPCell(balanceParagraph) { PaddingTop = 1f, PaddingBottom = 3f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(pdfTableContractPrice);
                document.Add(spaceTable);

                Phrase p12Phrase = new Phrase("Postdated checks shall be issued by the BUYER hereof to cover all the abovementioned installments. Any unpaid balance of the CONTRACT " +
                                              "PRICE shall earn an interest of 3% per month.", updateFontArial12);

                Paragraph p12 = new Paragraph
                {
                    p12Phrase
                };

                p12.SetLeading(7f, 0);
                p12.FirstLineIndent = 40f;
                document.Add(p12);
                document.Add(spaceTable);

                Phrase p13Phrase = new Phrase("No delay or omission in exercising any right granted to the SELLER under this Contract shall be construed as a waiver thereof and the receipt"
                    + " by the SELLER of any payment made in a manner other than as herein provided shall not be construed as a modification of the terms hereof. "
                    + "In the event the SELLER accepts the BUYER’s payment after due date, such payment shall include an additional sum to cover penaltieson delayed installment "
                    + "at the rate of 3% per month of delay on the amount due. The imposition of the penalty shall be without prejudice to the availment  by the seller of any remedy "
                    + "provided hereunder and by law. Moreover, acceptance of said payment should not be construed as condonation  of any subsequent failure, delay or default by "
                    + "the BUYER.", updateFontArial12);

                Paragraph p13 = new Paragraph
                {
                    p13Phrase
                };

                p13.SetLeading(7f, 0);

                p13.Alignment = Element.ALIGN_JUSTIFIED;
                p13.FirstLineIndent = 40f;
                document.Add(p13);
                document.Add(spaceTable);

                Phrase p14Phrase = new Phrase("2. Financing Option", updateFontArial12BoldItalic);
                Paragraph p14 = new Paragraph
                {
                    p14Phrase
                };
                p14.SetLeading(7f, 0);
                document.Add(p14);
                document.Add(spaceTable);

                Phrase p15Phrase = new Phrase("Notwithstanding the amortization schedule agreed upon, the BUYER may opt to pay the remaining balance of the CONTRACT PRICE through a loan which he/she "
                    + "may obtain from any public or private financing institution, the fee as and charges of which shall be for their own account.", updateFontArial12);

                Paragraph p15 = new Paragraph
                {
                    p15Phrase
                };
                p15.SetLeading(7f, 0);
                p15.Alignment = Element.ALIGN_JUSTIFIED;
                p15.FirstLineIndent = 40f;
                document.Add(p15);
                document.Add(spaceTable);

                Phrase p16Phrase = new Phrase("Should the loan approved for the BUYER be less than the balance of the CONTRACT PRICE, the BUYER shall pay the SELLER the amount corresponding to the "
                    + "difference upon approval of the said loan. Upon the BUYER’S availment of the said loan to the SELLER, the above schedule of payment shall be considered of no "
                    + "further effect and/or amended as the case may be.", updateFontArial12);

                Paragraph p16 = new Paragraph
                {
                    p16Phrase
                };

                p16.SetLeading(7f, 0);
                p16.Alignment = Element.ALIGN_JUSTIFIED;
                p16.FirstLineIndent = 40f;
                document.Add(p16);
                document.Add(spaceTable);

                Phrase p17Phrase = new Phrase("3. Place of Payment", updateFontArial12BoldItalic);
                Paragraph p17 = new Paragraph
                {
                    p17Phrase
                };
                p17.SetLeading(7f, 0);
                document.Add(p17);
                document.Add(spaceTable);

                Phrase p18Phrase = new Phrase("All payments other than those covered by the postdated checks due under this Contract shall be made by the BUYER to the SELLER’s cashiers at the SELLER’s"
                    + " office at Priland Development Corporation 18th Floor, BPI Cebu Corporation Center Cor. Archbishop Reyes & Luzon Ave. Cebu Business Park, Cebu City 6000, without necessity of demand or notice. Failure by the BUYER to do so shall entitle"
                    + " the SELLER to charge penalty at the rate of 3% per month.", updateFontArial12);

                Paragraph p18 = new Paragraph
                {
                    p18Phrase
                };
                p18.SetLeading(7f, 0);
                p18.Alignment = Element.ALIGN_JUSTIFIED;
                p18.FirstLineIndent = 40f;
                document.Add(p18);
                document.Add(spaceTable);

                Phrase p19Phrase = new Phrase("4. Solidary Liability", updateFontArial12BoldItalic);
                Paragraph p19 = new Paragraph
                {
                    p19Phrase
                };

                p19.SetLeading(7f, 0);
                document.Add(p19);
                document.Add(spaceTable);

                Phrase p20Phrase = new Phrase("If there are two or more BUYERS under this Contract, they shall be deemed solidarily liable for all the obligations of herein set forth.", updateFontArial12);

                Paragraph p20 = new Paragraph
                {
                    p20Phrase
                };

                p20.SetLeading(7f, 0);
                p20.Alignment = Element.ALIGN_JUSTIFIED;
                p20.FirstLineIndent = 40f;
                document.Add(p20);
                document.Add(spaceTable);

                Phrase p21Phrase = new Phrase("5. Application of Payments", updateFontArial12BoldItalic);
                Paragraph p21 = new Paragraph
                {
                    p21Phrase
                };

                p21.SetLeading(7f, 0);
                document.Add(p21);
                document.Add(spaceTable);

                Phrase p22Phrase = new Phrase("The SELLER shall have the right to determine the application of payments made by the BUYER. Unless otherwise indicated in the SELLER’s"
                    + " official receipt, payments shall be applied in the following order:", updateFontArial12);

                Paragraph p22 = new Paragraph
                {
                    p22Phrase
                };

                p2.SetLeading(7f, 0);
                p22.Alignment = Element.ALIGN_JUSTIFIED;
                p22.FirstLineIndent = 40f;
                document.Add(p22);
                document.Add(spaceTable);

                Phrase p23Phrase = new Phrase("(a) To costs and expenses incurred or advance by the SELLER pursuant to the Contract \n", updateFontArial12);
                Phrase p23Phrase2 = new Phrase("(b) To penalties \n", updateFontArial12);
                Phrase p23Phrase3 = new Phrase("(c) To interests \n", updateFontArial12);
                Phrase p23Phrase4 = new Phrase("(d) To the principal", updateFontArial12);

                Paragraph p23 = new Paragraph
                {
                    p23Phrase, p23Phrase2, p23Phrase3, p23Phrase4
                };

                p23.SetLeading(7f, 0);
                p23.Alignment = Element.ALIGN_JUSTIFIED;
                p23.IndentationLeft = 80f;
                p23.IndentationRight = 80f;
                document.Add(p23);
                document.Add(spaceTable);

                Phrase p24Phrase = new Phrase("6. Restrictions", updateFontArial12BoldItalic);
                Paragraph p24 = new Paragraph
                {
                    p24Phrase
                };

                p24.SetLeading(7f, 0);
                document.Add(p24);
                document.Add(spaceTable);

                Phrase p25Phrase = new Phrase("The BUYER shall not make any construction, alteration or renovations/additions on the UNIT without first obtaining the prior consent of the SELLER."
                    + " The building of the house or the renovations/additions to be made by the BUYER shall be subject to the approval of the SELLER. To ensure the proper"
                    + " conduct of the works, the BUYER shall post a cash bond in an amount to be fixed by the SELLER depending on the nature of the works to be undertaken,"
                    + " before commencing such works. Said bond shall be returned to the  BUYER upon completion of the construction, after deducting costs of utilities, damage"
                    + " to the common areas and other lots, and liability to third parties, if any.", updateFontArial12);

                Paragraph p25 = new Paragraph
                {
                    p25Phrase
                };

                p25.SetLeading(7f, 0);
                p25.Alignment = Element.ALIGN_JUSTIFIED;
                p25.FirstLineIndent = 40f;
                document.Add(p25);
                document.Add(spaceTable);

                Phrase p26Phrase = new Phrase("The BUYER  further agrees to strictly comply all the terms, conditions and limitations contained in the Declaration of Covenants, Conditions and Restrictions"
                    + " for the subdivision project, a copy of which is hereto attached as Annex “A” and made integral part hereof, as well as all the rules, regulations, and restrictions as may"
                    + " now or hereafter be required by the SELLER or the Association. The BUYER further confirms that his obligations under this Contract shall survive the full payment"
                    + " of the CONTRACT PRICE and the execution of the Deed of Absolute Sale.", updateFontArial12);

                Paragraph p26 = new Paragraph
                {
                    p26Phrase
                };

                p26.SetLeading(7f, 0);
                p26.Alignment = Element.ALIGN_JUSTIFIED;
                p26.FirstLineIndent = 40f;
                document.Add(p26);
                document.Add(spaceTable);

                Phrase p27Phrase = new Phrase("7. Homeowners’ Association", updateFontArial12BoldItalic);
                Paragraph p27 = new Paragraph
                {
                    p27Phrase
                };

                p27.SetLeading(7f, 0);
                document.Add(p27);
                document.Add(spaceTable);

                Phrase p28Phrase = new Phrase("For purposes of promoting and protecting their mutual interest and assist in their community development, the proper operation, administration"
                    + " and maintenance of the community’s facilities and utilities,cleanliness and beautification of subdivision premises, collection of garbage,"
                    + " security, fire protection, enforcement of the deed of restrictions and restrictive easements, and in general, for promoting the common benefit of"
                    + " the residents therein, the OWNER/DEVELOPER/SELLER shall initiate the organization of the homeowners’ association (referred to as the “Association”),"
                    + " which shall be a non stock, and non profit organization.", updateFontArial12);

                Paragraph p28 = new Paragraph
                {
                    p28Phrase
                };

                p28.SetLeading(7f, 0);
                p28.Alignment = Element.ALIGN_JUSTIFIED;
                p28.FirstLineIndent = 40f;
                document.Add(p28);
                document.Add(spaceTable);

                Phrase p29Phrase = new Phrase("The OWNER/SELLER/DEVELOPER and its representative/s are hereby authorized and empowered by the BUYER  to organize, incorporate and register the"
                    + " Association with the Housing Land Use Regulatory Board(HLURB), the Securities and Exchange Commission(SEC), the Local Government Unit concern and other"
                    + " government agencies, and/or entities of which the BUYER becomes an automatic member upon incorporation of the Association. The BUYER therefore agree/s and"
                    + " covenants to abide by its rules and regulations and to pay the dues and assessments duty levied and imposed by the Association.", updateFontArial12);

                Paragraph p29 = new Paragraph
                {
                    p29Phrase
                };

                p29.SetLeading(7f, 0);
                p29.Alignment = Element.ALIGN_JUSTIFIED;
                p29.FirstLineIndent = 40f;
                document.Add(p29);
                document.Add(spaceTable);

                Phrase p30Phrase = new Phrase("Association dues shall be assessed upon the BUYER for such purpose/s and in such time and manner as set forth in the Articles of Incorporation"
                    + " and By-Law, and in the rules and regulations to be adopted by the Association.", updateFontArial12);

                Paragraph p30 = new Paragraph
                {
                    p30Phrase
                };

                p30.SetLeading(7f, 0);
                p30.Alignment = Element.ALIGN_JUSTIFIED;
                p30.FirstLineIndent = 40f;
                document.Add(p30);
                document.Add(spaceTable);

                Phrase p31Phrase = new Phrase("The BUYER shall abide the rules and regulations issued by the SELLER or the Association in connection with the use and enjoyment of the facilities"
                    + " existing in the subdivision/village.", updateFontArial12);

                Paragraph p31 = new Paragraph
                {
                    p31Phrase
                };

                p31.SetLeading(7f, 0);
                p31.Alignment = Element.ALIGN_JUSTIFIED;
                p31.FirstLineIndent = 40f;
                document.Add(p31);
                document.Add(spaceTable);

                Phrase p32Phrase = new Phrase("Only unit owner/s in good standing are entitled to vote in any meeting where the vote is required or be voted upon in any election of the"
                    + " ASSOCIATION. The voting rights of unit owner/s who are not in good standing and of the amortizing buyers shall be executed by the"
                    + " SELLER/OWNER/DEVELOPER until such time as their respective obligation to the ASSOCIATION or to the SELLER are fully satisfied. A unit owner in"
                    + " good standing is one who has fully paid for his UNIT and is not delinquent in the payment of association dues and other assessments made by the"
                    + " ASSOCIATION.", updateFontArial12);

                Paragraph p32 = new Paragraph
                {
                    p32Phrase
                };

                p32.SetLeading(7f, 0);
                p32.Alignment = Element.ALIGN_JUSTIFIED;
                p32.FirstLineIndent = 40f;
                document.Add(p32);
                document.Add(spaceTable);

                Phrase p33Phrase = new Phrase("8. Taxes", updateFontArial12BoldItalic);
                Paragraph p33 = new Paragraph
                {
                    p33Phrase
                };

                p33.SetLeading(7f, 0);
                document.Add(p33);
                document.Add(spaceTable);

                Phrase p34Phrase = new Phrase("Real Property Tax", updateFontArial12Italic);
                Paragraph p34 = new Paragraph
                {
                    p34Phrase
                };

                p34.SetLeading(7f, 0);
                document.Add(p34);
                document.Add(spaceTable);

                Phrase p35Phrase = new Phrase("Real property and other taxes that may be levied on the UNIT during the effectivity of this Contract, including the corresponding subcharges"
                    + " and penalties in case of delinquency, shall be borne and paid by the BUYER from and after the title to the UNIT is registered in the BUYER’s name,"
                    + " or from the date possession of the UNIT is delivered to the BUYER, whichever comes first. The BUYER shall submit to the SELLER the official"
                    + " receipts evidencing payments of such liabilities within fifteen (15) days from the date such payments are made, which shall in no case be later"
                    + " than April 15 of each year.", updateFontArial12);

                Paragraph p35 = new Paragraph
                {
                    p35Phrase
                };

                p35.SetLeading(7f, 0);
                p35.Alignment = Element.ALIGN_JUSTIFIED;
                p35.FirstLineIndent = 40f;
                document.Add(p35);
                document.Add(spaceTable);

                Phrase p36Phrase = new Phrase("Should the BUYER fail to pay such taxes, the SELLER may, at its option but without any obligation on its part, pay the taxes due for and in"
                    + " behalf of the BUYER, with right of reimbursement from the BUYER, with interest and penalties at the same rate as those charged in case of default in the"
                    + " payment of the balance of the CONTRACT PRICE. Such interest and penalties shall be computed from the time payments were made by the SELLER until the same are"
                    + " fully reimbursed by the BUYER.", updateFontArial12);

                Paragraph p36 = new Paragraph
                {
                    p36Phrase
                };

                p36.SetLeading(7f, 0);
                p36.Alignment = Element.ALIGN_JUSTIFIED;
                p36.FirstLineIndent = 40f;
                document.Add(p36);
                document.Add(spaceTable);

                Phrase p37Phrase = new Phrase("Withholding Tax and Local Transfer Tax", updateFontArial12Italic);
                Paragraph p37 = new Paragraph
                {
                    p37Phrase
                };

                p37.SetLeading(7f, 0);
                document.Add(p37);
                document.Add(spaceTable);

                Phrase p38Phrase = new Phrase("The withholding tax and local transfer tax or its equivalent tax on the sale of the UNIT to the BUYER shall be for the account of the SELLER.", updateFontArial12);

                Paragraph p38 = new Paragraph
                {
                    p36Phrase
                };

                p38.SetLeading(7f, 0);
                p38.Alignment = Element.ALIGN_JUSTIFIED;
                p38.FirstLineIndent = 40f;
                document.Add(p38);
                document.Add(spaceTable);

                Phrase p39Phrase = new Phrase("Value-Added Tax and Documentary Stamp Tax", updateFontArial12Italic);
                Paragraph p39 = new Paragraph
                {
                    p39Phrase
                };

                p39.SetLeading(7f, 0);
                document.Add(p39);
                document.Add(spaceTable);

                Phrase p40Phrase = new Phrase("The value added tax, if any, documentary stamp tax, registration fees and any all other fees and expenses(except local transfer taxes) required to transfer title to the UNIT in the name of the BUYER shall be for the BUYER’s account.", updateFontArial12);

                Paragraph p40 = new Paragraph
                {
                    p40Phrase
                };

                p40.SetLeading(7f, 0);
                p40.Alignment = Element.ALIGN_JUSTIFIED;
                p40.FirstLineIndent = 40f;
                document.Add(p40);
                document.Add(spaceTable);

                Phrase p41Phrase = new Phrase("9. Default", updateFontArial12BoldItalic);
                Paragraph p41 = new Paragraph
                {
                    p41Phrase
                };

                p41.SetLeading(7f, 0);
                document.Add(p41);
                document.Add(spaceTable);

                Phrase p42Phrase = new Phrase("If the BUYER defaults in the performance of their obligations under this Contract, including but not limited to the non payment of any obligation regarding telephone, cable,"
                    + " electric and water connections and deposits, as well as assessments, association dues and similar fees, the SELLER, at their option, may cancel and rescind this Contract upon"
                    + " written notice to the BUYER/S and without need of any judicial declaration to that effect. In such case, any amount paid on account of the UNIT by the BUYER is not entitled to reimbursement"
                    + " if his/her payment is less than two(2) years.", updateFontArial12);

                Paragraph p42 = new Paragraph
                {
                    p42Phrase
                };

                p42.SetLeading(7f, 0);
                p42.Alignment = Element.ALIGN_JUSTIFIED;
                p42.FirstLineIndent = 40f;
                document.Add(p42);
                document.Add(spaceTable);

                Phrase p43Phrase = new Phrase("The above, however, is without prejudice to  the application of the provisions of Republic Act(R.A) No. 6552, otherwise knows as the ‘Realty Installment Buyers Protection Act’ which is"
                    + " hereby made an integral part hereof. In case of such cancellation or rescission,  the SELLER shall be at liberty to dispose  of and sell the UNIT to any other person in the same manner as if this"
                    + " Contract has never been executed or entered into.", updateFontArial12);

                Paragraph p43 = new Paragraph
                {
                    p43Phrase
                };

                p43.SetLeading(7f, 0);
                p43.Alignment = Element.ALIGN_JUSTIFIED;
                p43.FirstLineIndent = 40f;
                document.Add(p43);
                document.Add(spaceTable);

                Phrase p44Phrase = new Phrase("10. Breach of Contract", updateFontArial12BoldItalic);
                Paragraph p44 = new Paragraph
                {
                    p44Phrase
                };

                p44.SetLeading(7f, 0);
                document.Add(p44);
                document.Add(spaceTable);

                Phrase p45Phrase = new Phrase("Breach by the BUYER of any of the conditions contained herein shall have the same effect as nonpayment of the installment and other payment obligations, as provided in the preceding paragraphs.", updateFontArial12);

                Paragraph p45 = new Paragraph
                {
                    p45Phrase
                };

                p45.SetLeading(7f, 0);
                p45.Alignment = Element.ALIGN_JUSTIFIED;
                p45.FirstLineIndent = 40f;
                document.Add(p45);
                document.Add(spaceTable);

                Phrase p46Phrase = new Phrase("11. Assignment of Rights", updateFontArial12BoldItalic);
                Paragraph p46 = new Paragraph
                {
                    p46Phrase
                };

                p46.SetLeading(7f, 0);
                document.Add(p46);
                document.Add(spaceTable);

                Phrase p47Phrase = new Phrase("The BUYER hereby agrees that the SELLER shall have the right to sell, assign or transfer to one or more, purchasers, assignees or transferees any"
                    + " and all its rights and interest under this Contract, including all its receivables due hereunder, and/or the UNIT subject hereof; Provided, however, that any such purchaser,"
                    + " assignee or transferee expressly binds itself to honor the terms and conditions of this Contract with respect to the rights of the BUYER. The BUYER likewise agrees that the SELLER"
                    + " shall have the right to mortgage the entire subdivision project or portions thereof, including the UNIT in conformity with provision of PD 957 or BP 220; Provided, however, that upon"
                    + " the BUYER’s full payment of the CONTRACT PRICE, the title of the UNIT shall be delivered by the SELLER to the BUYER free from any and all kinds of liens and encumbrances.", updateFontArial12);

                Paragraph p47 = new Paragraph
                {
                    p47Phrase
                };

                p47.SetLeading(7f, 0);
                p47.Alignment = Element.ALIGN_JUSTIFIED;
                p47.FirstLineIndent = 40f;
                document.Add(p47);
                document.Add(spaceTable);

                Phrase p48Phrase = new Phrase("For purposes of availing and securing a loan or financing package to pay the balance of the CONTRACT PRICE,"
                    + " the BUYER recognizes and agrees to the right of the SELLER to assign all  its rights and receivables under  this Contract in favor of the funding bank or financial institution."
                    + " In such case, the BUYER undertakes to conform to the same and to perform faithfully all his obligations under this Contract without need of demand from the SELLER’s assignee."
                    + " Accordingly, the BUYER agrees that the assignee shall assume all the rights and interest of the SELLER under this Contract, and upon advice by the assignee,"
                    + " the BUYER shall pay their obligations under this Contract directly to the assignee. This assignment of rights and receivables shall be without prejudice to the execution of"
                    + " a deed of sale with real state mortgage on the UNIT which may immediately or thereafter be required by the SELLER or the assignee bank or"
                    + " financial institution for the purpose of securing the loan or financing package availed of for the payment of the balance of the CONTRACT PRICE of the BUYER to the SELLER,"
                    + " the BUYER hereby ratifying and confirming any and all acts of the SELLER in the execution of the power of attorney herein given.", updateFontArial12);

                Paragraph p48 = new Paragraph
                {
                    p48Phrase
                };

                p48.SetLeading(7f, 0);
                p48.Alignment = Element.ALIGN_JUSTIFIED;
                p48.FirstLineIndent = 40f;
                document.Add(p48);
                document.Add(spaceTable);

                Phrase p49Phrase = new Phrase("The BUYER may not assign, sell or transfer its rights under this contract, or any right or interest herein or in the UNIT, without prior written notice to and conformity of"
                    + " the SELLER. In case the SELLER approves the assignment, the BUYER shall pay the SELLER a transfer fee in the amount of P15,100.00  or such other amount as the SELLER may otherwise fix. However,"
                    + " he BUYER may, without securing a formal approval from the SELLER, assign its rights and interests under this Contract in favor of the assignee bank/financial institution (not applicable) to secure"
                    + " a loan which the BUYER may obtain from said bank to finance payment of the balance of the CONTRACT PRICE for the UNIT to the SELLER. Any such purchaser, assignee or transferee expressly binds himself"
                    + " to honor the terms and conditions of this Contract with respect to the rights and interest of the SELLER.", updateFontArial12);

                Paragraph p49 = new Paragraph
                {
                    p49Phrase
                };

                p49.SetLeading(7f, 0);
                p49.Alignment = Element.ALIGN_JUSTIFIED;
                p49.FirstLineIndent = 40f;
                document.Add(p49);
                document.Add(spaceTable);

                Phrase p50Phrase = new Phrase("12. Title and Ownership of the Unit", updateFontArial12BoldItalic);
                Paragraph p50 = new Paragraph
                {
                    p50Phrase
                };

                p50.SetLeading(7f, 0);
                document.Add(p50);
                document.Add(spaceTable);

                Phrase p51Phrase = new Phrase("The SELLER shall execute or cause the execution of a separate Deed of Absolute Sale and the issuance of the Certificate of Title to the Unit in favor of the BUYER,"
                    + " their successor and assigns, therby conveying to the BUYER, their successors and assign the title, rights and interests in the UNIT as soon as the following shall have been"
                    + " accomplished:", updateFontArial12);

                Paragraph p51 = new Paragraph
                {
                    p51Phrase
                };

                p51.SetLeading(7f, 0);
                p51.Alignment = Element.ALIGN_JUSTIFIED;
                p51.FirstLineIndent = 40f;
                document.Add(p51);
                document.Add(spaceTable);

                Phrase p52Phrase = new Phrase("(a) Payment in full of the CONTRACT PRICE and any all interests, penalties and other charges such as, but not limited to, telephone, cable, electric and water"
                    + " connections and deposits which may have accrued or which may have been advanced by the SELLER, including all other obligations of the BUYER under this Contract such as"
                    + " insurance premiums, cost of repairs, real state taxes advanced by the SELLER and bank charges or interests incidental to the BUYER’S loan or financial package;", updateFontArial12);

                Paragraph p52 = new Paragraph
                {
                    p52Phrase
                };

                p52.SetLeading(7f, 0);
                p52.Alignment = Element.ALIGN_JUSTIFIED;
                p52.IndentationLeft = 40f;
                p52.IndentationRight = 40f;
                document.Add(p52);
                document.Add(spaceTable);

                Phrase p53Phrase = new Phrase("(b) Issuance by the Registry of Deeds of the individual Certificate of Title covering the Unit in the name of the BUYER; and", updateFontArial12);

                Paragraph p53 = new Paragraph
                {
                    p53Phrase
                };

                p53.SetLeading(7f, 0);
                p53.Alignment = Element.ALIGN_JUSTIFIED;
                p53.IndentationLeft = 40f;
                p53.IndentationRight = 40f;
                document.Add(p53);
                document.Add(spaceTable);

                Phrase p54Phrase = new Phrase("(c) Payment of the membership fee to the Associaton, or to the SELLER if payment of such amount had been advanced by the SELLER, in such amount as shall be determined by the latter.", updateFontArial12);

                Paragraph p54 = new Paragraph
                {
                    p54Phrase
                };

                p54.SetLeading(7f, 0);
                p54.Alignment = Element.ALIGN_JUSTIFIED;
                p54.IndentationLeft = 40f;
                p54.IndentationRight = 40f;
                document.Add(p54);
                document.Add(spaceTable);

                Phrase p55Phrase = new Phrase("In the event that the Deed of Absolute Sale is executed prior to the BUYER’s settlement of association dues, electric and water deposits, and other advances/fees as may be imposed or incurred due to the BUYER’s financing requirements, the SELLER shall not deliver the UNIT, or the Certificate of Title therefor, until such time as all of the BUYER’S payables are settled in full.", updateFontArial12);

                Paragraph p55 = new Paragraph
                {
                    p55Phrase
                };

                p55.SetLeading(7f, 0);
                p55.Alignment = Element.ALIGN_JUSTIFIED;
                p55.FirstLineIndent = 40f;
                document.Add(p55);
                document.Add(spaceTable);

                Phrase p56Phrase = new Phrase("13. Warranties", updateFontArial12BoldItalic);
                Paragraph p56 = new Paragraph
                {
                    p56Phrase
                };

                p56.SetLeading(7f, 0);
                document.Add(p56);
                document.Add(spaceTable);

                Phrase p57Phrase = new Phrase("The SELLER warrants and guarantees(a) the authenticity and validity of the title to the UNIT subject of this Contact and undertakes to defend the same against"
                    + " all claims of any and all persons and entities; (b) that the title to the UNIT is free from liens and encumbrances, except for the mortgage, if any, referred"
                    + " to herein, those provided in the Declaration of Covenants, Conditions and Restrictions, those imposed by law, the Articles of Incorporation and By Laws of the"
                    + " Association, zoning regulations and other restrictions on the use and occupancy of the UNIT as may be imposed by government and other authorities having"
                    + " jurisdiction thereon, and to other restrictions and easements of record; and (c) that the UNIT is free from and clear of tenants, occupants and squatters and"
                    + " undertakes to hold the BUYER, their successor and assigns, free and harmless from any liability or responsibility with regard to any such tenants, occupants or"
                    + " squatters, or their eviction from the UNIT.", updateFontArial12);

                Paragraph p57 = new Paragraph
                {
                    p57Phrase
                };

                p57.SetLeading(7f, 0);
                p57.Alignment = Element.ALIGN_JUSTIFIED;
                p57.FirstLineIndent = 40f;
                document.Add(p57);
                document.Add(spaceTable);

                Phrase p58Phrase = new Phrase("14. Completion of Construction of the Unit", updateFontArial12BoldItalic);
                Paragraph p58 = new Paragraph
                {
                    p58Phrase
                };

                p58.SetLeading(7f, 0);
                document.Add(p58);
                document.Add(spaceTable);

                Phrase p59Phrase = new Phrase("The SELLER projects, without any warranty or covenant, the completion of construction of the UNIT and the subdivision project within the timetable allowed by HLURB, and/or other competent authority, unless prevented by “force majeure”.", updateFontArial12);

                Paragraph p59 = new Paragraph
                {
                    p59Phrase
                };

                p59.SetLeading(7f, 0);
                p59.Alignment = Element.ALIGN_JUSTIFIED;
                p59.FirstLineIndent = 40f;
                document.Add(p59);
                document.Add(spaceTable);

                Phrase p60Phrase = new Phrase("The term “force majeure” as used herein refers to any condition, event, cause or reason beyond the control of the SELLER, including but not limited to, any act of God, strikes, lockouts or other"
                    + " industrial disturbances, serious civil disturbances, unavoidable accidents, blow out, acts of the public enemy, war ,blockade, public riot, fire, flood, explosion, governmental or municipal restraint, court or"
                    + " administrative injunctions or other court or administrative orders stopping or interfering with the work progress, shortage or unavailability of equipment, materials or labor, restrictions or limitations upon the"
                    + " user thereof and/or acts of third person/s.", updateFontArial12);

                Paragraph p60 = new Paragraph
                {
                    p60Phrase
                };

                p60.SetLeading(7f, 0);
                p60.Alignment = Element.ALIGN_JUSTIFIED;
                p60.FirstLineIndent = 40f;
                document.Add(p60);
                document.Add(spaceTable);

                Phrase p61Phrase = new Phrase("Should the SELLER be delayed in the construction or completion of the UNIT or the subdivision project due to force majeure, the SELLER shall"
                    + " be entitled to such additional period of time sufficient to enable it to complete the construction of the same as shall correspond to the period of delay due"
                    + " to such cause. Should any condition or, cause beyond the control of the SELLER arise which renders the completion of the UNIT or the subdivision project no"
                    + " longer possible, the SELLER shall be relieved of any obligation arising out of this Contract, except to reimburse the BUYER whatever it may have received from"
                    + " them under and by virtue of this Contract, without interest in any event at all.", updateFontArial12);

                Paragraph p61 = new Paragraph
                {
                    p61Phrase
                };

                p61.SetLeading(7f, 0);
                p61.Alignment = Element.ALIGN_JUSTIFIED;
                p61.FirstLineIndent = 40f;
                document.Add(p61);
                document.Add(spaceTable);

                Phrase p62Phrase = new Phrase("The BUYER expressly agrees and accepts that the failure of the SELLER to complete the UNIT or the subdivision project within the period specified above due to any force majeure shall"
                    + " not be a ground to rescind or cancel this Contract and the SELLER have no liability whatsoever to the BUYER for such non completion, except as provided in the immediately preceding paragraph and"
                    + " Section 23 of Presidentail Decreee(PD) No. 967.", updateFontArial12);

                Paragraph p62 = new Paragraph
                {
                    p62Phrase
                };

                p62.SetLeading(7f, 0);
                p62.Alignment = Element.ALIGN_JUSTIFIED;
                p62.FirstLineIndent = 40f;
                document.Add(p62);
                document.Add(spaceTable);

                Phrase p63Phrase = new Phrase("The SELLER may not be compelled to complete the construction of the UNIT prior to the BUYER’s full settlement of the downpayment and"
                    + " any additional amounts due relative thereto, and the delivery of the postdated check to cover the BUYER’s monthly amortization payments.", updateFontArial12);

                Paragraph p63 = new Paragraph
                {
                    p63Phrase
                };

                p63.SetLeading(7f, 0);
                p63.Alignment = Element.ALIGN_JUSTIFIED;
                p63.FirstLineIndent = 40f;
                document.Add(p63);
                document.Add(spaceTable);

                Phrase p64Phrase = new Phrase("15. Delivery of the Unit", updateFontArial12BoldItalic);
                Paragraph p64 = new Paragraph
                {
                    p64Phrase
                };

                p64.SetLeading(7f, 0);
                document.Add(p64);
                document.Add(spaceTable);

                Phrase p65Phrase = new Phrase("The possession of the Unit shall be delivered by the SELLER to the BUYER within reasonable period of time from the"
                    + " date of completion of construction of such UNIT and its related facilities. It is understood, however, that physical possession of the PROPERTY shall not"
                    + " be delivered by the SELLER to the BUYER unless the latter shall have complied with all conditions and requirements prescribed for this purpose by the"
                    + " SELLER to the BUYER unless the latter shall have complied with all conditions and requirements prescribed for this purpose by the SELLER under its"
                    + " policies prevailing at the time.", updateFontArial12);

                Paragraph p65 = new Paragraph
                {
                    p65Phrase
                };

                p65.SetLeading(7f, 0);
                p65.Alignment = Element.ALIGN_JUSTIFIED;
                p65.FirstLineIndent = 40f;
                document.Add(p65);
                document.Add(spaceTable);

                Phrase p66Phrase = new Phrase("Upon completion of the UNIT, the SELLER shall serve in the BUYER a written notice of turn over stating the date on which the UNIT shall be ready"
                    + " for delivery or occupancy by the BUYER. If the BUYER is not in default, the possession of the UNIT shall be delivered to them. The BUYER shall be given a"
                    + " reasonable opportunity to inspect and examine the UNIT before acceptance of the same. Provided however, that if no inspection is made on or before the date or"
                    + " within the period stated in the notice, the UNIT shall be deemed to have already been inspected by the BUYER and the same shall be considered as to have been"
                    + " completed and delivered in the date specified in the Notice.", updateFontArial12);

                Paragraph p66 = new Paragraph
                {
                    p66Phrase
                };

                p66.SetLeading(7f, 0);
                p66.Alignment = Element.ALIGN_JUSTIFIED;
                p66.FirstLineIndent = 40f;
                document.Add(p66);
                document.Add(spaceTable);

                Phrase p67Phrase = new Phrase("Within the prescribed period for inspection prior to the turnover of the UNIT to the BUYER, the BUYER shall register with the SELLER their written"
                    + " complaint on any defect. Failure to so register such complaint shall be deemed an unqualified and unconditional acceptance of the UNIT by the BUYER and shall constitute"
                    + " a bar for future complaint or action on the same.", updateFontArial12);

                Paragraph p67 = new Paragraph
                {
                    p67Phrase
                };

                p67.SetLeading(7f, 0);
                p67.Alignment = Element.ALIGN_JUSTIFIED;
                p67.FirstLineIndent = 40f;
                document.Add(p67);
                document.Add(spaceTable);

                Phrase p68Phrase = new Phrase("The BUYER shall be deemed to have taken possession of the UNIT in any of the following or analogous circumstances:"
                    + " (1) on the date specified in the SELLER’s notice of turnover  and upon the BUYER’s actual or constructive receipt thereof irrespective of their"
                    + " non-occupancy of the UNIT for any reason whatsoever; (2) when the BUYER actually occupies the UNIT; (3) when the BUYER commences to introduce"
                    + " improvements, alterations, furnishing, etc. on the UNIT; (4) when the BUYER takes or receives the keys to the UNIT;"
                    + " (5) when the BUYER accepts the UNIT or when the UNIT is deemed accepted as provided herein,", updateFontArial12);

                Paragraph p68 = new Paragraph
                {
                    p68Phrase
                };

                p68.SetLeading(7f, 0);
                p68.Alignment = Element.ALIGN_JUSTIFIED;
                p68.FirstLineIndent = 40f;
                document.Add(p68);
                document.Add(spaceTable);

                Phrase p69Phrase = new Phrase("From and after the date specified in notice of turnover, or when the BUYER takes possession of the UNIT in accordance with the immediately"
                    + " preceding paragraph, notwithstanding title to the UNIT had not been transferred to the BUYER, the BUYER shall, in place of the SELLER, observe all the"
                    + " conditions and restrictions on the UNIT and shall henceforth be liable for all risk of loss or damage to the UNIT, charges and fees for utilities and service,"
                    + " taxes and homeowners’ association dues, and other related obligations and assessments pertaining to the UNIT.", updateFontArial12);

                Paragraph p69 = new Paragraph
                {
                    p69Phrase
                };

                p69.SetLeading(7f, 0);
                p69.Alignment = Element.ALIGN_JUSTIFIED;
                p69.FirstLineIndent = 40f;
                document.Add(p69);
                document.Add(spaceTable);

                Phrase p70Phrase = new Phrase("The BUYER shall, before moving into the UNIT. Pay membership and other dues assessed on the UNIT by the Homeowners’"
                    + " Association to be established in the subdivision project.", updateFontArial12);

                Paragraph p70 = new Paragraph
                {
                    p70Phrase
                };

                p70.SetLeading(7f, 0);
                p70.Alignment = Element.ALIGN_JUSTIFIED;
                p70.FirstLineIndent = 40f;
                document.Add(p70);
                document.Add(spaceTable);

                Phrase p71Phrase = new Phrase("Upon moving in, the BUYER shall pay move-in fees covering the determined cost incurred by the SELLER for pedestal,"
                    + " electrical connection and water connection of the HOUSE and LOT.", updateFontArial12);

                Paragraph p71 = new Paragraph
                {
                    p71Phrase
                };

                p71.SetLeading(7f, 0);
                p71.Alignment = Element.ALIGN_JUSTIFIED;
                p71.FirstLineIndent = 40f;
                document.Add(p71);
                document.Add(spaceTable);

                Phrase p72Phrase = new Phrase("16. Insurance", updateFontArial12BoldItalic);
                Paragraph p72 = new Paragraph
                {
                    p72Phrase
                };

                p72.SetLeading(7f, 0);
                document.Add(p72);
                document.Add(spaceTable);

                Phrase p73Phrase = new Phrase("The BUYER shall obtain and maintain the following insurance until the BUYER has fully paid the Contrast Price and its related"
                    + " charges to the SELLER, with the SELLER or its assignee as the designated beneficiary:", updateFontArial12);

                Paragraph p73 = new Paragraph
                {
                    p73Phrase
                };

                p73.SetLeading(7f, 0);
                p73.Alignment = Element.ALIGN_JUSTIFIED;
                p73.FirstLineIndent = 40f;
                document.Add(p73);
                document.Add(spaceTable);

                Phrase p74Phrase = new Phrase("(a) Redemption Insurance – This Insurance, which cover risk in case of death of the BUYER, is subject to the Schedule of Insurance"
                    + " in the SELLER’s Master Policy.", updateFontArial12);

                Paragraph p74 = new Paragraph
                {
                    p74Phrase
                };

                p74.SetLeading(7f, 0);
                p74.Alignment = Element.ALIGN_JUSTIFIED;
                p74.IndentationLeft = 40f;
                p74.IndentationRight = 40f;
                document.Add(p74);
                document.Add(spaceTable);

                Phrase p75Phrase = new Phrase("(b) Fire Insurance – The buyer shall obtain fire as well as allied peril insurance/s on the UNIT for an amount equivalent to at least"
                    + " the contract value of the residential unit and/or its improvements. The premiums for this coverage shall be prepared annually by the BUYER."
                    + " The initial year’s prepayment shall, be deducted from, the Contrast proceeds, while the repayments for the succeeding years shall be collected"
                    + " together with the BUYER’s monthly installment payments.", updateFontArial12);

                Paragraph p75 = new Paragraph
                {
                    p75Phrase
                };

                p75.SetLeading(7f, 0);
                p75.Alignment = Element.ALIGN_JUSTIFIED;
                p75.IndentationLeft = 40f;
                p75.IndentationRight = 40f;
                document.Add(p75);
                document.Add(spaceTable);

                Phrase p76Phrase = new Phrase("(c) Other insurances as may be required for purposes of the BUYER’s housing loan.", updateFontArial12);

                Paragraph p76 = new Paragraph
                {
                    p76Phrase
                };

                p76.SetLeading(7f, 0);
                p76.Alignment = Element.ALIGN_JUSTIFIED;
                p76.IndentationLeft = 40f;
                p76.IndentationRight = 40f;
                document.Add(p76);
                document.Add(spaceTable);

                Phrase p77Phrase = new Phrase("17. Miscellaneous Provisions", updateFontArial12BoldItalic);
                Paragraph p77 = new Paragraph
                {
                    p77Phrase
                };

                p77.SetLeading(7f, 0);
                document.Add(p77);
                document.Add(spaceTable);

                Phrase p78Phrase = new Phrase("(a) The BUYER warrants in full the truth of the representations made in the applications for the purchase of the UNIT subject of this Contract,"
                    + " and any falsehood or misrepresentation stated therein shall be sufficient ground for the cancellation or rescission of this Contract.", updateFontArial12);

                Paragraph p78 = new Paragraph
                {
                    p78Phrase
                };

                p78.SetLeading(7f, 0);
                p78.Alignment = Element.ALIGN_JUSTIFIED;
                p78.IndentationLeft = 40f;
                document.Add(p78);
                document.Add(spaceTable);

                Phrase p79Phrase = new Phrase("(b) The BUYER shall notify the SELLER in writing of any change in their mailing address. Should the BUYER fails to do so, their address stated in the Contract shall remain their address for all intents and purposes.", updateFontArial12);

                Paragraph p79 = new Paragraph
                {
                    p79Phrase
                };

                p79.SetLeading(7f, 0);
                p79.Alignment = Element.ALIGN_JUSTIFIED;
                p79.IndentationLeft = 40f;
                document.Add(p79);
                document.Add(spaceTable);

                Phrase p80Phrase = new Phrase("(c) Discrepancy of less than ten percent (10%) in the approximate gross area of the UNIT as stated in the Contract, in brochures or price list than the actual area of the UNIT when completed, shall not result in an increase or decrease in the selling price.", updateFontArial12);

                Paragraph p80 = new Paragraph
                {
                    p80Phrase
                };

                p80.SetLeading(7f, 0);
                p80.Alignment = Element.ALIGN_JUSTIFIED;
                p80.IndentationLeft = 40f;
                document.Add(p80);
                document.Add(spaceTable);

                Phrase p81Phrase = new Phrase("(d) The SELLER reserves the right to construct other improvements on available, unutilized or vacant land or space surrounding or adjacent to the UNIT and hereby reserves its ownership thereof.", updateFontArial12);

                Paragraph p81 = new Paragraph
                {
                    p81Phrase
                };

                p81.SetLeading(7f, 0);
                p81.Alignment = Element.ALIGN_JUSTIFIED;
                p81.IndentationLeft = 40f;
                document.Add(p81);
                document.Add(spaceTable);

                Phrase p82Phrase = new Phrase("(d1) The SELLER may upgrade/downgrade/revise house specification as part of the exercise of its right pursuant to this Contract being developer.", updateFontArial12);

                Paragraph p82 = new Paragraph
                {
                    p82Phrase
                };

                p82.SetLeading(7f, 0);
                p82.Alignment = Element.ALIGN_JUSTIFIED;
                p82.IndentationLeft = 140f;
                document.Add(p82);
                document.Add(spaceTable);

                Phrase p83Phrase = new Phrase("(e) In the event that the subdivision project and UNIT becomes not economically feasible such that there are adverse conditions, changes and its structure,"
                    + " or other similar factors or reasons, the SELLER may, upon written notice to the BUYER, change or alter the design, specifications and/or the price of the UNIT or replace"
                    + " the same with a similar lot, or cancel this Contract and return in full, without interest, all payments received from the BUYER.", updateFontArial12);

                Paragraph p83 = new Paragraph
                {
                    p83Phrase
                };

                p83.SetLeading(7f, 0);
                p83.Alignment = Element.ALIGN_JUSTIFIED;
                p83.IndentationLeft = 40f;
                document.Add(p83);
                document.Add(spaceTable);

                Phrase p84Phrase = new Phrase("(f) If the sale of the UNIT hereunder constitutes “bulk buying” subject to the provisions of HLURB Administrative Order NO. 09, Series"
                    + " of 1994, or the HLURB Rules and Regulations on Bulk Buying, the BUYER hereby agrees and undertakes to comply  with the provisions of the aforesaid"
                    + " Administrative Order.", updateFontArial12);

                Paragraph p84 = new Paragraph
                {
                    p84Phrase
                };

                p84.SetLeading(7f, 0);
                p84.Alignment = Element.ALIGN_JUSTIFIED;
                p84.IndentationLeft = 40f;
                document.Add(p84);
                document.Add(spaceTable);

                Phrase p85Phrase = new Phrase("(g) The BUYER agrees to be bound by all terms and conditions on the Declaration of Restrictions for the Subdivision Project and the Articles of"
                    + " Incorporation and By Laws of the homeowners association, copies of which shall be duly finished upon request of the BUYER. The BUYER further confirms that his obligations"
                    + " under this Contract will survive upon payment of the CONTRACT PRICE and the execution of the Deed of Absolute Sale.", updateFontArial12);

                Paragraph p85 = new Paragraph
                {
                    p85Phrase
                };

                p85.SetLeading(7f, 0);
                p85.Alignment = Element.ALIGN_JUSTIFIED;
                p85.IndentationLeft = 40f;
                document.Add(p85);
                document.Add(spaceTable);

                Phrase p86Phrase = new Phrase("(h) Any reference to any party to this Contract includes such party’s successor and assigns.", updateFontArial12);

                Paragraph p86 = new Paragraph
                {
                    p86Phrase
                };

                p86.SetLeading(7f, 0);
                p86.Alignment = Element.ALIGN_JUSTIFIED;
                p86.IndentationLeft = 40f;
                document.Add(p86);
                document.Add(spaceTable);

                Phrase p87Phrase = new Phrase("18. Venue", updateFontArial12BoldItalic);
                Paragraph p87 = new Paragraph
                {
                    p87Phrase
                };

                p87.SetLeading(7f, 0);
                document.Add(p87);
                document.Add(spaceTable);

                Phrase p88Phrase = new Phrase("Should the SELLER be constrained to resort to courts to project its rights or to seek redress for its grievances under this Contract,"
                    + " or to defend itself against any action or proceeding instituted by the BUYER or any other party arising from this Contract or any related document,"
                    + " the BUYER shall further pay the SELLER, as and by way of attorney’s fees, a sum equivalent to at least twenty percent (20%) of the total amount"
                    + " due or involved , or the amount of fifty thousand pesos (P50,000.00) whichever is higher, in addition to the cost and expenses of litigation, and"
                    + " to the actual and other damages provided hereinabove to which the SELLER shall be entitled  by law and under this Contract. Any actions or"
                    + " proceedings related to this Contract shall be brought before proper courts of Cebu City, all other venues being expressly waived.", updateFontArial12);

                Paragraph p88 = new Paragraph
                {
                    p88Phrase
                };

                p88.SetLeading(7f, 0);
                p88.Alignment = Element.ALIGN_JUSTIFIED;
                p88.FirstLineIndent = 40f;
                document.Add(p88);
                document.Add(spaceTable);

                Phrase p89Phrase = new Phrase("19. Separability Clause", updateFontArial12BoldItalic);
                Paragraph p89 = new Paragraph
                {
                    p89Phrase
                };

                p89.SetLeading(7f, 0);
                document.Add(p89);
                document.Add(spaceTable);

                Phrase p90Phrase = new Phrase("In case one or more of the provisions contained in this Contract to Sell shall be declared invalid, illegal or unenforceable in any"
                    + " respect by a competent authority, the validity, legality, and enforceability of the remaining provisions contained herein shall not in any way be"
                    + " affected or impaired thereby.", updateFontArial12);

                Paragraph p90 = new Paragraph
                {
                    p90Phrase
                };

                p90.SetLeading(7f, 0);
                p90.Alignment = Element.ALIGN_JUSTIFIED;
                p90.FirstLineIndent = 40f;
                document.Add(p90);
                document.Add(spaceTable);

                Phrase p91Phrase = new Phrase("20. Repealing Clause", updateFontArial12BoldItalic);
                Paragraph p91 = new Paragraph
                {
                    p91Phrase
                };

                p91.SetLeading(7f, 0);
                document.Add(p91);
                document.Add(spaceTable);

                Phrase p92Phrase = new Phrase("This Contract cancels and supersedes all previous  Contracts between tha parties herein and this Contract shall not be considered as changed, modified,"
                    + " altered or in any manner amended by acts of tolerance of the SELLER unless such changes, modifications, alterations or amendments are made in writing and signed by"
                    + " both parties to this contract.", updateFontArial12);

                Paragraph p92 = new Paragraph
                {
                    p92Phrase
                };

                p92.SetLeading(7f, 0);
                p92.Alignment = Element.ALIGN_JUSTIFIED;
                p92.FirstLineIndent = 40f;
                document.Add(p92);
                document.Add(spaceTable);

                Phrase p93Phrase = new Phrase("21. ", updateFontArial12BoldItalic);

                Phrase p93Phrase2 = new Phrase("The BUYER hereby represent/s that (i) this Contract has been read, understood and accepted by them; (ii) the obligations of the BUYER hereunder and"
                    + " under the Deed of Absolute Sale, including their compliance with the Declaration of Covenants, Conditions and Restrictions constitutes legal, valid and binding obligations,"
                    + " fully enforceable against them; and (iii) the BUYER has full power, authority and legal right to execute, deliver and perform this Contract and the Deed of Sale.", updateFontArial12);

                Paragraph p93 = new Paragraph
                {
                    p93Phrase, p93Phrase2
                };

                p93.SetLeading(7f, 0);
                p93.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(p93);
                document.Add(spaceTable);

                Phrase p94Phrase = new Phrase("IN WITNESS WHEREOF, The parties hereto signed this instrument on the date and the place hereinbefore mentioned.", updateFontArial12);

                Paragraph p94 = new Paragraph
                {
                    p94Phrase
                };

                p94.SetLeading(7f, 0);
                p94.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(p94);
                document.Add(spaceTable);
                document.Add(spaceTable);

                if (sysSettings.Any())
                {
                    Phrase p95Phrase = new Phrase(sysSettings.FirstOrDefault().Company, updateFontArial12Bold);

                    Paragraph p95 = new Paragraph
                    {
                        p95Phrase
                    };

                    p95.SetLeading(7f, 0);
                    p95.Alignment = Element.ALIGN_JUSTIFIED;
                    document.Add(p95);
                    document.Add(spaceTable);
                }

                PdfPTable pdfTableSignatureSignatures = new PdfPTable(5);
                pdfTableSignatureSignatures.SetWidths(new float[] { 100f, 15f, 100f, 15f, 100f });
                pdfTableSignatureSignatures.WidthPercentage = 100;
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase("Seller", updateFontArial12)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase("Buyer/s", updateFontArial12)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });

                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase("Represented by:", updateFontArial12)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Border = 0 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Border = 0 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Border = 0 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Border = 0 });

                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12Bold)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 60f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Border = 0 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });

                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 1 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 1 });

                //pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial12)) { Border = 0, PaddingTop = 30f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                //pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial12)) { Border = 0 });
                //pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(Spouse, fontArial12Bold)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 30f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                //pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial12)) { Border = 0 });
                //pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial12)) { Border = 0 });
                //pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial12)) { Border = 0 });
                //pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial12)) { Border = 1 });
                //pdfTableSignatureSignatures.AddCell(new PdfPCell(new Phrase(" ", fontArial12)) { Border = 0 });
                document.Add(pdfTableSignatureSignatures);

                document.Add(spaceTable);
                document.Add(spaceTable);

                Phrase p96Phrase = new Phrase("Signed in the presence of: ", updateFontArial12);

                Paragraph p96 = new Paragraph
                {
                    p96Phrase
                };

                p96.SetLeading(7f, 0);
                p96.Alignment = Element.ALIGN_CENTER;
                document.Add(p96);
                document.Add(spaceTable);

                PdfPTable pdfTableWitnessesSignatures = new PdfPTable(3);
                pdfTableWitnessesSignatures.SetWidths(new float[] { 100f, 50f, 100f });
                pdfTableWitnessesSignatures.WidthPercentage = 100;
                pdfTableWitnessesSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0, PaddingTop = 15f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableWitnessesSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableWitnessesSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0, PaddingTop = 15f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableWitnessesSignatures.AddCell(new PdfPCell(new Phrase("Witness", updateFontArial12)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableWitnessesSignatures.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableWitnessesSignatures.AddCell(new PdfPCell(new Phrase("Witness", updateFontArial12)) { HorizontalAlignment = 1, Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(pdfTableWitnessesSignatures);

                document.Add(spaceTable);
                document.Add(spaceTable);

                Phrase p97Phrase = new Phrase("ACKNOWLEDGEMENT", updateFontArial12Bold);

                Paragraph p97 = new Paragraph
                {
                    p97Phrase
                };

                p97.SetLeading(7f, 0);
                p97.Alignment = Element.ALIGN_CENTER;
                document.Add(p97);
                document.Add(spaceTable);

                Phrase p98Phrase = new Phrase("REPUBLIC OF THE PHILIPPINES \nCITY OF CEBU", updateFontArial12);

                Paragraph p98 = new Paragraph
                {
                    p98Phrase
                };

                p98.SetLeading(7f, 0);
                p98.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(p98);
                document.Add(spaceTable);

                Phrase p99Phrase = new Phrase("BEFORE ME a Notary Public for and in the above jurisdiction, this ________ day of __________________, personally appeared the following:", updateFontArial12);

                Paragraph p99 = new Paragraph
                {
                    p99Phrase
                };

                p99.SetLeading(7f, 0);
                p99.Alignment = Element.ALIGN_JUSTIFIED;
                p99.FirstLineIndent = 40f;
                document.Add(p99);
                document.Add(spaceTable);

                PdfPTable pdfTableIdentification = new PdfPTable(5);
                pdfTableIdentification.SetWidths(new float[] { 100f, 20f, 100f, 20f, 100f });
                pdfTableIdentification.WidthPercentage = 100;
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase("NAME", updateFontArial12Bold)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase("IDENTIFICATION", updateFontArial12Bold)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase("ISSUED BY", updateFontArial12Bold)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });

                if (sysSettings.Any())
                {
                    pdfTableIdentification.AddCell(new PdfPCell(new Phrase(sysSettings.FirstOrDefault().Company, updateFontArial12)) { Border = 0, PaddingTop = 10f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                    pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                    pdfTableIdentification.AddCell(new PdfPCell(new Phrase("TIN: ", updateFontArial12)) { Border = 0, PaddingTop = 10f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                    pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                    pdfTableIdentification.AddCell(new PdfPCell(new Phrase("Bureau of Internal Revenue", updateFontArial12)) { Border = 0, PaddingTop = 10f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                }

                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(Customer, updateFontArial12)) { Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().MstCustomer.IdType + ": " + soldUnit.FirstOrDefault().MstCustomer.IdNumber, updateFontArial12)) { Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(soldUnit.FirstOrDefault().MstCustomer.IdType + " Agency", updateFontArial12)) { Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                //pdfTableIdentification.AddCell(new PdfPCell(new Phrase("Bureau of Internal Revenue", fontArial12)) { Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(Spouse, updateFontArial12)) { Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase("TIN: " + soldUnit.FirstOrDefault().MstCustomer.SpouseTIN, updateFontArial12)) { Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase("Bureau of Internal Revenue", updateFontArial12)) { Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0 });
                pdfTableIdentification.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 1, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(pdfTableIdentification);

                document.Add(spaceTable);

                Phrase p100Phrase = new Phrase("All known to me and identified through the competent evidence of identity hereinabove describe to be the same persons who executed"
                    + " the foregoing deed and acknowledge that the same is their own and free and voluntary act, deed, their authority and that of the corporation herein represented.", updateFontArial12);

                Paragraph p100 = new Paragraph
                {
                    p100Phrase
                };

                p100.SetLeading(7f, 0);
                p100.Alignment = Element.ALIGN_JUSTIFIED;
                p100.FirstLineIndent = 40f;
                document.Add(p100);
                document.Add(spaceTable);

                Phrase p101Phrase = new Phrase("This instrument refers to a Contract to Sell consisting of five (5) pages, and Annex “A”, signed by the parties and their instrumental"
                    + " witnesses at the end of the body of the documents and on the left hand margin of the reserve side hereof and the Annex, each and every page"
                    + " of which is sealed with my notarial seal.", updateFontArial12);

                Paragraph p101 = new Paragraph
                {
                    p101Phrase
                };

                p101.SetLeading(7f, 0);
                p101.Alignment = Element.ALIGN_JUSTIFIED;
                p101.FirstLineIndent = 40f;
                document.Add(p101);
                document.Add(spaceTable);

                Phrase p102Phrase = new Phrase("WITNESS MY HAND AND NOTARIAL SEAL on the date and at the place first hereinabove written.", updateFontArial12);

                Paragraph p102 = new Paragraph
                {
                    p102Phrase
                };

                p102.SetLeading(7f, 0);
                p102.Alignment = Element.ALIGN_JUSTIFIED;
                p102.IndentationLeft = 80f;
                document.Add(p102);
                document.Add(spaceTable);

                Phrase p103Phrase = new Phrase(
                    "Doc.No.      \t _______________  ; \n" +
                    "Page No.    \t _______________  ; \n" +
                    "Book No.    \t _______________  ; \n" +
                    "Series No.  \t _______________  ; \n", updateFontArial12);

                Paragraph p103 = new Paragraph
                {
                    p103Phrase
                };

                p103.SetLeading(7f, 0);
                p103.Alignment = Element.ALIGN_JUSTIFIED;
                p103.IndentationLeft = 80f;
                document.Add(p103);
                document.Add(spaceTable);
            }

            // ==============
            // Close Document
            // ==============
            document.Close();

            // ==============
            // Reponse Stream
            // ==============
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

        // ========================================
        // PDF Sold Unit Equipment Payment Schedule
        // ========================================
        private PdfPCell PhraseCell(Phrase phrase, int align, int border)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = align; //0=Left, 1=Centre, 2=Right
            cell.BorderWidth = border;
            return cell;
        }

        [HttpGet, Route("SoldUnitEquitySchedule/{id}")]
        public HttpResponseMessage PdfSoldUnitEquitySchedule(int id)
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

            // =====================================
            // Get Sold Unit Equity Payment Schedule
            // =====================================
            var soldUnitEquitySchedules = from d in db.TrnSoldUnitEquitySchedules
                                          orderby d.PaymentDate ascending
                                          where d.SoldUnitId == Convert.ToInt32(id)
                                          select d;

            if (soldUnitEquitySchedules.Any())
            {
                var settings = from d in db.SysSettings select d;
                string company = "";
                if (settings.Any())
                {
                    company = settings.FirstOrDefault().Company;
                }

                PdfPTable companyTable = new PdfPTable(2);
                companyTable.SetWidths(new float[] { 100f, 100f });
                companyTable.WidthPercentage = 100;
                companyTable.AddCell(new PdfPCell(new Phrase(company, fontArial17Bold)) { Border = 0 });
                companyTable.AddCell(new PdfPCell(new Phrase("Equity Payment Schedule", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });

                document.Add(companyTable);

                document.Add(line);

                document.Add(spaceTable);

                PdfPTable soldUnitTable = new PdfPTable(2);
                soldUnitTable.SetWidths(new float[] { 80f, 460f });
                soldUnitTable.WidthPercentage = 100;

                string soldUnitNumber = soldUnitEquitySchedules.FirstOrDefault().TrnSoldUnit.SoldUnitNumber;
                string customer = soldUnitEquitySchedules.FirstOrDefault().TrnSoldUnit.MstCustomer.LastName + ", " + soldUnitEquitySchedules.FirstOrDefault().TrnSoldUnit.MstCustomer.FirstName;
                string project = soldUnitEquitySchedules.FirstOrDefault().TrnSoldUnit.MstProject.Project;
                string unit = soldUnitEquitySchedules.FirstOrDefault().TrnSoldUnit.MstUnit.UnitCode;

                soldUnitTable.AddCell(PhraseCell(new Phrase("Sold Unit Number:", fontArial11Bold), 0, 0));
                soldUnitTable.AddCell(PhraseCell(new Phrase(soldUnitNumber, fontArial11Bold), 0, 0));
                soldUnitTable.AddCell(PhraseCell(new Phrase("Customer:", fontArial11Bold), 0, 0));
                soldUnitTable.AddCell(PhraseCell(new Phrase(customer, fontArial11Bold), 0, 0));
                soldUnitTable.AddCell(PhraseCell(new Phrase("Project:", fontArial11Bold), 0, 0));
                soldUnitTable.AddCell(PhraseCell(new Phrase(project, fontArial11Bold), 0, 0));
                soldUnitTable.AddCell(PhraseCell(new Phrase("Unit:", fontArial11Bold), 0, 0));
                soldUnitTable.AddCell(PhraseCell(new Phrase(unit, fontArial11Bold), 0, 0));

                document.Add(soldUnitTable);

                document.Add(spaceTable);
                document.Add(spaceTable);

                PdfPTable scheduleTable = new PdfPTable(6);
                scheduleTable.SetWidths(new float[] { 60f, 60f, 60f, 60f, 150f, 150f });
                scheduleTable.WidthPercentage = 100;

                scheduleTable.AddCell(PhraseCell(new Phrase("Payment Date", fontArial11Bold), 1, 1));
                scheduleTable.AddCell(PhraseCell(new Phrase("Amortization", fontArial11Bold), 1, 1));
                scheduleTable.AddCell(PhraseCell(new Phrase("Check Number", fontArial11Bold), 1, 1));
                scheduleTable.AddCell(PhraseCell(new Phrase("Check Date", fontArial11Bold), 1, 1));
                scheduleTable.AddCell(PhraseCell(new Phrase("Check Bank", fontArial11Bold), 1, 1));
                scheduleTable.AddCell(PhraseCell(new Phrase("Remarks", fontArial11Bold), 1, 1));

                foreach (var schedule in soldUnitEquitySchedules)
                {
                    String paymentDate = schedule.PaymentDate.ToShortDateString();
                    String amortization = schedule.Amortization.ToString("0.00");
                    String checkNumber = schedule.CheckNumber;
                    String checkDate = schedule.CheckDate == null ? "" : schedule.CheckDate.Value.ToShortDateString();
                    String checkBank = schedule.CheckBank;
                    String remarks = schedule.Remarks;

                    scheduleTable.AddCell(PhraseCell(new Phrase(paymentDate, fontArial11Bold), 2, 1));
                    scheduleTable.AddCell(PhraseCell(new Phrase(amortization, fontArial11Bold), 2, 1));
                    scheduleTable.AddCell(PhraseCell(new Phrase(checkNumber, fontArial11Bold), 0, 1));
                    scheduleTable.AddCell(PhraseCell(new Phrase(checkDate, fontArial11Bold), 2, 1));
                    scheduleTable.AddCell(PhraseCell(new Phrase(checkBank, fontArial11Bold), 0, 1));
                    scheduleTable.AddCell(PhraseCell(new Phrase(remarks, fontArial11Bold), 0, 1));
                }

                document.Add(scheduleTable);

                document.Add(spaceTable);
                document.Add(spaceTable);
                document.Add(spaceTable);

                // ===========
                // Prepared By
                // ===========

                String PreparedByName = soldUnitEquitySchedules.FirstOrDefault().TrnSoldUnit.MstUser2.FullName;

                PdfPTable preparedByTable = new PdfPTable(5);
                preparedByTable.SetWidths(new float[] { 80f, 150f, 80f, 80f, 150f });
                preparedByTable.WidthPercentage = 100;

                preparedByTable.AddCell(new PdfPCell(new Phrase("Prepared by: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                preparedByTable.AddCell(new PdfPCell(new Phrase(PreparedByName, fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                preparedByTable.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                preparedByTable.AddCell(new PdfPCell(new Phrase("Received by: ", fontArial11Bold)) { Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });
                preparedByTable.AddCell(new PdfPCell(new Phrase("", fontArial11)) { HorizontalAlignment = 1, Border = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, PaddingRight = 5f });

                document.Add(preparedByTable);
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

        [HttpGet, Route("BuyersUndertaking/{id}")]
        public HttpResponseMessage BuyersUndertaking(Int32 id)
        {
            Font updateFontArial10Bold = FontFactory.GetFont("Arial", 7, Font.BOLD);
            Font updateFontArialBold = FontFactory.GetFont("Arial", 14, Font.BOLD);
            Font updateFontArial17Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font updateFontArial11Bold = FontFactory.GetFont("Arial", 7, Font.BOLD);

            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.SetMargins(30f, 30f, 30f, 30f);

            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 5f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 80;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", updateFontArial10Bold)) { PaddingTop = 1f, Border = 0 });
            // =============
            // Open Document
            // =============
            document.Open();

            var projectLogo = from d in db.MstProjects
                              select d;

            Image logo = Image.GetInstance(projectLogo.FirstOrDefault().ProjectLogo);
            logo.ScaleToFit(1000f, 60f);

            PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
            pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
            pdfTableCompanyDetail.WidthPercentage = 100;
            pdfTableCompanyDetail.AddCell(new PdfPCell(logo) { Border = 0 });
            pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("BUYER'S UNDERTAKING \n Revised 09.03.19", updateFontArial17Bold)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 2 });
            document.Add(pdfTableCompanyDetail);
            document.Add(line);

            //Phrase headerPhraseLabel = new Phrase("BUYER'S UNDERTAKING \n");
            //Phrase headerDatailPhraseData = new Phrase("Revised 09.03.19");
            //Paragraph paragraph1 = new Paragraph
            //    {
            //        headerPhraseLabel, headerDatailPhraseData
            //    };
            //document.Add(paragraph1);
            document.Add(spaceTable);

            Phrase paragraph2Phrase = new Phrase("WHEREAS, on __________________________________ the undersigned applied to purchase from Greentech Development Corporation, \n" +
                "a parcel of land / house and lot, particularly described in the Reservation Agreement Form the undersigned accomplished for this purpose.");
            Paragraph paragraph2 = new Paragraph
                {
                    paragraph2Phrase
                };
            document.Add(paragraph2);
            document.Add(spaceTable);

            Phrase paragraph3Phrase = new Phrase("NOW THEREFORE, for and in consideration of the foregoing premises, the undersigned hereby:");
            Paragraph paragraph3 = new Paragraph
                {
                    paragraph3Phrase
                };
            document.Add(paragraph2);
            document.Add(spaceTable);


            Phrase paragraph4Phrase = new Phrase("Commits to submit the following requirements DP & Loan Processing according to the timelines below as follows:");
            Paragraph paragraph4 = new Paragraph
                {
                   paragraph4Phrase
                };
            document.Add(paragraph4);
            document.Add(spaceTable);

            List list1 = new List(List.ORDERED, 20f);
            list1.SetListSymbol("\u2022");

            list1.IndentationLeft = 20f;
            list1.IndentationRight = 20f;

            list1.Add("Reservation Date		:_____________________(Date today)");
            list1.Add("Down Payment Deadline	:_____________________(30 days)");
            list1.Add("Requirements Deadline	:_____________________(30 days)");
            list1.Add("Loan Approval Deadline	:_____________________(60 days)");
            document.Add(list1);
            document.Add(spaceTable);

            Phrase paragraph5Phrase = new Phrase("NOTE/ INSTRUCTIONS:");
            Paragraph paragraph5 = new Paragraph
                {
                   paragraph5Phrase
                };
            document.Add(paragraph5);
            List list2 = new List(List.ORDERED, 20f);
            list1.SetListSymbol("\u2022");

            list2.Add("BUYER to submit requirements at BANK.");
            list2.Add("Upon submission, client must submit the transmittal slip back to Greentech with the name and signature of the Bank’s representative who received the documents.");
            list2.Add("To confirm submission of complete requirements to the BANK, the client must request the bank to send a confirmation email to Greentech upon reviewing the complete requirements of the client.");
            list2.Add("BUYER to follow-up at BANK for approval.");
            document.Add(list2);
            document.Add(spaceTable);

            Phrase paragraph6Phrase = new Phrase("I undersigned that my failure to submit the above requirements required from me and /or any misrepresentation on the information indicated in my Loan Application Form will be sufficient ground for Greentech Development Corporation to cancel my contract and forfeit as liquidated damages my reservation fee and whatever other payments I made.");
            Paragraph paragraph6 = new Paragraph
                {
                    paragraph6Phrase
                };
            document.Add(paragraph6);
            Phrase paragraph7Phrase = new Phrase("I am aware and I agree that as part of Bank Financing requirement, I will pay the bank charges being billed by the bank and I will sign the contract documents of the bank within 30 days from the bank’s advice, otherwise, at the absolute discretion of the Greentech Development Corporation, to the cancellation or rescission of this contract, penalty charges amounting to not more than Ten Thousand Pesos (Php10,000) per month of the delay period and/or the forfeiture of all amount paid by the buyer as liquidated damages.");
            Paragraph paragraph7 = new Paragraph
                {
                    paragraph7Phrase
                };
            document.Add(paragraph7);

            document.Add(spaceTable);
            document.Add(spaceTable);
            document.Add(spaceTable);



            PdfPTable table = new PdfPTable(2);
            float[] widths1 = new float[] { 5f, 5f };
            table.SetWidths(widths1);
            table.WidthPercentage = 80;

            PdfPCell tablerow1column1 = new PdfPCell(new Phrase("_________________________", updateFontArialBold)) { PaddingTop = 1f, Border = 0 };
            PdfPCell tablerow1column2 = new PdfPCell(new Phrase("_________________________", updateFontArialBold)) { PaddingTop = 1f, Border = 0 };
            PdfPCell tablerow2column1 = new PdfPCell(new Phrase("Signature of Buyer over Printed Name", updateFontArialBold)) { PaddingTop = 1f, Border = 0 };
            PdfPCell tablerowcolumn2 = new PdfPCell(new Phrase("Spouse (if applicable)", updateFontArialBold)) { PaddingTop = 1f, Border = 0 };

            //cell.Colspan = 2;
            //cell.HorizontalAlignment = 0;
            table.AddCell(tablerow1column1);
            table.AddCell(tablerow1column2);
            table.AddCell(tablerow2column1);
            table.AddCell(tablerowcolumn2);
            document.Add(table);



            // ==============
            // Close Document
            // ==============
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

        [HttpGet, Route("ReservationAgreement/{id}")]
        public HttpResponseMessage ReservationAgreement(Int32 id)
        {
            Font updateFontArial10 = FontFactory.GetFont("Arial", 7);
            Font updateFontArial10Bold = FontFactory.GetFont("Arial", 7, Font.BOLD);
            Font updateFontArial10BoldItalic = FontFactory.GetFont("Arial", 5, Font.BOLDITALIC, BaseColor.WHITE);
            Font updateFontArial12Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font updateFontArial12BoldItalic = FontFactory.GetFont("Arial", 9, Font.BOLDITALIC);
            Font updateFontArial12 = FontFactory.GetFont("Arial", 9);
            Font updateFontArial12Italic = FontFactory.GetFont("Arial", 9, Font.ITALIC);
            Font updateFontArial17Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font updateFontArial11Bold = FontFactory.GetFont("Arial", 7, Font.BOLD);
            Font updateFontArialBold = FontFactory.GetFont("Arial", 7, Font.BOLD);

            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.SetPageSize(PageSize.LETTER);
            document.SetMargins(50f, 50f, 50f, 50f);

            // =============
            // Open Document
            // =============
            document.Open();

            // ===========
            // Space Table
            // ===========
            PdfPTable spaceTable = new PdfPTable(1);
            float[] widthCellsSpaceTable = new float[] { 5f };
            spaceTable.SetWidths(widthCellsSpaceTable);
            spaceTable.WidthPercentage = 80;
            spaceTable.AddCell(new PdfPCell(new Phrase(" ", updateFontArial10Bold)) { PaddingTop = 1f, Border = 0 });

            String project = "";
            String unit = "";
            String lotArea = "";
            String TCP = "";

            String reservationFee = "";

            String applicant = "Sample";
            String date = "";
            String address = "";

            // ============
            // Get Customer
            // ============
            var customer = from d in db.MstCustomers
                           where d.Id == Convert.ToInt32(id)
                           select d;

            if (customer.Any())
            {
                var projectLogo = from d in db.MstProjects
                                  select d;

                Image logo = Image.GetInstance(projectLogo.FirstOrDefault().ProjectLogo);
                logo.ScaleToFit(1000f, 60f);

                PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
                pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
                pdfTableCompanyDetail.WidthPercentage = 100;
                pdfTableCompanyDetail.AddCell(new PdfPCell(logo) { Border = 0 });
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("Reservation Aggreeement", updateFontArial17Bold)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 2 });
                document.Add(pdfTableCompanyDetail);
                document.Add(line);

                document.Add(spaceTable);

                applicant = customer.FirstOrDefault().FirstName + " " + customer.FirstOrDefault().MiddleName + " " + customer.FirstOrDefault().LastName;
                address = customer.FirstOrDefault().Address;
            }

            Paragraph p1 = new Paragraph
            {
                new Chunk("To: KAISER URBAN DEVELOPMENT CORP. CEBU CITY", updateFontArial12)
            };
            p1.SetLeading(7f, 0);
            document.Add(p1);
            document.Add(spaceTable);

            Phrase p2Phrase = new Phrase("I, the undersigned, hereby manifest and submit my intension to reserve:", updateFontArial12);
            Paragraph p2 = new Paragraph
            {
                p2Phrase
            };
            p2.SetLeading(7f, 0);
            p2.FirstLineIndent = 40f;
            document.Add(p2);
            document.Add(spaceTable);

            PdfPTable tblProjects = new PdfPTable(5);
            float[] tblProjectWidths = new float[] { 5f, 5f, 5f, 5f, 5f };
            tblProjects.SetWidths(tblProjectWidths);
            tblProjects.WidthPercentage = 100;
            tblProjects.AddCell(new PdfPCell(new Phrase("PROJECT", updateFontArial10Bold)) { HorizontalAlignment = 1, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblProjects.AddCell(new PdfPCell(new Phrase("UNIT", updateFontArial10Bold)) { HorizontalAlignment = 1, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblProjects.AddCell(new PdfPCell(new Phrase("LOT AREA", updateFontArial10Bold)) { HorizontalAlignment = 1, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblProjects.AddCell(new PdfPCell(new Phrase("TCP", updateFontArial10Bold)) { HorizontalAlignment = 1, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblProjects.AddCell(new PdfPCell(new Phrase("TRANSFER CHARGES", updateFontArial10Bold)) { HorizontalAlignment = 1, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblProjects.AddCell(new PdfPCell(new Phrase(project, updateFontArial10)));
            tblProjects.AddCell(new PdfPCell(new Phrase(unit, updateFontArial10)));
            tblProjects.AddCell(new PdfPCell(new Phrase(lotArea, updateFontArial10)) { HorizontalAlignment = 2 });
            tblProjects.AddCell(new PdfPCell(new Phrase(TCP, updateFontArial10)) { HorizontalAlignment = 2 });
            tblProjects.AddCell(new PdfPCell(new Phrase(" ", updateFontArial10)) { HorizontalAlignment = 2 });
            document.Add(tblProjects);
            document.Add(spaceTable);

            Phrase p3Phrase = new Phrase("I fully understand that should I opt to purchase the aforesaid Townhouse/Lot/House:", updateFontArial12);
            Paragraph p3 = new Paragraph
            {
                p3Phrase
            };
            p3.SetLeading(7f, 0);
            p3.FirstLineIndent = 40f;
            document.Add(p3);
            document.Add(spaceTable);

            PdfPTable tblContent = new PdfPTable(3);
            float[] tblContentWidths = new float[] { 5f, 5f, 80f };
            tblContent.SetWidths(tblContentWidths);
            tblContent.WidthPercentage = 100;
            tblContent.AddCell(new PdfPCell(new Phrase("1.", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("My preferred scheme is   _____________________ and is subject to the approval of Greentech Development Corporation.", updateFontArial12)) { Colspan = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase("If I opt to obtain outside financing for the entire balance of the purchase price or any part thereof, I shall comply with the procedure and requirements of GREENTECH DEVELOPMENT CORPORATION, on commercial financing.", updateFontArial12)) { Colspan = 3, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase("2.", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("The RESERVATION FEE of P " + reservationFee + " shall be deductible from the D/P of the TCP.", updateFontArial12)) { Colspan = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase("3.", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("The DOWNPAYMENT of P______________________, payable in the amount of P________________ per month for ____________ (___) months.", updateFontArial12)) { Colspan = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase("4.", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("RESERVATION PERIOD", updateFontArial12)) { Colspan = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("4.01", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("My reservation for the subject LOT / HOUSE&LOT is good for a period of THIRTY (30) days from approval of the reservation application. Should I fail to exercise my option to purchase the LOT/HOUSE & LOT within the reservation period, GREENTECH DEVELOPMENT CORPORATION may sell the LOT / HOUSE&LOT to another applicant/buyer.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("4.02", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("The reservation fee is non-refundable except as provided in paragraphs 6.05 and 6.12 and shall automatically be forfeited infavor of GREENTECH DEVELOPMENT CORPORATION upon the   failure to exercise the option to purchase the LOT / HOUSE & LOT within the reservation period.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("4.03", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("Should I fail  to submit  the signed Contract to Sell and all other required documents  to support the proposed purchase within THIRTY(30) days from date hereof, Greentech Development Corporation at its option, may cancel  this reservation  and forfeit  in its favor any and all amounts I have paid by virtue hereof.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase("5.", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("DOWNPAYMENT / MONTHLY AMORTIZATION / EARNEST MONEY", updateFontArial12)) { Colspan = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase("To effect my option to purchase the property, a down payment or first monthly amortization, whichever is applicable, based on the approved Sample Computation Sheet shall be paid by the applicant within the THIRTY (30)-day reservation period.", updateFontArial12)) { Colspan = 3, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("No cancellation of the sale by the applicant will be entertained after partial or full receipt of the down payment or any monthly amortization.  Any down payment or monthly amortization given by the applicant prior to the submission of the Contract to Sell shall be treated as Earnest Money and considered as proof of the perfection of the contract.  Greentech Development Corporation will have the sole discretion or right to cancel the sale in case of default of any of the payments or in case the applicant indicates that he/she will opt to no longer continue with the sale.  The cancellation will take effect after thirty (30) days from the receipt by the applicant or, in case the applicant cannot be found, from the leaving of a copy, of the notice of cancellation at the address stated in this Agreement.  The Earnest Money will be forfeited in favor of Greentech Development Corporation in case of such cancellation.", updateFontArial12)) { Colspan = 3, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase("6.", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("This reservation agreement is subject to the following TERMS:", updateFontArial12)) { Colspan = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.01", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("This reservation is on a first-come-first-serve basis and shall only take effect upon approval of GREENTECH DEVELOPMENT CORPORATION.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.02", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("This reservation is exclusive for the aforementioned LOT/HOUSE & LOT. A Change of the Lot/House & Lot may be allowed by GREENTECH DEVELOPMENT CORPORATION at its discretionand can be exercised only once subject to company’s policy on request for change of Lot/House & Lot.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.03", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("This  reservation, if and when accepted by Greentech Development Corporation, shall contain the entire agreement  between myself and GREENTECH DEVELOPMENT CORPORATION as of the date of such acceptance and any stipulation, representation, agreement or promise, oral or otherwise, not contained or incorporated herein  by reference, shall not bind GREENTECH DEVELOPMENT CORPORATION.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.04", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("It is understood that any representation or warranty made to me by the Sales Agent who handled this reservation that is not embodied  herein shall not be binding on Greentech Development Corporation unless reduced into writing and signed by authorized signatory of GREENTECH DEVELOPMENT CORPORATION. This reservation shall not be considered as changed, modified or altered or in anyway amended by any act/acts of tolerance by GREENTECH DEVELOPMENT CORPORATION unless such change/s, modification/s or amendment/s are made in writing and signed by the authorized signatory of Greentech Development Corporation.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.05", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("In the event that the above-chosen lot has been found to be not available for sale or disposition, I agree to have the said property exchanged with another lot of similar area and value in the same subdivision, or to the cancellation of this reservation subject to the reimbursement of all the amounts I have thus far paid to GREENTECH DEVELOPMENT CORPORATION without any interest or penalty. I hereby acknowledge and confirm that in case of such cancellation, GREENTECH DEVELOPMENT CORPORATION shall have no liability whatsoever, except to reimburse all the amounts I have remitted to it without any interest or penalty.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.06", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("The applicant’s failure to pay the downpayment / first amortization within the reservation period shall cause the forfeiture of the reservation fee in favor of GREENTECH DEVELOPMENT CORPORATION as damages & as compensation for the opportunity loss.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.07", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("The applicant shall not be allowed to transfer his/her Reservation Application to another individual person or Corporation unless specifically allowed by law.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.08", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("Any and all payments made to a party other than the authorized representative of GREENTECH DEVELOPMENT CORPORATION to receive payments shall be at the applicant’s sole and exclusive risk and responsibility, and shall not make the GREENTECH DEVELOPMENT CORPORATION answerable or responsible in any way therefore.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.09", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("The applicant hereby acknowledges having read and understood the Contract to Sell and other pertinent sales document and agrees to the standard terms and terms and conditions contained therein.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.10", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("In case one or more of the provisions contained in this Reservation Terms and Conditions shall be declared invalid, illegal or unenforceable in any respect by any competent governmental authority, the validity, legality and enforceability of the remaining  provisions contained herein shall not in any way be affected or impaired thereby.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.11", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("This Reservation Terms and Conditions, the Contract to Sell, and The Deed of Absolute Sale to be executed pursuant hereto constitute the entire agreement of the parties concerning the sale, transfer and conveyance of the Lot/House & Lot reserved.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.12", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("I hereby acknowledge that GREENTECH DEVELOPMENT CORPORATION has the right not to accept, or withdraw or cancel its acceptance of this reservation for any cause whatsoever, at any time before the execution of a Contract to Sell in my favor by giving prior written notice of its intention to do so and refunding to me all the amount I have paid to it without interest or penalty.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.13", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("I shall be solely responsible for establishing my legal qualifications to acquire the said Greentech Development Corporation lot and to have the same registered in my name.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.14", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("My failure and/or refusal to submit necessary documents required by GREENTECH DEVELOPMENT CORPORATION within the reservation period may be a ground for forfeiture of this reservation.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.15", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("In the event that the check covering the reservation payment/downpayment corresponding to this reservation is dishonored by the drawee bank concerned for any reason whatsoever, this reservation shall automatically be canceled and shall cease to have any force and effect, regardless of whether or not GREENTECH DEVELOPMENT CORPORATION has accepted this reservation as of the date of such dishonor.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.16", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("This reservation and the rights and obligations of the parties hereunder shall be governed by, and construed in accordance with, the laws of the Republic of the Philippines and any action or proceeding arising out of, or relating to this reservation shall be brought exclusively in the proper courts of the province of Cebu of the Republic of the Philippines.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.17", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("All notices, letters and /or communications to me pertaining to this reservation shall be sent either personally or by registered mail to my mailing address in the Philippines as indicated herein. I undertake to promptly inform GREENTECH DEVELOPMENT CORPORATION n of any change of my address. Any such notice, letter or communication shall be deemed to have been duly delivered or given to me on the date of receipt if delivered personally, or upon the lapse of seven (7) days from its posting by mail.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.18", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("In case of conflict between any stipulation embodied herein and any provision of the Contract to Sell, the provision of the Contract to Sell shall prevail.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContent.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("6.19", updateFontArial12)) { HorizontalAlignment = 2, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContent.AddCell(new PdfPCell(new Phrase("I hereby represent and certify that all the information I have given in this Reservation Agreement are true and accurate as of the date hereof. I undertake to notify GREENTECH DEVELOPMENT CORPORATION in writing of any change in any such information.", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            document.Add(tblContent);
            document.Add(spaceTable);

            PdfPTable tblContentSignature = new PdfPTable(2);
            float[] tblContentSignatureWidths = new float[] { 30f, 70f };
            tblContentSignature.SetWidths(tblContentSignatureWidths);
            tblContentSignature.WidthPercentage = 100;

            tblContentSignature.AddCell(new PdfPCell(new Phrase(applicant.ToUpper(), updateFontArial12)) { Border = 0, HorizontalAlignment = 1, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContentSignature.AddCell(new PdfPCell(new Phrase("With my marital consent: ______________________________________", updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            tblContentSignature.AddCell(new PdfPCell(new Phrase("(Name & Signature of Applicant)", updateFontArial12Bold)) { Border = 0, HorizontalAlignment = 1, PaddingBottom = 10f });
            tblContentSignature.AddCell(new PdfPCell(new Phrase(" ", updateFontArial12)) { Border = 0, PaddingBottom = 10f });

            tblContentSignature.AddCell(new PdfPCell(new Phrase("Date: " + date, updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });
            tblContentSignature.AddCell(new PdfPCell(new Phrase("Address: " + address, updateFontArial12)) { Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            document.Add(tblContentSignature);

            document.Add(spaceTable);
            document.Add(spaceTable);
            document.Add(spaceTable);

            PdfPTable tblContentCompanySignature = new PdfPTable(1);
            float[] tblContentCompanySignatureWidths = new float[] { 100f };
            tblContentCompanySignature.SetWidths(tblContentCompanySignatureWidths);
            tblContentCompanySignature.WidthPercentage = 100;

            tblContentCompanySignature.AddCell(new PdfPCell(new Phrase("GREENTECH DEVELOPMENT CORPORATION", updateFontArial12Bold)) { HorizontalAlignment = 1, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 30f });

            tblContentCompanySignature.AddCell(new PdfPCell(new Phrase("___________________________________________", updateFontArial12Bold)) { HorizontalAlignment = 1, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 2f });
            tblContentCompanySignature.AddCell(new PdfPCell(new Phrase("Authorized Signature", updateFontArial12Bold)) { HorizontalAlignment = 1, Border = 0, PaddingLeft = 5f, PaddingTop = 5f, PaddingRight = 5f, PaddingBottom = 5f });

            document.Add(tblContentCompanySignature);
            document.Add(spaceTable);

            // ==============
            // Close Document
            // ==============
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

        [HttpGet, Route("ComputationSheet/{id}")]
        public HttpResponseMessage ComputationSheet(Int32 id)
        {
            Font updateFontArial10 = FontFactory.GetFont("Arial", 7);
            Font updateFontArial10Bold = FontFactory.GetFont("Arial", 7, Font.BOLD);
            Font updateFontArial10BoldItalic = FontFactory.GetFont("Arial", 5, Font.BOLDITALIC, BaseColor.WHITE);
            Font updateFontArial12Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font updateFontArial12BoldItalic = FontFactory.GetFont("Arial", 9, Font.BOLDITALIC);
            Font updateFontArial12 = FontFactory.GetFont("Arial", 9);
            Font updateFontArial12Italic = FontFactory.GetFont("Arial", 9, Font.ITALIC);
            Font updateFontArial17Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font updateFontArial17UNDERLINE = FontFactory.GetFont("Arial", 12, Font.UNDERLINE);

            Font updateFontArial11Bold = FontFactory.GetFont("Arial", 7, Font.BOLD);
            Font updateFontArialBold = FontFactory.GetFont("Arial", 7, Font.BOLD);

            // ===============
            // Open PDF Stream
            // ===============
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.SetPageSize(PageSize.LETTER);
            document.SetMargins(50f, 50f, 30f, 50f);

            // =============
            // Open Document
            // =============
            document.Open();

            var customer = from d in db.MstCustomers
                           where d.Id == Convert.ToInt32(id)
                           select d;

            if (customer.Any())
            {
                var projectLogo = from d in db.MstProjects
                                  select d;

                Image logo = Image.GetInstance(projectLogo.FirstOrDefault().ProjectLogo);
                logo.ScaleToFit(1000f, 60f);

                PdfPTable pdfTableCompanyDetail = new PdfPTable(2);
                pdfTableCompanyDetail.SetWidths(new float[] { 100f, 100f });
                pdfTableCompanyDetail.WidthPercentage = 100;
                pdfTableCompanyDetail.AddCell(new PdfPCell(logo) { Border = 0 });
                pdfTableCompanyDetail.AddCell(new PdfPCell(new Phrase("Computation Sheet", updateFontArial17Bold)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 2 });
                document.Add(pdfTableCompanyDetail);
                document.Add(line);

                PdfPTable pdfTableComputationSheet = new PdfPTable(4);
                pdfTableComputationSheet.SetWidths(new float[] { 25f, 25f, 100f, 100f });
                pdfTableComputationSheet.WidthPercentage = 100;
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("TCP: ____________________________________", updateFontArial12)) { Colspan = 4, PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("I. R/F: ______________________________________", updateFontArial12Bold)) { Colspan = 3, PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("II. D/P: ________________ = ______________________", updateFontArial12Bold)) { Colspan = 3, PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("Less R/F:  ______________________________________", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("Net D/P:  ______________________________________", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("Terms:  ____________________________month/s @ _______% (interest / discount)", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("Monthly D/P: Php____________________________month/s @ _______% (interest / discount)", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("III. BAL: ______________________________________", updateFontArial12Bold)) { Colspan = 3, PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Colspan = 2, PaddingTop = 5, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("___ CASH", updateFontArial12)) { Colspan = 2, PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("___ BANK", updateFontArial12)) { Colspan = 2, PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("___ IN-HOUSE", updateFontArial12)) { Colspan = 2, PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("Term:__________months", updateFontArial12)) { Colspan = 2, PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("Int. Rate:__________%", updateFontArial12)) { Colspan = 2, PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { Colspan = 2, PaddingTop = 20, Border = 0, HorizontalAlignment = 1 });
                pdfTableComputationSheet.AddCell(new PdfPCell(new Phrase("Mo. Amortization:_________________", updateFontArial12)) { Colspan = 2, PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });

                document.Add(pdfTableComputationSheet);

                document.Add(line);

                PdfPTable pdfTableComputationSheetAdditionalInfo = new PdfPTable(2);
                pdfTableComputationSheetAdditionalInfo.SetWidths(new float[] { 100f, 100f });
                pdfTableComputationSheetAdditionalInfo.WidthPercentage = 100;

                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("Project: _______________________________", updateFontArial12)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("Unit #: ________________________________", updateFontArial12)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });

                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("______________________________________", updateFontArial12)) { PaddingTop = 30, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("______________________________________", updateFontArial12)) { PaddingTop = 30, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("BUYER", updateFontArial12Bold)) { PaddingTop = 3, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("Sales Manager", updateFontArial12Bold)) { PaddingTop = 3, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("(Signature over printed name)", updateFontArial12)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("Date: _________________________________", updateFontArial12)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });

                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("Legend:", updateFontArial12)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { PaddingTop = 20, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("TCP: Total Contract Price", updateFontArial12)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("Approved by:", updateFontArial12Italic)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("R/F: Reservation Fee", updateFontArial12)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("D/P: Down Payment", updateFontArial12)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("", updateFontArial12)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("BAL: Balance", updateFontArial12)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("Kaiser Christopher F. Tan", updateFontArial12Bold)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("Int: Interest per annum", updateFontArial12)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });
                pdfTableComputationSheetAdditionalInfo.AddCell(new PdfPCell(new Phrase("President / CEo", updateFontArial12)) { PaddingTop = 5, Border = 0, HorizontalAlignment = 0 });

                document.Add(pdfTableComputationSheetAdditionalInfo);

            }

            // ==============
            // Close Document
            // ==============
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
