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

        //List
        [HttpGet, Route("List")]
        public List<Models.MstBroker> GetMstBroker()
        {
            var MstBrokerData = from d in db.MstBrokers
                                select new Models.MstBroker
                                {
                                    Id = d.Id,
                                    BrokerCode = d.BrokerCode,
                                    LastName = d.LastName,
                                    FirstName = d.FirstName,
                                    MiddleName = d.MiddleName,
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

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public Models.MstBroker GetMstBrokerId(string id)
        {
            var MstBrokerData = from d in db.MstBrokers
                                where d.Id == Convert.ToInt32(id)
                                select new Models.MstBroker
                                {
                                    Id = d.Id,
                                    BrokerCode = d.BrokerCode,
                                    LastName = d.LastName,
                                    FirstName = d.FirstName,
                                    MiddleName = d.MiddleName,
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

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostMstBroker(MstBroker addMstBroker)
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
                        BrokerCode = addMstBroker.BrokerCode,
                        LastName = addMstBroker.LastName,
                        FirstName = addMstBroker.FirstName,
                        MiddleName = addMstBroker.MiddleName,
                        LicenseNumber = addMstBroker.LicenseNumber,
                        BirthDate = Convert.ToDateTime(addMstBroker.BirthDate),
                        CivilStatus = addMstBroker.CivilStatus,
                        Gender = addMstBroker.Gender,
                        Address = addMstBroker.Address,
                        TelephoneNumber = addMstBroker.TelephoneNumber,
                        MobileNumber = addMstBroker.MobileNumber,
                        Religion = addMstBroker.Religion,
                        EmailAddress = addMstBroker.EmailAddress,
                        Facebook = addMstBroker.Facebook,
                        TIN = addMstBroker.TIN,
                        RealtyFirm = addMstBroker.RealtyFirm,
                        RealtyFirmAddress = addMstBroker.RealtyFirmAddress,
                        RealtyFirmTelephoneNumber = addMstBroker.RealtyFirmTelephoneNumber,
                        RealtyFirmMobileNumber = addMstBroker.RealtyFirmMobileNumber,
                        RealtyFirmFaxNumber = addMstBroker.RealtyFirmFaxNumber,
                        RealtyFirmEmailAddress = addMstBroker.RealtyFirmEmailAddress,
                        RealtyFirmWebsite = addMstBroker.RealtyFirmWebsite,
                        RealtyFirmTIN = addMstBroker.RealtyFirmTIN,
                        Organization = addMstBroker.Organization,
                        Remarks = addMstBroker.Remarks,
                        Picture = addMstBroker.Picture,
                        Status = addMstBroker.Status,
                        IsLocked = addMstBroker.IsLocked,
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

        //Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstBroker(string id)
        {
            try
            {
                var MstBrokerData = from d in db.MstBrokers where d.Id == Convert.ToInt32(id) select d;
                if (MstBrokerData.Any())
                {
                    if (!MstBrokerData.First().IsLocked)
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

        //Save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveBroker(MstBroker broker)
        {
            try
            {
                var MstBrokerData = from d in db.MstBrokers where d.Id == Convert.ToInt32(broker.Id) select d;
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

        //Lock
        [HttpPut, Route("Lock")]
        public HttpResponseMessage UpdateBroker(MstBroker UpdateMstBroker)
        {
            try
            {
                var MstBrokerData = from d in db.MstBrokers where d.Id == Convert.ToInt32(UpdateMstBroker.Id) select d;
                if (MstBrokerData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UpdateProjectData = MstBrokerData.FirstOrDefault();


                        UpdateProjectData.BrokerCode = UpdateMstBroker.BrokerCode;
                        UpdateProjectData.LastName = UpdateMstBroker.LastName;
                        UpdateProjectData.FirstName = UpdateMstBroker.FirstName;
                        UpdateProjectData.MiddleName = UpdateMstBroker.MiddleName;
                        UpdateProjectData.LicenseNumber = UpdateMstBroker.LicenseNumber;
                        UpdateProjectData.BirthDate = Convert.ToDateTime(UpdateMstBroker.BirthDate);
                        UpdateProjectData.CivilStatus = UpdateMstBroker.CivilStatus;
                        UpdateProjectData.Gender = UpdateMstBroker.Gender;
                        UpdateProjectData.Address = UpdateMstBroker.Address;
                        UpdateProjectData.TelephoneNumber = UpdateMstBroker.TelephoneNumber;
                        UpdateProjectData.MobileNumber = UpdateMstBroker.MobileNumber;
                        UpdateProjectData.Religion = UpdateMstBroker.Religion;
                        UpdateProjectData.EmailAddress = UpdateMstBroker.EmailAddress;
                        UpdateProjectData.Facebook = UpdateMstBroker.Facebook;
                        UpdateProjectData.TIN = UpdateMstBroker.TIN;
                        UpdateProjectData.RealtyFirm = UpdateMstBroker.RealtyFirm;
                        UpdateProjectData.RealtyFirmAddress = UpdateMstBroker.RealtyFirmAddress;
                        UpdateProjectData.RealtyFirmTelephoneNumber = UpdateMstBroker.RealtyFirmTelephoneNumber;
                        UpdateProjectData.RealtyFirmMobileNumber = UpdateMstBroker.RealtyFirmMobileNumber;
                        UpdateProjectData.RealtyFirmFaxNumber = UpdateMstBroker.RealtyFirmFaxNumber;
                        UpdateProjectData.RealtyFirmEmailAddress = UpdateMstBroker.RealtyFirmEmailAddress;
                        UpdateProjectData.RealtyFirmWebsite = UpdateMstBroker.RealtyFirmWebsite;
                        UpdateProjectData.RealtyFirmTIN = UpdateMstBroker.RealtyFirmTIN;
                        UpdateProjectData.Organization = UpdateMstBroker.Organization;
                        UpdateProjectData.Remarks = UpdateMstBroker.Remarks;
                        UpdateProjectData.Picture = UpdateMstBroker.Picture;
                        UpdateProjectData.Status = UpdateMstBroker.Status;
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

        //Unlock
        [HttpPut, Route("UnLock")]
        public HttpResponseMessage UnLock(MstBroker UnLockMstBroker)
        {
            try
            {
                var MstBrokerData = from d in db.MstBrokers where d.Id == Convert.ToInt32(UnLockMstBroker.Id) select d;
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
