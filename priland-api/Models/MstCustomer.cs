using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class MstCustomer
    {
        public Int32 Id { get; set; }
        public String CustomerCode { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String CivilStatus { get; set; }
        public String Gender { get; set; }
        public String BirthDate { get; set; }
        public String TIN { get; set; }
        public String IdType { get; set; }
        public String IdNumber { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String Province { get; set; }
        public String Country { get; set; }
        public String ZipCode { get; set; }
        public String EmailAddress { get; set; }
        public String TelephoneNumber { get; set; }
        public String MobileNumber { get; set; }
        public String Employer { get; set; }
        public String EmployerIndustry { get; set; }
        public Int32 NoOfYearsEmployed { get; set; }
        public String Position { get; set; }
        public String EmployementStatus { get; set; }
        public String EmployerAddress { get; set; }
        public String EmployerCity { get; set; }
        public String EmployerProvince { get; set; }
        public String EmployerCountry { get; set; }
        public String EmployerZipCode { get; set; }
        public String EmployerTelephoneNumber { get; set; }
        public String EmployerMobileNumber { get; set; }
        public String Remarks { get; set; }
        public String Status { get; set; }
        public String Picture { get; set; }
        public Int32 CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}