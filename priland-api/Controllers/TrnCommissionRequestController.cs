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
    [RoutePrefix("api/TrnCommissionRequest")]
    public class TrnCommissionRequestController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        //List
        [HttpGet, Route("List")]
        public List<TrnCommissionRequest> GetMstProject()
        {
            var TrnCommissionRequestData = from d in db.TrnCommissionRequests
                                           select new Models.TrnCommissionRequest
                                           {
                                               Id = d.Id,
                                               CommissionRequestNumber = d.CommissionRequestNumber,
                                               CommissionRequestDate = d.CommissionRequestDate.ToShortDateString(),
                                               BrokerId = d.BrokerId,
                                               Broker = d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName,
                                               SoldUnitId = d.SoldUnitId,
                                               CommissionNumber = d.CommissionRequestNumber,
                                               Amount = d.Amount,
                                               Remarks = d.Remarks,
                                               PreparedBy = d.PreparedBy,
                                               CheckedBy = d.CheckedBy,
                                               ApprovedBy = d.ApprovedBy,
                                               Status = d.Status,
                                               IsLocked = d.IsLocked,
                                               CreatedBy = d.CreatedBy,
                                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                               UpdatedBy = d.UpdatedBy,
                                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                           };
            return TrnCommissionRequestData.ToList();
        }

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public TrnCommissionRequest GetTrnCommissionRequestId(string id)
        {
            var TrnCommissionRequestData = from d in db.TrnCommissionRequests
                                           where d.Id == Convert.ToInt32(id)
                                           select new TrnCommissionRequest
                                           {
                                               Id = d.Id,
                                               CommissionRequestNumber = d.CommissionRequestNumber,
                                               CommissionRequestDate = d.CommissionRequestDate.ToShortDateString(),
                                               BrokerId = d.BrokerId,
                                               SoldUnitId = d.SoldUnitId,
                                               CommissionNumber = d.CommissionRequestNumber,
                                               Amount = d.Amount,
                                               Remarks = d.Remarks,
                                               PreparedBy = d.PreparedBy,
                                               CheckedBy = d.CheckedBy,
                                               ApprovedBy = d.ApprovedBy,
                                               Status = d.Status,
                                               IsLocked = d.IsLocked,
                                               CreatedBy = d.CreatedBy,
                                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                               UpdatedBy = d.UpdatedBy,
                                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                           };
            return (TrnCommissionRequest)TrnCommissionRequestData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostTrnCommissionRequest(TrnCommissionRequest addTrnCommissionRequest)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;
                if (currentUser.Any())
                {
                    Data.TrnCommissionRequest newTrnCommissionRequest = new Data.TrnCommissionRequest()
                    {
                        CommissionRequestNumber = addTrnCommissionRequest.CommissionRequestNumber,
                        CommissionRequestDate = Convert.ToDateTime(addTrnCommissionRequest.CommissionRequestDate),
                        BrokerId = addTrnCommissionRequest.BrokerId,
                        SoldUnitId = addTrnCommissionRequest.SoldUnitId,
                        CommissionNumber = addTrnCommissionRequest.CommissionRequestNumber,
                        Amount = addTrnCommissionRequest.Amount,
                        Remarks = addTrnCommissionRequest.Remarks,
                        PreparedBy = addTrnCommissionRequest.PreparedBy,
                        CheckedBy = addTrnCommissionRequest.CheckedBy,
                        ApprovedBy = addTrnCommissionRequest.ApprovedBy,
                        Status = addTrnCommissionRequest.Status,
                        IsLocked = addTrnCommissionRequest.IsLocked,
                        CreatedBy = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedBy = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now
                    };

                    db.TrnCommissionRequests.InsertOnSubmit(newTrnCommissionRequest);
                    db.SubmitChanges();

                    return newTrnCommissionRequest.Id;
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
        public HttpResponseMessage DeleteTrnCommissionRequest(string id)
        {
            try
            {
                var TrnCommissionRequestData = from d in db.TrnCommissionRequests where d.Id == Convert.ToInt32(id) select d;
                if (TrnCommissionRequestData.Any())
                {
                    if (!TrnCommissionRequestData.First().IsLocked)
                    {
                        db.TrnCommissionRequests.DeleteOnSubmit(TrnCommissionRequestData.First());
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
        public HttpResponseMessage SaveTrnCommissionRequest(TrnCommissionRequest commisionrequest)
        {
            try
            {
                var TrnCommissionRequestData = from d in db.TrnCommissionRequests where d.Id == Convert.ToInt32(commisionrequest.Id) select d;
                if (TrnCommissionRequestData.Any())
                {
                    if (TrnCommissionRequestData.FirstOrDefault().IsLocked == false)
                    {
                        var currentUser = from d in db.MstUsers
                                          where d.AspNetId == User.Identity.GetUserId()
                                          select d;

                        if (currentUser.Any())
                        {
                            var UpdateTrnCommissionRequestData = TrnCommissionRequestData.FirstOrDefault();

                            UpdateTrnCommissionRequestData.CommissionRequestNumber = commisionrequest.CommissionRequestNumber;
                            UpdateTrnCommissionRequestData.CommissionRequestDate = Convert.ToDateTime(commisionrequest.CommissionRequestDate);
                            UpdateTrnCommissionRequestData.BrokerId = commisionrequest.BrokerId;
                            UpdateTrnCommissionRequestData.SoldUnitId = commisionrequest.SoldUnitId;
                            UpdateTrnCommissionRequestData.CommissionNumber = commisionrequest.CommissionRequestNumber;
                            UpdateTrnCommissionRequestData.Amount = commisionrequest.Amount;
                            UpdateTrnCommissionRequestData.Remarks = commisionrequest.Remarks;
                            UpdateTrnCommissionRequestData.PreparedBy = commisionrequest.PreparedBy;
                            UpdateTrnCommissionRequestData.CheckedBy = commisionrequest.CheckedBy;
                            UpdateTrnCommissionRequestData.ApprovedBy = commisionrequest.ApprovedBy;
                            UpdateTrnCommissionRequestData.Status = commisionrequest.Status;
                            UpdateTrnCommissionRequestData.UpdatedBy = currentUser.FirstOrDefault().Id;
                            UpdateTrnCommissionRequestData.UpdatedDateTime = DateTime.Now;

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
        public HttpResponseMessage UpdateCommissionRequestId(TrnCommissionRequest UpdateTrnCommissionRequest)
        {
            try
            {
                var TrnCommissionRequestData = from d in db.TrnCommissionRequests where d.Id == Convert.ToInt32(UpdateTrnCommissionRequest.Id) select d;
                if (TrnCommissionRequestData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UpdateCommissionRequestData = TrnCommissionRequestData.FirstOrDefault();

                        UpdateCommissionRequestData.CommissionRequestNumber = UpdateTrnCommissionRequest.CommissionRequestNumber;
                        UpdateCommissionRequestData.CommissionRequestDate = Convert.ToDateTime(UpdateTrnCommissionRequest.CommissionRequestDate);
                        UpdateCommissionRequestData.BrokerId = UpdateTrnCommissionRequest.BrokerId;
                        UpdateCommissionRequestData.SoldUnitId = UpdateTrnCommissionRequest.SoldUnitId;
                        UpdateCommissionRequestData.CommissionNumber = UpdateTrnCommissionRequest.CommissionRequestNumber;
                        UpdateCommissionRequestData.Amount = UpdateTrnCommissionRequest.Amount;
                        UpdateCommissionRequestData.Remarks = UpdateTrnCommissionRequest.Remarks;
                        UpdateCommissionRequestData.PreparedBy = UpdateTrnCommissionRequest.PreparedBy;
                        UpdateCommissionRequestData.CheckedBy = UpdateTrnCommissionRequest.CheckedBy;
                        UpdateCommissionRequestData.ApprovedBy = UpdateTrnCommissionRequest.ApprovedBy;
                        UpdateCommissionRequestData.Status = UpdateTrnCommissionRequest.Status;
                        UpdateCommissionRequestData.IsLocked = true;
                        UpdateCommissionRequestData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateCommissionRequestData.UpdatedDateTime = DateTime.Now;

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
        public HttpResponseMessage UnLock(TrnCommissionRequest UnLockTrnCommissionRequest)
        {
            try
            {
                var TrnCommissionRequestData = from d in db.TrnCommissionRequests where d.Id == Convert.ToInt32(UnLockTrnCommissionRequest.Id) select d;
                if (TrnCommissionRequestData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UnLockCommissionRequesData = TrnCommissionRequestData.FirstOrDefault();

                        UnLockCommissionRequesData.IsLocked = false;
                        UnLockCommissionRequesData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UnLockCommissionRequesData.UpdatedDateTime = DateTime.Now;

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
