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
                                    RealtyFirm = d.RealtyFirm,
                                    RealtyFirmAddress = d.RealtyFirmAddress,
                                    RealtyFirmTelephoneNumber = d.RealtyFirmTelephoneNumber,
                                    RealtyFirmMobileNumber = d.RealtyFirmMobileNumber,
                                    RealtyFirmFaxNumber = d.RealtyFirmFaxNumber,
                                    RealtyFirmEmailAddress = d.RealtyFirmEmailAddress,
                                    RealtyFirmWebsite = d.RealtyFirmWebsite,
                                    RealtyFirmTIN = d.RealtyFirmTIN,
                                    Organization = d.Organization,
                                    Remarks = d.Remarks,
                                    Picture = d.Picture,
                                    Status = d.Status,
                                    IsLocked = d.IsLocked,
                                    CreatedBy = d.CreatedBy,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedBy = d.UpdatedBy,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
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
                                    RealtyFirm = d.RealtyFirm,
                                    RealtyFirmAddress = d.RealtyFirmAddress,
                                    RealtyFirmTelephoneNumber = d.RealtyFirmTelephoneNumber,
                                    RealtyFirmMobileNumber = d.RealtyFirmMobileNumber,
                                    RealtyFirmFaxNumber = d.RealtyFirmFaxNumber,
                                    RealtyFirmEmailAddress = d.RealtyFirmEmailAddress,
                                    RealtyFirmWebsite = d.RealtyFirmWebsite,
                                    RealtyFirmTIN = d.RealtyFirmTIN,
                                    Organization = d.Organization,
                                    Remarks = d.Remarks,
                                    Picture = d.Picture,
                                    Status = d.Status,
                                    IsLocked = d.IsLocked,
                                    CreatedBy = d.CreatedBy,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedBy = d.UpdatedBy,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
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
                    Data.MstBroker newMstBroker = new Data.MstBroker()
                    {
                        BrokerCode = broker.BrokerCode,
                        LastName = broker.LastName,
                        FirstName = broker.FirstName,
                        MiddleName = broker.MiddleName,
                        LicenseNumber = broker.LicenseNumber,
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
                        RealtyFirm = broker.RealtyFirm,
                        RealtyFirmAddress = broker.RealtyFirmAddress,
                        RealtyFirmTelephoneNumber = broker.RealtyFirmTelephoneNumber,
                        RealtyFirmMobileNumber = broker.RealtyFirmMobileNumber,
                        RealtyFirmFaxNumber = broker.RealtyFirmFaxNumber,
                        RealtyFirmEmailAddress = broker.RealtyFirmEmailAddress,
                        RealtyFirmWebsite = broker.RealtyFirmWebsite,
                        RealtyFirmTIN = broker.RealtyFirmTIN,
                        Organization = broker.Organization,
                        Remarks = broker.Remarks,
                        Picture = broker.Picture,
                        Status = broker.Status,
                        IsLocked = broker.IsLocked,
                        CreatedBy = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedBy = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now,
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
                            UpdateBrokerData.RealtyFirm = broker.RealtyFirm;
                            UpdateBrokerData.RealtyFirmAddress = broker.RealtyFirmAddress;
                            UpdateBrokerData.RealtyFirmTelephoneNumber = broker.RealtyFirmTelephoneNumber;
                            UpdateBrokerData.RealtyFirmMobileNumber = broker.RealtyFirmMobileNumber;
                            UpdateBrokerData.RealtyFirmFaxNumber = broker.RealtyFirmFaxNumber;
                            UpdateBrokerData.RealtyFirmEmailAddress = broker.RealtyFirmEmailAddress;
                            UpdateBrokerData.RealtyFirmWebsite = broker.RealtyFirmWebsite;
                            UpdateBrokerData.RealtyFirmTIN = broker.RealtyFirmTIN;
                            UpdateBrokerData.Organization = broker.Organization;
                            UpdateBrokerData.Remarks = broker.Remarks;
                            UpdateBrokerData.Picture = broker.Picture;
                            UpdateBrokerData.Status = broker.Status;
                            UpdateBrokerData.UpdatedBy = currentUser.FirstOrDefault().Id;
                            UpdateBrokerData.UpdatedDateTime = DateTime.Now;

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
                        var UpdateProjectData = MstBrokerData.FirstOrDefault();

                        UpdateProjectData.BrokerCode = broker.BrokerCode;
                        UpdateProjectData.LastName = broker.LastName;
                        UpdateProjectData.FirstName = broker.FirstName;
                        UpdateProjectData.MiddleName = broker.MiddleName;
                        UpdateProjectData.LicenseNumber = broker.LicenseNumber;
                        UpdateProjectData.BirthDate = Convert.ToDateTime(broker.BirthDate);
                        UpdateProjectData.CivilStatus = broker.CivilStatus;
                        UpdateProjectData.Gender = broker.Gender;
                        UpdateProjectData.Address = broker.Address;
                        UpdateProjectData.TelephoneNumber = broker.TelephoneNumber;
                        UpdateProjectData.MobileNumber = broker.MobileNumber;
                        UpdateProjectData.Religion = broker.Religion;
                        UpdateProjectData.EmailAddress = broker.EmailAddress;
                        UpdateProjectData.Facebook = broker.Facebook;
                        UpdateProjectData.TIN = broker.TIN;
                        UpdateProjectData.RealtyFirm = broker.RealtyFirm;
                        UpdateProjectData.RealtyFirmAddress = broker.RealtyFirmAddress;
                        UpdateProjectData.RealtyFirmTelephoneNumber = broker.RealtyFirmTelephoneNumber;
                        UpdateProjectData.RealtyFirmMobileNumber = broker.RealtyFirmMobileNumber;
                        UpdateProjectData.RealtyFirmFaxNumber = broker.RealtyFirmFaxNumber;
                        UpdateProjectData.RealtyFirmEmailAddress = broker.RealtyFirmEmailAddress;
                        UpdateProjectData.RealtyFirmWebsite = broker.RealtyFirmWebsite;
                        UpdateProjectData.RealtyFirmTIN = broker.RealtyFirmTIN;
                        UpdateProjectData.Organization = broker.Organization;
                        UpdateProjectData.Remarks = broker.Remarks;
                        UpdateProjectData.Picture = broker.Picture;
                        UpdateProjectData.Status = broker.Status;
                        UpdateProjectData.IsLocked = true;
                        UpdateProjectData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateProjectData.UpdatedDateTime = DateTime.Now;

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
