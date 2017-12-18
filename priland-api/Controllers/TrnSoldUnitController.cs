using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using Microsoft.AspNet;
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
                    CreatedBy = addTrnSoldUnit.CreatedBy,
                    CreatedDateTime = Convert.ToDateTime(addTrnSoldUnit.CreatedDateTime),
                    UpdatedBy = addTrnSoldUnit.UpdatedBy,
                    UpdatedDateTime = Convert.ToDateTime(addTrnSoldUnit.UpdatedDateTime)
                };

                db.TrnSoldUnits.InsertOnSubmit(newMstProject);
                db.SubmitChanges();

                return newMstProject.Id;
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
        public HttpResponseMessage UpdateSoldUnit(string id, TrnSoldUnit UpdateTrnSoldUnit)
        {
            try
            {
                var TrnSoldUniData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(id) select d;
                if (TrnSoldUniData.Any())
                {
                    if (!TrnSoldUniData.First().IsLocked)
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
                        UpdateTrnSoldUnitData.CreatedBy = UpdateTrnSoldUnit.CreatedBy;
                        UpdateTrnSoldUnitData.CreatedDateTime = Convert.ToDateTime(UpdateTrnSoldUnit.CreatedDateTime);
                        UpdateTrnSoldUnitData.UpdatedBy = UpdateTrnSoldUnit.UpdatedBy;
                        UpdateTrnSoldUnitData.UpdatedDateTime = Convert.ToDateTime(UpdateTrnSoldUnit.UpdatedDateTime);

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
        public HttpResponseMessage UnLock(string id, TrnSoldUnit UnLockTrnSoldUnit)
        {
            try
            {
                var TrnSoldUnitData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(id) select d;
                if (TrnSoldUnitData.Any())
                {
                    if (TrnSoldUnitData.First().IsLocked)
                    {
                        var UnLockTrnSoldUnitData = TrnSoldUnitData.FirstOrDefault();

                        UnLockTrnSoldUnitData.IsLocked = false;

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
