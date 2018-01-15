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
    [RoutePrefix("api/MstCustomer")]
    public class MstCustomerController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        private String padNumWithZero(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

        // List
        [HttpGet, Route("List")]
        public List<MstCustomer> GetMstCustomers()
        { 
            var MstCustomerData = from d in db.MstCustomers
                                  select new Models.MstCustomer
                                  {
                                      Id = d.Id,
                                      CustomerCode = d.CustomerCode,
                                      LastName = d.LastName,
                                      FirstName = d.FirstName,
                                      MiddleName = d.MiddleName,
                                      FullName = d.LastName + ", " + d.FirstName + " " + d.MiddleName,
                                      CivilStatus = d.CivilStatus,
                                      Gender = d.Gender,
                                      BirthDate = d.BirthDate.ToShortDateString(),
                                      TIN = d.TIN,
                                      IdType = d.IdType,
                                      IdNumber = d.IdNumber,
                                      Address = d.Address,
                                      City = d.City,
                                      Province = d.Province,
                                      Country = d.Country,
                                      ZipCode = d.ZipCode,
                                      EmailAddress = d.EmailAddress,
                                      TelephoneNumber = d.TelephoneNumber,
                                      MobileNumber = d.MobileNumber,
                                      Employer = d.Employer,
                                      EmployerIndustry = d.EmployerIndustry,
                                      NoOfYearsEmployed = d.NoOfYearsEmployed,
                                      Position = d.Position,
                                      EmploymentStatus = d.EmploymentStatus,
                                      EmployerAddress = d.EmployerAddress,
                                      EmployerCity = d.EmployerCity,
                                      EmployerProvince = d.EmployerProvince,
                                      EmployerCountry = d.EmployerCountry,
                                      EmployerZipCode = d.EmployerZipCode,
                                      EmployerTelephoneNumber = d.EmployerTelephoneNumber,
                                      EmployerMobileNumber = d.EmployerMobileNumber,
                                      Remarks = d.Remarks,
                                      Status = d.Status,
                                      Picture = d.Picture,
                                      IsLocked = d.IsLocked,
                                      CreatedBy = d.CreatedBy,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedBy = d.UpdatedBy,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };
            return MstCustomerData.ToList();
        }

        // Detail
        [HttpGet, Route("Detail/{id}")]
        public Models.MstCustomer GetMstCustomer(string id)
        {
            var MstCustomerData = from d in db.MstCustomers
                                  where d.Id == Convert.ToInt32(id)
                                  select new Models.MstCustomer
                                  {
                                      Id = d.Id,
                                      CustomerCode = d.CustomerCode,
                                      LastName = d.LastName,
                                      FirstName = d.FirstName,
                                      MiddleName = d.MiddleName,
                                      FullName = d.LastName + ", " + d.FirstName + " " + d.MiddleName,
                                      CivilStatus = d.CivilStatus,
                                      Gender = d.Gender,
                                      BirthDate = d.BirthDate.ToShortDateString(),
                                      TIN = d.TIN,
                                      IdType = d.IdType,
                                      IdNumber = d.IdNumber,
                                      Address = d.Address,
                                      City = d.City,
                                      Province = d.Province,
                                      Country = d.Country,
                                      ZipCode = d.ZipCode,
                                      EmailAddress = d.EmailAddress,
                                      TelephoneNumber = d.TelephoneNumber,
                                      MobileNumber = d.MobileNumber,
                                      Employer = d.Employer,
                                      EmployerIndustry = d.EmployerIndustry,
                                      NoOfYearsEmployed = d.NoOfYearsEmployed,
                                      Position = d.Position,
                                      EmploymentStatus = d.EmploymentStatus,
                                      EmployerAddress = d.EmployerAddress,
                                      EmployerCity = d.EmployerCity,
                                      EmployerProvince = d.EmployerProvince,
                                      EmployerCountry = d.EmployerCountry,
                                      EmployerZipCode = d.EmployerZipCode,
                                      EmployerTelephoneNumber = d.EmployerTelephoneNumber,
                                      EmployerMobileNumber = d.EmployerMobileNumber,
                                      Remarks = d.Remarks,
                                      Status = d.Status,
                                      Picture = d.Picture,
                                      IsLocked = d.IsLocked,
                                      CreatedBy = d.CreatedBy,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedBy = d.UpdatedBy,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };
            return MstCustomerData.FirstOrDefault();
        }

        // Add
        [HttpPost, Route("Add")]
        public Int32 PostMstCustomer(MstCustomer customer)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {

                    string customerCode = "0001";
                    var customers = from d in db.MstCustomers.OrderByDescending(d => d.Id) select d;
                    if (customers.Any())
                    {
                        Int32 nextProjectCode = Convert.ToInt32(customers.FirstOrDefault().CustomerCode) + 1;
                        customerCode = padNumWithZero(nextProjectCode, 4);
                    }

                    Data.MstCustomer newMstCustomer = new Data.MstCustomer()
                    {
                        CustomerCode = customerCode,
                        LastName = customer.LastName,
                        FirstName = customer.FirstName,
                        MiddleName = customer.MiddleName,
                        CivilStatus = customer.CivilStatus,
                        Gender = customer.Gender,
                        BirthDate = Convert.ToDateTime(customer.BirthDate),
                        TIN = customer.TIN,
                        IdType = customer.IdType,
                        IdNumber = customer.IdNumber,
                        Address = customer.Address,
                        City = customer.City,
                        Province = customer.Province,
                        Country = customer.Country,
                        ZipCode = customer.ZipCode,
                        EmailAddress = customer.EmailAddress,
                        TelephoneNumber = customer.TelephoneNumber,
                        MobileNumber = customer.MobileNumber,
                        Employer = customer.Employer,
                        EmployerIndustry = customer.EmployerIndustry,
                        NoOfYearsEmployed = customer.NoOfYearsEmployed,
                        Position = customer.Position,
                        EmploymentStatus = customer.EmploymentStatus,
                        EmployerAddress = customer.EmployerAddress,
                        EmployerCity = customer.EmployerCity,
                        EmployerProvince = customer.EmployerProvince,
                        EmployerCountry = customer.EmployerCountry,
                        EmployerZipCode = customer.EmployerZipCode,
                        EmployerTelephoneNumber = customer.EmployerTelephoneNumber,
                        EmployerMobileNumber = customer.EmployerMobileNumber,
                        Remarks = customer.Remarks,
                        Status = customer.Status,
                        Picture = customer.Picture,
                        IsLocked = customer.IsLocked,
                        CreatedBy = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedBy = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now,
                    };

                    db.MstCustomers.InsertOnSubmit(newMstCustomer);
                    db.SubmitChanges();

                    return newMstCustomer.Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstCustomer(string id)
        {
            try
            {
                var MstCustomerData = from d in db.MstCustomers 
                                      where d.Id == Convert.ToInt32(id) select d;

                if (MstCustomerData.Any())
                {

                    if (MstCustomerData.First().IsLocked == false)
                    {
                        db.MstCustomers.DeleteOnSubmit(MstCustomerData.First());
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // Save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveMstCustomer(MstCustomer customer)
        {
            try
            {
                var MstCustomerData = from d in db.MstCustomers 
                                      where d.Id == Convert.ToInt32(customer.Id) select d;

                if (MstCustomerData.Any())
                {
                    if (MstCustomerData.First().IsLocked == false)
                    {
                        var currentUser = from d in db.MstUsers
                                          where d.AspNetId == User.Identity.GetUserId()
                                          select d;

                        if (currentUser.Any())
                        {
                            var UpdateMstCustomerData = MstCustomerData.FirstOrDefault();

                            UpdateMstCustomerData.CustomerCode = customer.CustomerCode;
                            UpdateMstCustomerData.LastName = customer.LastName;
                            UpdateMstCustomerData.FirstName = customer.FirstName;
                            UpdateMstCustomerData.MiddleName = customer.MiddleName;
                            UpdateMstCustomerData.CivilStatus = customer.CivilStatus;
                            UpdateMstCustomerData.BirthDate = Convert.ToDateTime(customer.BirthDate);
                            UpdateMstCustomerData.TIN = customer.TIN;
                            UpdateMstCustomerData.IdType = customer.IdType;
                            UpdateMstCustomerData.IdNumber = customer.IdNumber;
                            UpdateMstCustomerData.Address = customer.Address;
                            UpdateMstCustomerData.City = customer.City;
                            UpdateMstCustomerData.Province = customer.Province;
                            UpdateMstCustomerData.Country = customer.Country;
                            UpdateMstCustomerData.ZipCode = customer.ZipCode;
                            UpdateMstCustomerData.EmailAddress = customer.EmailAddress;
                            UpdateMstCustomerData.TelephoneNumber = customer.TelephoneNumber;
                            UpdateMstCustomerData.MobileNumber = customer.MobileNumber;
                            UpdateMstCustomerData.Employer = customer.Employer;
                            UpdateMstCustomerData.EmployerIndustry = customer.EmployerIndustry;
                            UpdateMstCustomerData.NoOfYearsEmployed = customer.NoOfYearsEmployed;
                            UpdateMstCustomerData.Position = customer.Position;
                            UpdateMstCustomerData.EmploymentStatus = customer.EmploymentStatus;
                            UpdateMstCustomerData.EmployerAddress = customer.EmployerAddress;
                            UpdateMstCustomerData.EmployerCity = customer.EmployerCity;
                            UpdateMstCustomerData.EmployerProvince = customer.EmployerProvince;
                            UpdateMstCustomerData.EmployerCountry = customer.EmployerCountry;
                            UpdateMstCustomerData.EmployerZipCode = customer.EmployerZipCode;
                            UpdateMstCustomerData.EmployerTelephoneNumber = customer.EmployerTelephoneNumber;
                            UpdateMstCustomerData.EmployerMobileNumber = customer.EmployerMobileNumber;
                            UpdateMstCustomerData.Remarks = customer.Remarks;
                            UpdateMstCustomerData.Status = customer.Status;
                            UpdateMstCustomerData.UpdatedBy = currentUser.FirstOrDefault().Id;
                            UpdateMstCustomerData.UpdatedDateTime = DateTime.Now;

                            db.SubmitChanges();

                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // Lock
        [HttpPut, Route("Lock")]
        public HttpResponseMessage LockMstCustomer(MstCustomer customer)
        {
            try
            {
                var MstCustomerData = from d in db.MstCustomers
                                      where d.Id == Convert.ToInt32(customer.Id)
                                      select d;

                if (MstCustomerData.Any())
                {

                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;

                    if (currentUser.Any())
                    {
                        var UpdateCustomerData = MstCustomerData.FirstOrDefault();

                        UpdateCustomerData.CustomerCode = customer.CustomerCode;
                        UpdateCustomerData.LastName = customer.LastName;
                        UpdateCustomerData.FirstName = customer.FirstName;
                        UpdateCustomerData.MiddleName = customer.MiddleName;
                        UpdateCustomerData.CivilStatus = customer.CivilStatus;
                        UpdateCustomerData.Gender = customer.Gender;
                        UpdateCustomerData.BirthDate = Convert.ToDateTime(customer.BirthDate);
                        UpdateCustomerData.TIN = customer.TIN;
                        UpdateCustomerData.IdType = customer.IdType;
                        UpdateCustomerData.IdNumber = customer.IdNumber;
                        UpdateCustomerData.Address = customer.Address;
                        UpdateCustomerData.City = customer.City;
                        UpdateCustomerData.Province = customer.Province;
                        UpdateCustomerData.Country = customer.Country;
                        UpdateCustomerData.ZipCode = customer.ZipCode;
                        UpdateCustomerData.EmailAddress = customer.EmailAddress;
                        UpdateCustomerData.TelephoneNumber = customer.TelephoneNumber;
                        UpdateCustomerData.MobileNumber = customer.MobileNumber;
                        UpdateCustomerData.Employer = customer.Employer;
                        UpdateCustomerData.EmployerIndustry = customer.EmployerIndustry;
                        UpdateCustomerData.NoOfYearsEmployed = customer.NoOfYearsEmployed;
                        UpdateCustomerData.Position = customer.Position;
                        UpdateCustomerData.EmploymentStatus = customer.EmploymentStatus;
                        UpdateCustomerData.EmployerAddress = customer.EmployerAddress;
                        UpdateCustomerData.EmployerCity = customer.EmployerCity;
                        UpdateCustomerData.EmployerProvince = customer.EmployerProvince;
                        UpdateCustomerData.EmployerCountry = customer.EmployerCountry;
                        UpdateCustomerData.EmployerZipCode = customer.EmployerZipCode;
                        UpdateCustomerData.EmployerTelephoneNumber = customer.EmployerTelephoneNumber;
                        UpdateCustomerData.EmployerMobileNumber = customer.EmployerMobileNumber;
                        UpdateCustomerData.Remarks = customer.Remarks;
                        UpdateCustomerData.Status = customer.Status;
                        UpdateCustomerData.IsLocked = true;
                        UpdateCustomerData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateCustomerData.UpdatedDateTime = DateTime.Now;

                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // Unlock
        [HttpPut, Route("UnLock")]
        public HttpResponseMessage UnLockMstCustomer(MstCustomer customer)
        {
            try
            {
                var MstCustomerData = from d in db.MstCustomers
                                      where d.Id == Convert.ToInt32(customer.Id)
                                      select d;

                if (MstCustomerData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;

                    if (currentUser.Any())
                    {
                        var UnLockCustomerData = MstCustomerData.FirstOrDefault();

                        UnLockCustomerData.IsLocked = false;
                        UnLockCustomerData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UnLockCustomerData.UpdatedDateTime = DateTime.Now;

                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
