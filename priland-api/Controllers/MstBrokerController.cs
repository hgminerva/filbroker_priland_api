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
    [RoutePrefix("api/MstBroker")] 
    public class MstBrokerController : ApiController
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
        public List<MstBroker> GetMstBrokers()
        {
            var MstBrokerData = from d in db.MstBrokers
                                select new MstBroker
                                {
                                    Id = d.Id,
                                    BrokerCode = d.BrokerCode,
                                    LastName = d.LastName,
                                    FirstName = d.FirstName,
                                    MiddleName = d.MiddleName,
                                    FullName = d.LastName + ", " + d.FirstName + " " + d.MiddleName,
                                    LicenseNumber = d.LicenseNumber,
                                    LicenseNumberValidUntil = d.LicenseNumberValidUntil,
                                    BirthDate = d.BirthDate.ToShortDateString(),
                                    CivilStatus = d.CivilStatus,
                                    Gender = d.Gender,
                                    Address = d.Address,
                                    TelephoneNumber = d.TelephoneNumber,
                                    MobileNumber = d.MobileNumber,
                                    Religion = d.Religion,
                                    EmailAddress = d.EmailAddress,
                                    Facebook = d.Facebook,
                                    TIN = d.TIN,
                                    HLURBRegistrationNumber = d.HLURBRegistrationNumber,
                                    RealtyFirm = d.RealtyFirm,
                                    RealtyFirmAddress = d.RealtyFirmAddress,
                                    RealtyFirmTelephoneNumber = d.RealtyFirmTelephoneNumber,
                                    RealtyFirmMobileNumber = d.RealtyFirmMobileNumber,
                                    RealtyFirmFaxNumber = d.RealtyFirmFaxNumber,
                                    RealtyFirmEmailAddress = d.RealtyFirmEmailAddress,
                                    RealtyFirmWebsite = d.RealtyFirmWebsite,
                                    RealtyFirmTIN = d.RealtyFirmTIN,
                                    RealtyFirmLicenseNumber = d.RealtyFirmLicenseNumber,
                                    RealtyFirmLicenseNumberValidUntil = d.RealtyFirmLicenseNumberValidUntil,
                                    RealtyFormHLURBRegistrationNumber = d.RealtyFormHLURBRegistrationNumber,
                                    Organization = d.Organization,
                                    Remarks = d.Remarks,
                                    Picture = d.Picture,
                                    Attachment1 = d.Attachment1,
                                    Attachment2 = d.Attachment2,
                                    Attachment3 = d.Attachment3,
                                    Attachment4 = d.Attachment4,
                                    Attachment5 = d.Attachment5,
                                    Status = d.Status,
                                    IsLocked = d.IsLocked,
                                    CreatedBy = d.CreatedBy,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedBy = d.UpdatedBy,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                                    Type = d.Type
                                };
            return MstBrokerData.ToList();
        }

        // Detail
        [HttpGet, Route("Detail/{id}")]
        public MstBroker GetMstBroker(string id)
        {
            var MstBrokerData = from d in db.MstBrokers
                                where d.Id == Convert.ToInt32(id)
                                select new MstBroker
                                {
                                    Id = d.Id,
                                    BrokerCode = d.BrokerCode,
                                    LastName = d.LastName,
                                    FirstName = d.FirstName,
                                    MiddleName = d.MiddleName,
                                    FullName = d.LastName + ", " + d.FirstName + " " + d.MiddleName,
                                    LicenseNumber = d.LicenseNumber,
                                    LicenseNumberValidUntil = d.LicenseNumberValidUntil,
                                    BirthDate = d.BirthDate.ToShortDateString(),
                                    CivilStatus = d.CivilStatus,
                                    Gender = d.Gender,
                                    Address = d.Address,
                                    TelephoneNumber = d.TelephoneNumber,
                                    MobileNumber = d.MobileNumber,
                                    Religion = d.Religion,
                                    EmailAddress = d.EmailAddress,
                                    Facebook = d.Facebook,
                                    TIN = d.TIN,
                                    HLURBRegistrationNumber = d.HLURBRegistrationNumber,
                                    RealtyFirm = d.RealtyFirm,
                                    RealtyFirmAddress = d.RealtyFirmAddress,
                                    RealtyFirmTelephoneNumber = d.RealtyFirmTelephoneNumber,
                                    RealtyFirmMobileNumber = d.RealtyFirmMobileNumber,
                                    RealtyFirmFaxNumber = d.RealtyFirmFaxNumber,
                                    RealtyFirmEmailAddress = d.RealtyFirmEmailAddress,
                                    RealtyFirmWebsite = d.RealtyFirmWebsite,
                                    RealtyFirmTIN = d.RealtyFirmTIN,
                                    RealtyFirmLicenseNumber = d.RealtyFirmLicenseNumber,
                                    RealtyFirmLicenseNumberValidUntil = d.RealtyFirmLicenseNumberValidUntil,
                                    RealtyFormHLURBRegistrationNumber = d.RealtyFormHLURBRegistrationNumber,
                                    Organization = d.Organization,
                                    Remarks = d.Remarks,
                                    Picture = d.Picture,
                                    Attachment1 = d.Attachment1,
                                    Attachment2 = d.Attachment2,
                                    Attachment3 = d.Attachment3,
                                    Attachment4 = d.Attachment4,
                                    Attachment5 = d.Attachment5,
                                    Status = d.Status,
                                    IsLocked = d.IsLocked,
                                    CreatedBy = d.CreatedBy,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedBy = d.UpdatedBy,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                                    Type = d.Type
                                };
            return (Models.MstBroker)MstBrokerData.FirstOrDefault();
        }

        // Add
        [HttpPost, Route("Add")]
        public Int32 PostMstBroker(MstBroker broker)
        {
            try
            {
                var  currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;

                if (currentUser.Any())
                {

                    string brokerCode = "0001";
                    var brokers = from d in db.MstBrokers.OrderByDescending(d => d.Id) select d;
                    if (brokers.Any())
                    {
                        Int32 nextBrokerCode = Convert.ToInt32(brokers.FirstOrDefault().BrokerCode) + 1;
                        brokerCode = padNumWithZero(nextBrokerCode, 4);
                    }

                    Data.MstBroker newMstBroker = new Data.MstBroker()
                    {
                        BrokerCode = brokerCode,
                        LastName = broker.LastName,
                        FirstName = broker.FirstName,
                        MiddleName = broker.MiddleName,
                        LicenseNumber = broker.LicenseNumber,
                        LicenseNumberValidUntil = broker.LicenseNumberValidUntil,
                        BirthDate = Convert.ToDateTime(broker.BirthDate),
                        CivilStatus = broker.CivilStatus,
                        Gender = broker.Gender,
                        Address = broker.Address,
                        TelephoneNumber = broker.TelephoneNumber,
                        MobileNumber = broker.MobileNumber,
                        Religion = broker.Religion,
                        EmailAddress = broker.EmailAddress,
                        Facebook = broker.Facebook,
                        TIN = broker.TIN,
                        HLURBRegistrationNumber = broker.HLURBRegistrationNumber,
                        RealtyFirm = broker.RealtyFirm,
                        RealtyFirmAddress = broker.RealtyFirmAddress,
                        RealtyFirmTelephoneNumber = broker.RealtyFirmTelephoneNumber,
                        RealtyFirmMobileNumber = broker.RealtyFirmMobileNumber,
                        RealtyFirmFaxNumber = broker.RealtyFirmFaxNumber,
                        RealtyFirmEmailAddress = broker.RealtyFirmEmailAddress,
                        RealtyFirmWebsite = broker.RealtyFirmWebsite,
                        RealtyFirmTIN = broker.RealtyFirmTIN,
                        RealtyFirmLicenseNumber = broker.RealtyFirmLicenseNumber,
                        RealtyFirmLicenseNumberValidUntil = broker.RealtyFirmLicenseNumberValidUntil,
                        RealtyFormHLURBRegistrationNumber = broker.RealtyFormHLURBRegistrationNumber,
                        Organization = broker.Organization,
                        Remarks = broker.Remarks,
                        Picture = broker.Picture,
                        Attachment1 = broker.Attachment1,
                        Attachment2 = broker.Attachment2,
                        Attachment3 = broker.Attachment3,
                        Attachment4 = broker.Attachment4,
                        Attachment5 = broker.Attachment5,
                        Status = broker.Status,
                        IsLocked = broker.IsLocked,
                        CreatedBy = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedBy = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now,
                        Type = broker.Type
                    };

                    db.MstBrokers.InsertOnSubmit(newMstBroker);
                    db.SubmitChanges();

                    return newMstBroker.Id;
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
        public HttpResponseMessage DeleteMstBroker(string id)
        {
            try
            {
                var MstBrokerData = from d in db.MstBrokers 
                                    where d.Id == Convert.ToInt32(id) select d;

                if (MstBrokerData.Any())
                {
                    if (MstBrokerData.First().IsLocked == false)
                    {
                        db.MstBrokers.DeleteOnSubmit(MstBrokerData.First());
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
        public HttpResponseMessage SaveMstBroker(MstBroker broker)
        {
            try
            {
                var MstBrokerData = from d in db.MstBrokers 
                                    where d.Id == Convert.ToInt32(broker.Id) select d;

                if (MstBrokerData.Any())
                {
                    if (MstBrokerData.First().IsLocked == false)
                    {
                        var currentUser = from d in db.MstUsers
                                          where d.AspNetId == User.Identity.GetUserId()
                                          select d;

                        if (currentUser.Any())
                        {
                            var UpdateBrokerData = MstBrokerData.FirstOrDefault();

                            UpdateBrokerData.BrokerCode = broker.BrokerCode;
                            UpdateBrokerData.LastName = broker.LastName;
                            UpdateBrokerData.FirstName = broker.FirstName;
                            UpdateBrokerData.MiddleName = broker.MiddleName;
                            UpdateBrokerData.LicenseNumber = broker.LicenseNumber;
                            UpdateBrokerData.LicenseNumberValidUntil = broker.LicenseNumberValidUntil;
                            UpdateBrokerData.BirthDate = Convert.ToDateTime(broker.BirthDate);
                            UpdateBrokerData.CivilStatus = broker.CivilStatus;
                            UpdateBrokerData.Gender = broker.Gender;
                            UpdateBrokerData.Address = broker.Address;
                            UpdateBrokerData.TelephoneNumber = broker.TelephoneNumber;
                            UpdateBrokerData.MobileNumber = broker.MobileNumber;
                            UpdateBrokerData.Religion = broker.Religion;
                            UpdateBrokerData.EmailAddress = broker.EmailAddress;
                            UpdateBrokerData.Facebook = broker.Facebook;
                            UpdateBrokerData.TIN = broker.TIN;
                            UpdateBrokerData.HLURBRegistrationNumber = broker.HLURBRegistrationNumber;
                            UpdateBrokerData.RealtyFirm = broker.RealtyFirm;
                            UpdateBrokerData.RealtyFirmAddress = broker.RealtyFirmAddress;
                            UpdateBrokerData.RealtyFirmTelephoneNumber = broker.RealtyFirmTelephoneNumber;
                            UpdateBrokerData.RealtyFirmMobileNumber = broker.RealtyFirmMobileNumber;
                            UpdateBrokerData.RealtyFirmFaxNumber = broker.RealtyFirmFaxNumber;
                            UpdateBrokerData.RealtyFirmEmailAddress = broker.RealtyFirmEmailAddress;
                            UpdateBrokerData.RealtyFirmWebsite = broker.RealtyFirmWebsite;
                            UpdateBrokerData.RealtyFirmTIN = broker.RealtyFirmTIN;
                            UpdateBrokerData.RealtyFirmLicenseNumber = broker.RealtyFirmLicenseNumber;
                            UpdateBrokerData.RealtyFirmLicenseNumberValidUntil = broker.RealtyFirmLicenseNumberValidUntil;
                            UpdateBrokerData.RealtyFormHLURBRegistrationNumber = broker.RealtyFormHLURBRegistrationNumber;
                            UpdateBrokerData.Organization = broker.Organization;
                            UpdateBrokerData.Remarks = broker.Remarks;
                            UpdateBrokerData.Picture = broker.Picture;
                            UpdateBrokerData.Attachment1 = broker.Attachment1;
                            UpdateBrokerData.Attachment2 = broker.Attachment2;
                            UpdateBrokerData.Attachment3 = broker.Attachment3;
                            UpdateBrokerData.Attachment4 = broker.Attachment4;
                            UpdateBrokerData.Attachment5 = broker.Attachment5;
                            UpdateBrokerData.Status = broker.Status;
                            UpdateBrokerData.UpdatedBy = currentUser.FirstOrDefault().Id;
                            UpdateBrokerData.UpdatedDateTime = DateTime.Now;
                            UpdateBrokerData.Type = broker.Type;

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
        public HttpResponseMessage LockMstBroker(MstBroker broker)
        {
            try
            {
                var MstBrokerData = from d in db.MstBrokers 
                                    where d.Id == Convert.ToInt32(broker.Id) select d;

                if (MstBrokerData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;

                    if (currentUser.Any())
                    {
                        var UpdateBrokerData = MstBrokerData.FirstOrDefault();

                        UpdateBrokerData.BrokerCode = broker.BrokerCode;
                        UpdateBrokerData.LastName = broker.LastName;
                        UpdateBrokerData.FirstName = broker.FirstName;
                        UpdateBrokerData.MiddleName = broker.MiddleName;
                        UpdateBrokerData.LicenseNumber = broker.LicenseNumber;
                        UpdateBrokerData.LicenseNumberValidUntil = broker.LicenseNumberValidUntil;
                        UpdateBrokerData.BirthDate = Convert.ToDateTime(broker.BirthDate);
                        UpdateBrokerData.CivilStatus = broker.CivilStatus;
                        UpdateBrokerData.Gender = broker.Gender;
                        UpdateBrokerData.Address = broker.Address;
                        UpdateBrokerData.TelephoneNumber = broker.TelephoneNumber;
                        UpdateBrokerData.MobileNumber = broker.MobileNumber;
                        UpdateBrokerData.Religion = broker.Religion;
                        UpdateBrokerData.EmailAddress = broker.EmailAddress;
                        UpdateBrokerData.Facebook = broker.Facebook;
                        UpdateBrokerData.TIN = broker.TIN;
                        UpdateBrokerData.HLURBRegistrationNumber = broker.HLURBRegistrationNumber;
                        UpdateBrokerData.RealtyFirm = broker.RealtyFirm;
                        UpdateBrokerData.RealtyFirmAddress = broker.RealtyFirmAddress;
                        UpdateBrokerData.RealtyFirmTelephoneNumber = broker.RealtyFirmTelephoneNumber;
                        UpdateBrokerData.RealtyFirmMobileNumber = broker.RealtyFirmMobileNumber;
                        UpdateBrokerData.RealtyFirmFaxNumber = broker.RealtyFirmFaxNumber;
                        UpdateBrokerData.RealtyFirmEmailAddress = broker.RealtyFirmEmailAddress;
                        UpdateBrokerData.RealtyFirmWebsite = broker.RealtyFirmWebsite;
                        UpdateBrokerData.RealtyFirmTIN = broker.RealtyFirmTIN;
                        UpdateBrokerData.RealtyFirmLicenseNumber = broker.RealtyFirmLicenseNumber;
                        UpdateBrokerData.RealtyFirmLicenseNumberValidUntil = broker.RealtyFirmLicenseNumberValidUntil;
                        UpdateBrokerData.RealtyFormHLURBRegistrationNumber = broker.RealtyFormHLURBRegistrationNumber;
                        UpdateBrokerData.Organization = broker.Organization;
                        UpdateBrokerData.Remarks = broker.Remarks;
                        UpdateBrokerData.Picture = broker.Picture;
                        UpdateBrokerData.Attachment1 = broker.Attachment1;
                        UpdateBrokerData.Attachment2 = broker.Attachment2;
                        UpdateBrokerData.Attachment3 = broker.Attachment3;
                        UpdateBrokerData.Attachment4 = broker.Attachment4;
                        UpdateBrokerData.Attachment5 = broker.Attachment5;
                        UpdateBrokerData.Status = broker.Status;
                        UpdateBrokerData.IsLocked = true;
                        UpdateBrokerData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateBrokerData.UpdatedDateTime = DateTime.Now;
                        UpdateBrokerData.Type = broker.Type;

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
        public HttpResponseMessage UnLockMstBroker(MstBroker broker)
        {
            try
            {
                var MstBrokerData = from d in db.MstBrokers 
                                    where d.Id == Convert.ToInt32(broker.Id) select d;

                if (MstBrokerData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;

                    if (currentUser.Any())
                    {
                        var UnLockBrokerData = MstBrokerData.FirstOrDefault();

                        UnLockBrokerData.IsLocked = false;
                        UnLockBrokerData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UnLockBrokerData.UpdatedDateTime = DateTime.Now;

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
