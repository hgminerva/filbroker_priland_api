﻿using System;
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

        //List
        [HttpGet, Route("List")]
        public List<MstCustomer> GetMstCustomer()
        { 
            var MstCustomerData = from d in db.MstCustomers
                                  select new Models.MstCustomer
                                  {

                                      Id = d.Id,
                                      CustomerCode = d.CustomerCode,
                                      LastName = d.LastName,
                                      FirstName = d.FirstName,
                                      MiddleName = d.MiddleName,
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

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public Models.MstCustomer GetMstCustomerId(string id)
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
            return (Models.MstCustomer)MstCustomerData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostMstCustomer(MstCustomer addMstCustomer)
        {
            try
            {
                Data.MstCustomer newMstCustomer = new Data.MstCustomer()
                {
                    CustomerCode = addMstCustomer.CustomerCode,
                    LastName = addMstCustomer.LastName,
                    FirstName = addMstCustomer.FirstName,
                    MiddleName = addMstCustomer.MiddleName,
                    CivilStatus = addMstCustomer.CivilStatus,
                    Gender = addMstCustomer.Gender,
                    BirthDate = Convert.ToDateTime(addMstCustomer.BirthDate),
                    TIN = addMstCustomer.TIN,
                    IdType = addMstCustomer.IdType,
                    IdNumber = addMstCustomer.IdNumber,
                    Address = addMstCustomer.Address,
                    City = addMstCustomer.City,
                    Province = addMstCustomer.Province,
                    Country = addMstCustomer.Country,
                    ZipCode = addMstCustomer.ZipCode,
                    EmailAddress = addMstCustomer.EmailAddress,
                    TelephoneNumber = addMstCustomer.TelephoneNumber,
                    MobileNumber = addMstCustomer.MobileNumber,
                    Employer = addMstCustomer.Employer,
                    EmployerIndustry = addMstCustomer.EmployerIndustry,
                    NoOfYearsEmployed = addMstCustomer.NoOfYearsEmployed,
                    Position = addMstCustomer.Position,
                    EmploymentStatus = addMstCustomer.EmploymentStatus,
                    EmployerAddress = addMstCustomer.EmployerAddress,
                    EmployerCity = addMstCustomer.EmployerCity,
                    EmployerProvince = addMstCustomer.EmployerProvince,
                    EmployerCountry = addMstCustomer.EmployerCountry,
                    EmployerZipCode = addMstCustomer.EmployerZipCode,
                    EmployerTelephoneNumber = addMstCustomer.EmployerTelephoneNumber,
                    EmployerMobileNumber = addMstCustomer.EmployerMobileNumber,
                    Remarks = addMstCustomer.Remarks,
                    Status = addMstCustomer.Status,
                    Picture = addMstCustomer.Picture,
                    IsLocked = addMstCustomer.IsLocked,
                    CreatedBy = addMstCustomer.CreatedBy,
                    CreatedDateTime = Convert.ToDateTime(addMstCustomer.CreatedDateTime),
                    UpdatedBy = addMstCustomer.UpdatedBy,
                    UpdatedDateTime = Convert.ToDateTime(addMstCustomer.UpdatedDateTime)
                };

                db.MstCustomers.InsertOnSubmit(newMstCustomer);
                db.SubmitChanges();

                return newMstCustomer.Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        //Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstCustomer(string id)
        {
            try
            {
                var MstCustomerData = from d in db.MstCustomers where d.Id == Convert.ToInt32(id) select d;
                if (MstCustomerData.Any())
                {
                    if (!MstCustomerData.First().IsLocked)
                    {
                        db.MstCustomers.DeleteOnSubmit(MstCustomerData.First());
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
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

        //Lock
        [HttpPut, Route("Lock/{id}")]
        public HttpResponseMessage UpdateCustomer(string id, Models.MstCustomer UpdateMstCustomer)
        {
            try
            {
                var MstCustomerData = from d in db.MstCustomers where d.Id == Convert.ToInt32(id) select d;
                if (MstCustomerData.Any())
                {
                    if (!MstCustomerData.First().IsLocked)
                    {
                        var UpdateCustomerData = MstCustomerData.FirstOrDefault();

                        UpdateCustomerData.CustomerCode = UpdateMstCustomer.CustomerCode;
                        UpdateCustomerData.LastName = UpdateMstCustomer.LastName;
                        UpdateCustomerData.FirstName = UpdateMstCustomer.FirstName;
                        UpdateCustomerData.MiddleName = UpdateMstCustomer.MiddleName;
                        UpdateCustomerData.CivilStatus = UpdateMstCustomer.CivilStatus;
                        UpdateCustomerData.Gender = UpdateMstCustomer.Gender;
                        UpdateCustomerData.BirthDate = Convert.ToDateTime(UpdateMstCustomer.BirthDate);
                        UpdateCustomerData.TIN = UpdateMstCustomer.TIN;
                        UpdateCustomerData.IdType = UpdateMstCustomer.IdType;
                        UpdateCustomerData.IdNumber = UpdateMstCustomer.IdNumber;
                        UpdateCustomerData.Address = UpdateMstCustomer.Address;
                        UpdateCustomerData.City = UpdateMstCustomer.City;
                        UpdateCustomerData.Province = UpdateMstCustomer.Province;
                        UpdateCustomerData.Country = UpdateMstCustomer.Country;
                        UpdateCustomerData.ZipCode = UpdateMstCustomer.ZipCode;
                        UpdateCustomerData.EmailAddress = UpdateMstCustomer.EmailAddress;
                        UpdateCustomerData.TelephoneNumber = UpdateMstCustomer.TelephoneNumber;
                        UpdateCustomerData.MobileNumber = UpdateMstCustomer.MobileNumber;
                        UpdateCustomerData.Employer = UpdateMstCustomer.Employer;
                        UpdateCustomerData.EmployerIndustry = UpdateMstCustomer.EmployerIndustry;
                        UpdateCustomerData.NoOfYearsEmployed = UpdateMstCustomer.NoOfYearsEmployed;
                        UpdateCustomerData.Position = UpdateMstCustomer.Position;
                        UpdateCustomerData.EmploymentStatus = UpdateMstCustomer.EmploymentStatus;
                        UpdateCustomerData.EmployerAddress = UpdateMstCustomer.EmployerAddress;
                        UpdateCustomerData.EmployerCity = UpdateMstCustomer.EmployerCity;
                        UpdateCustomerData.EmployerProvince = UpdateMstCustomer.EmployerProvince;
                        UpdateCustomerData.EmployerCountry = UpdateMstCustomer.EmployerCountry;
                        UpdateCustomerData.EmployerZipCode = UpdateMstCustomer.EmployerZipCode;
                        UpdateCustomerData.EmployerTelephoneNumber = UpdateMstCustomer.EmployerTelephoneNumber;
                        UpdateCustomerData.EmployerMobileNumber = UpdateMstCustomer.EmployerMobileNumber;
                        UpdateCustomerData.Remarks = UpdateMstCustomer.Remarks;
                        UpdateCustomerData.Status = UpdateMstCustomer.Status;
                        UpdateCustomerData.IsLocked = true;
                        UpdateCustomerData.CreatedBy = UpdateMstCustomer.CreatedBy;
                        UpdateCustomerData.CreatedDateTime = Convert.ToDateTime(UpdateMstCustomer.CreatedDateTime);
                        UpdateCustomerData.UpdatedBy = UpdateMstCustomer.UpdatedBy;
                        UpdateCustomerData.UpdatedDateTime = Convert.ToDateTime(UpdateMstCustomer.UpdatedDateTime);

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

        //Unlock
        [HttpPut, Route("UnLock/{id}")]
        public HttpResponseMessage UnLock(string id, MstCustomer UnLockMstCustomer)
        {
            try
            {
                var MstCustomerData = from d in db.MstCustomers where d.Id == Convert.ToInt32(id) select d;
                if (MstCustomerData.Any())
                {
                    if (MstCustomerData.First().IsLocked)
                    {
                        var UnLockCustomerData = MstCustomerData.FirstOrDefault();

                        UnLockCustomerData.IsLocked = false;

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