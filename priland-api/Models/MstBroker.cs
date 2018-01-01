using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class MstBroker
    {
        public Int32 Id { get; set; }
        public String BrokerCode { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LicenseNumber { get; set; }
        public String BirthDate { get; set; }
        public String CivilStatus { get; set; }
        public String Gender { get; set; }
        public String Address { get; set; }
        public String TelephoneNumber { get; set; }
        public String MobileNumber { get; set; }
        public String Religion { get; set; }
        public String EmailAddress { get; set; }
        public String Facebook { get; set; }
        public String TIN { get; set; }
        public String RealtyFirm { get; set; }
        public String RealtyFirmAddress { get; set; }
        public String RealtyFirmTelephoneNumber { get; set; }
        public String RealtyFirmMobileNumber { get; set; }
        public String RealtyFirmFaxNumber { get; set; }
        public String RealtyFirmEmailAddress { get; set; }
        public String RealtyFirmWebsite { get; set; }
        public String RealtyFirmTIN { get; set; }
        public String Organization { get; set; }
        public String Remarks { get; set; }
        public String Picture { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}