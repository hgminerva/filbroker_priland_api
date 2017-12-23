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
    [RoutePrefix("api/TrnSoldUnit")]
    public class TrnSoldUnitController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        //List
        [HttpGet, Route("List")]
        public List<TrnSoldUnit> GetTrnSoldUnit()
        {
            var TrnSoldUnitData = from d in db.TrnSoldUnits
                                  select new TrnSoldUnit
                                  {
                                      Id = d.Id,
                                      SoldUnitNumber = d.SoldUnitNumber,
                                      SoldUnitDate = d.SoldUnitDate.ToShortDateString(),
                                      ProjectId = d.ProjectId,
                                      Project = d.MstProject.Project,
                                      UnitId = d.UnitId,
                                      Unit = d.MstUnit.Block + " " + d.MstUnit.Lot,
                                      CustomerId = d.CustomerId,
                                      Customer = d.MstCustomer.LastName + ", " + d.MstCustomer.FirstName + " " + d.MstCustomer.MiddleName,
                                      BrokerId = d.BrokerId,
                                      Broker= d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName,
                                      CheckListId = d.CheckListId,
                                      CheckList=d.MstCheckList.CheckList,
                                      Price = d.Price,
                                      TotalInvestment = d.TotalInvestment,
                                      PaymentOptions = d.PaymentOptions,
                                      Financing = d.Financing,
                                      Remarks = d.Remarks,
                                      PreparedBy = d.PreparedBy,
                                      Prepared=d.MstUser.Username,
                                      CheckedBy = d.CheckedBy,
                                      Checked=d.MstUser.Username,
                                      ApprovedBy = d.ApprovedBy,
                                      Approved=d.MstUser.Username,
                                      Status = d.Status,
                                      IsLocked = d.IsLocked,
                                      CreatedBy = d.CreatedBy,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedBy = d.UpdatedBy,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };
            return TrnSoldUnitData.ToList();
        }

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public TrnSoldUnit GetTrnSoldUnitId(string id)
        {
            var TrnSoldUnitData = from d in db.TrnSoldUnits
                                  where d.Id == Convert.ToInt32(id)
                                  select new TrnSoldUnit
                                  {
                                      Id = d.Id,
                                      SoldUnitNumber = d.SoldUnitNumber,
                                      SoldUnitDate = d.SoldUnitDate.ToShortDateString(),
                                      ProjectId = d.ProjectId,
                                      UnitId = d.UnitId,
                                      CustomerId = d.CustomerId,
                                      BrokerId = d.BrokerId,
                                      CheckListId = d.CheckListId,
                                      Price = d.Price,
                                      TotalInvestment = d.TotalInvestment,
                                      PaymentOptions = d.PaymentOptions,
                                      Financing = d.Financing,
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
            return (TrnSoldUnit)TrnSoldUnitData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostTrnSoldUnit(TrnSoldUnit addTrnSoldUnit)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;
                if (currentUser.Any())
                {
                    Data.TrnSoldUnit newMstProject = new Data.TrnSoldUnit()
                    {
                        SoldUnitNumber = addTrnSoldUnit.SoldUnitNumber,
                        SoldUnitDate = Convert.ToDateTime(addTrnSoldUnit.SoldUnitDate),
                        ProjectId = addTrnSoldUnit.ProjectId,
                        UnitId = addTrnSoldUnit.UnitId,
                        CustomerId = addTrnSoldUnit.CustomerId,
                        BrokerId = addTrnSoldUnit.BrokerId,
                        CheckListId = addTrnSoldUnit.CheckListId,
                        Price = addTrnSoldUnit.Price,
                        TotalInvestment = addTrnSoldUnit.TotalInvestment,
                        PaymentOptions = addTrnSoldUnit.PaymentOptions,
                        Financing = addTrnSoldUnit.Financing,
                        Remarks = addTrnSoldUnit.Remarks,
                        PreparedBy = addTrnSoldUnit.PreparedBy,
                        CheckedBy = addTrnSoldUnit.CheckedBy,
                        ApprovedBy = addTrnSoldUnit.ApprovedBy,
                        Status = addTrnSoldUnit.Status,
                        IsLocked = addTrnSoldUnit.IsLocked,
                        CreatedBy = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedBy = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now
                    };

                    db.TrnSoldUnits.InsertOnSubmit(newMstProject);
                    db.SubmitChanges();

                    return newMstProject.Id;
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
        public HttpResponseMessage DeleteTrnSoldUnit(string id)
        {
            try
            {
                var TrnSoldUnitData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(id) select d;
                if (TrnSoldUnitData.Any())
                {
                    if (!TrnSoldUnitData.First().IsLocked)
                    {
                        db.TrnSoldUnits.DeleteOnSubmit(TrnSoldUnitData.First());
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
        public HttpResponseMessage SaveSoldUnit(TrnSoldUnit soldunit)
        {
            try
            {
                var TrnSoldUniData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(soldunit.Id) select d;
                if (TrnSoldUniData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UpdateTrnSoldUnitData = TrnSoldUniData.FirstOrDefault();

                        UpdateTrnSoldUnitData.SoldUnitNumber = soldunit.SoldUnitNumber;
                        UpdateTrnSoldUnitData.SoldUnitDate = Convert.ToDateTime(soldunit.SoldUnitDate);
                        UpdateTrnSoldUnitData.ProjectId = soldunit.ProjectId;
                        UpdateTrnSoldUnitData.UnitId = soldunit.UnitId;
                        UpdateTrnSoldUnitData.CustomerId = soldunit.CustomerId;
                        UpdateTrnSoldUnitData.BrokerId = soldunit.BrokerId;
                        UpdateTrnSoldUnitData.CheckListId = soldunit.CheckListId;
                        UpdateTrnSoldUnitData.Price = soldunit.Price;
                        UpdateTrnSoldUnitData.TotalInvestment = soldunit.TotalInvestment;
                        UpdateTrnSoldUnitData.PaymentOptions = soldunit.PaymentOptions;
                        UpdateTrnSoldUnitData.Financing = soldunit.Financing;
                        UpdateTrnSoldUnitData.Remarks = soldunit.Remarks;
                        UpdateTrnSoldUnitData.PreparedBy = soldunit.PreparedBy;
                        UpdateTrnSoldUnitData.CheckedBy = soldunit.CheckedBy;
                        UpdateTrnSoldUnitData.ApprovedBy = soldunit.ApprovedBy;
                        UpdateTrnSoldUnitData.Status = soldunit.Status;
                        UpdateTrnSoldUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateTrnSoldUnitData.UpdatedDateTime = DateTime.Now;

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
        //Lock
        [HttpPut, Route("Lock")]
        public HttpResponseMessage UpdateSoldUnit(TrnSoldUnit UpdateTrnSoldUnit)
        {
            try
            {
                var TrnSoldUniData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(UpdateTrnSoldUnit.Id) select d;
                if (TrnSoldUniData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UpdateTrnSoldUnitData = TrnSoldUniData.FirstOrDefault();

                        UpdateTrnSoldUnitData.SoldUnitNumber = UpdateTrnSoldUnit.SoldUnitNumber;
                        UpdateTrnSoldUnitData.SoldUnitDate = Convert.ToDateTime(UpdateTrnSoldUnit.SoldUnitDate);
                        UpdateTrnSoldUnitData.ProjectId = UpdateTrnSoldUnit.ProjectId;
                        UpdateTrnSoldUnitData.UnitId = UpdateTrnSoldUnit.UnitId;
                        UpdateTrnSoldUnitData.CustomerId = UpdateTrnSoldUnit.CustomerId;
                        UpdateTrnSoldUnitData.BrokerId = UpdateTrnSoldUnit.BrokerId;
                        UpdateTrnSoldUnitData.CheckListId = UpdateTrnSoldUnit.CheckListId;
                        UpdateTrnSoldUnitData.Price = UpdateTrnSoldUnit.Price;
                        UpdateTrnSoldUnitData.TotalInvestment = UpdateTrnSoldUnit.TotalInvestment;
                        UpdateTrnSoldUnitData.PaymentOptions = UpdateTrnSoldUnit.PaymentOptions;
                        UpdateTrnSoldUnitData.Financing = UpdateTrnSoldUnit.Financing;
                        UpdateTrnSoldUnitData.Remarks = UpdateTrnSoldUnit.Remarks;
                        UpdateTrnSoldUnitData.PreparedBy = UpdateTrnSoldUnit.PreparedBy;
                        UpdateTrnSoldUnitData.CheckedBy = UpdateTrnSoldUnit.CheckedBy;
                        UpdateTrnSoldUnitData.ApprovedBy = UpdateTrnSoldUnit.ApprovedBy;
                        UpdateTrnSoldUnitData.Status = UpdateTrnSoldUnit.Status;
                        UpdateTrnSoldUnitData.IsLocked = true;
                        UpdateTrnSoldUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateTrnSoldUnitData.UpdatedDateTime = DateTime.Now;

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
        public HttpResponseMessage UnLock(TrnSoldUnit UnLockTrnSoldUnit)
        {
            try
            {
                var TrnSoldUnitData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(UnLockTrnSoldUnit.Id) select d;
                if (TrnSoldUnitData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UnLockTrnSoldUnitData = TrnSoldUnitData.FirstOrDefault();

                        UnLockTrnSoldUnitData.IsLocked = false;
                        UnLockTrnSoldUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UnLockTrnSoldUnitData.UpdatedDateTime = DateTime.Now;

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
