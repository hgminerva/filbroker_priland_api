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
                    CreatedBy = addTrnCommissionRequest.CreatedBy,
                    CreatedDateTime = Convert.ToDateTime(addTrnCommissionRequest.CreatedDateTime),
                    UpdatedBy = addTrnCommissionRequest.UpdatedBy,
                    UpdatedDateTime = Convert.ToDateTime(addTrnCommissionRequest.UpdatedDateTime)
                };

                db.TrnCommissionRequests.InsertOnSubmit(newTrnCommissionRequest);
                db.SubmitChanges();

                return newTrnCommissionRequest.Id;
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
        public HttpResponseMessage UpdateCommissionRequestId(string id, TrnCommissionRequest UpdateTrnCommissionRequest)
        {
            try
            {
                var TrnCommissionRequestData = from d in db.TrnCommissionRequests where d.Id == Convert.ToInt32(id) select d;
                if (TrnCommissionRequestData.Any())
                {
                    if (!TrnCommissionRequestData.First().IsLocked)
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
                        UpdateCommissionRequestData.CreatedBy = UpdateTrnCommissionRequest.CreatedBy;
                        UpdateCommissionRequestData.CreatedDateTime = Convert.ToDateTime(UpdateTrnCommissionRequest.CreatedDateTime);
                        UpdateCommissionRequestData.UpdatedBy = UpdateTrnCommissionRequest.UpdatedBy;
                        UpdateCommissionRequestData.UpdatedDateTime = Convert.ToDateTime(UpdateTrnCommissionRequest.UpdatedDateTime);

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
        public HttpResponseMessage UnLock(string id, TrnCommissionRequest UnLockTrnCommissionRequest)
        {
            try
            {
                var TrnCommissionRequestData = from d in db.TrnCommissionRequests where d.Id == Convert.ToInt32(id) select d;
                if (TrnCommissionRequestData.Any())
                {
                    if (TrnCommissionRequestData.First().IsLocked)
                    {
                        var UnLockCommissionRequesData = TrnCommissionRequestData.FirstOrDefault();

                        UnLockCommissionRequesData.IsLocked = false;

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
