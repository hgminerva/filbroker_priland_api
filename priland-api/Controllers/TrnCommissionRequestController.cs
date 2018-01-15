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

        public String padNumWithZero(Int32 number, Int32 length)
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
        public List<TrnCommissionRequest> GetTrnCommissionRequest()
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
                                               SoldUnit = d.TrnSoldUnit.SoldUnitNumber,
                                               CommissionNumber = d.CommissionRequestNumber,
                                               Amount = d.Amount,
                                               Remarks = d.Remarks,
                                               PreparedBy = d.PreparedBy,
                                               PrepearedByUser = d.MstUser2.Username,
                                               CheckedBy = d.CheckedBy,
                                               CheckedByUser = d.MstUser3.Username,
                                               ApprovedBy = d.ApprovedBy,
                                               ApprovedByUser = d.MstUser4.Username,
                                               Status = d.Status,
                                               IsLocked = d.IsLocked,
                                               CreatedBy = d.CreatedBy,
                                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                               UpdatedBy = d.UpdatedBy,
                                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                           };
            return TrnCommissionRequestData.ToList();
        }

        // List per date range
        [HttpGet, Route("ListPerDates/{dateStart}/{dateEnd}")]
        public List<TrnCommissionRequest> GetTrnCommissionRequestPerDates(string dateStart, string dateEnd)
        {
            var TrnCommissionRequestData = from d in db.TrnCommissionRequests
                                           where d.CommissionRequestDate >= Convert.ToDateTime(dateStart) &&
                                                 d.CommissionRequestDate <= Convert.ToDateTime(dateEnd)
                                           orderby d.CommissionNumber descending
                                           select new Models.TrnCommissionRequest
                                           {
                                               Id = d.Id,
                                               CommissionRequestNumber = d.CommissionRequestNumber,
                                               CommissionRequestDate = d.CommissionRequestDate.ToShortDateString(),
                                               BrokerId = d.BrokerId,
                                               Broker = d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName,
                                               SoldUnitId = d.SoldUnitId,
                                               SoldUnit = d.TrnSoldUnit.SoldUnitNumber,
                                               CommissionNumber = d.CommissionRequestNumber,
                                               Amount = d.Amount,
                                               Remarks = d.Remarks,
                                               PreparedBy = d.PreparedBy,
                                               PrepearedByUser = d.MstUser2.Username,
                                               CheckedBy = d.CheckedBy,
                                               CheckedByUser = d.MstUser3.Username,
                                               ApprovedBy = d.ApprovedBy,
                                               ApprovedByUser = d.MstUser4.Username,
                                               Status = d.Status,
                                               IsLocked = d.IsLocked,
                                               CreatedBy = d.CreatedBy,
                                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                               UpdatedBy = d.UpdatedBy,
                                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                           };
            return TrnCommissionRequestData.ToList();
        }

        // Detail
        [HttpGet, Route("Detail/{id}")]
        public TrnCommissionRequest GetTrnCommissionRequestDetail(string id)
        {
            var TrnCommissionRequestData = from d in db.TrnCommissionRequests
                                           where d.Id == Convert.ToInt32(id)
                                           select new TrnCommissionRequest
                                           {
                                               Id = d.Id,
                                               CommissionRequestNumber = d.CommissionRequestNumber,
                                               CommissionRequestDate = d.CommissionRequestDate.ToShortDateString(),
                                               BrokerId = d.BrokerId,
                                               Broker = d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName,
                                               SoldUnitId = d.SoldUnitId,
                                               SoldUnit = d.TrnSoldUnit.SoldUnitNumber,
                                               CommissionNumber = d.CommissionRequestNumber,
                                               Amount = d.Amount,
                                               Remarks = d.Remarks,
                                               PreparedBy = d.PreparedBy,
                                               PrepearedByUser = d.MstUser2.Username,
                                               CheckedBy = d.CheckedBy,
                                               CheckedByUser = d.MstUser3.Username,
                                               ApprovedBy = d.ApprovedBy,
                                               ApprovedByUser = d.MstUser4.Username,
                                               Status = d.Status,
                                               IsLocked = d.IsLocked,
                                               CreatedBy = d.CreatedBy,
                                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                               UpdatedBy = d.UpdatedBy,
                                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                           };
            return TrnCommissionRequestData.FirstOrDefault();
        }

        // Add
        [HttpPost, Route("Add")]
        public Int32 PostTrnCommissionRequest(TrnCommissionRequest commissionRequest)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    string commissionRequestNumber = "0000000001";
                    var commissionRequests = from d in db.TrnCommissionRequests.OrderByDescending(d => d.Id) select d;
                    if (commissionRequests.Any())
                    {
                        Int32 nextCommissionRequestNumber = Convert.ToInt32(commissionRequests.FirstOrDefault().CommissionNumber) + 1;
                        commissionRequestNumber = padNumWithZero(nextCommissionRequestNumber, 10);
                    }

                    Int32 soldUnitId = 0;
                    Int32 brokerId = 0;

                    var soldUnits = from d in db.TrnSoldUnits where d.IsLocked == true select d;
                    if (soldUnits.Any())
                    {
                        var soldUnit = soldUnits.FirstOrDefault();

                        soldUnitId = soldUnit.Id;
                        brokerId = soldUnit.BrokerId;
                    }

                    if (soldUnitId > 0 && brokerId > 0)
                    {
                        Data.TrnCommissionRequest newTrnCommissionRequest = new Data.TrnCommissionRequest()
                        {
                            CommissionRequestNumber = commissionRequestNumber,
                            CommissionRequestDate = Convert.ToDateTime(commissionRequest.CommissionRequestDate),
                            BrokerId = brokerId,
                            SoldUnitId = soldUnitId,
                            CommissionNumber = commissionRequest.CommissionRequestNumber,
                            Amount = commissionRequest.Amount,
                            Remarks = commissionRequest.Remarks,
                            PreparedBy = currentUser.FirstOrDefault().Id,
                            CheckedBy = currentUser.FirstOrDefault().Id,
                            ApprovedBy = currentUser.FirstOrDefault().Id,
                            Status = commissionRequest.Status,
                            IsLocked = commissionRequest.IsLocked,
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
        public HttpResponseMessage DeleteTrnCommissionRequest(string id)
        {
            try
            {
                var TrnCommissionRequestData = from d in db.TrnCommissionRequests where d.Id == Convert.ToInt32(id) select d;

                if (TrnCommissionRequestData.Any())
                {
                    if (TrnCommissionRequestData.First().IsLocked == false)
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

        // Save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveTrnCommissionRequest(TrnCommissionRequest commisionrequest)
        {
            try
            {
                var TrnCommissionRequestData = from d in db.TrnCommissionRequests 
                                               where d.Id == Convert.ToInt32(commisionrequest.Id) select d;
               
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

        // Lock
        [HttpPut, Route("Lock")]
        public HttpResponseMessage LockTrnCommissionRequest(TrnCommissionRequest commissionRequest)
        {
            try
            {
                var TrnCommissionRequestData = from d in db.TrnCommissionRequests
                                               where d.Id == Convert.ToInt32(commissionRequest.Id)
                                               select d;
                
                if (TrnCommissionRequestData.Any())
                {

                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;

                    if (currentUser.Any())
                    {
                        var UpdateCommissionRequestData = TrnCommissionRequestData.FirstOrDefault();

                        UpdateCommissionRequestData.CommissionRequestDate = Convert.ToDateTime(commissionRequest.CommissionRequestDate);
                        UpdateCommissionRequestData.BrokerId = commissionRequest.BrokerId;
                        UpdateCommissionRequestData.SoldUnitId = commissionRequest.SoldUnitId;
                        UpdateCommissionRequestData.CommissionNumber = commissionRequest.CommissionRequestNumber;
                        UpdateCommissionRequestData.Amount = commissionRequest.Amount;
                        UpdateCommissionRequestData.Remarks = commissionRequest.Remarks;
                        UpdateCommissionRequestData.PreparedBy = commissionRequest.PreparedBy;
                        UpdateCommissionRequestData.CheckedBy = commissionRequest.CheckedBy;
                        UpdateCommissionRequestData.ApprovedBy = commissionRequest.ApprovedBy;
                        UpdateCommissionRequestData.Status = commissionRequest.Status;
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

        // Unlock
        [HttpPut, Route("UnLock")]
        public HttpResponseMessage UnlockTrnCommissionRequest(TrnCommissionRequest commissionRequest)
        {
            try
            {
                var TrnCommissionRequestData = from d in db.TrnCommissionRequests
                                               where d.Id == Convert.ToInt32(commissionRequest.Id)
                                               select d;
                
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
