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
    [RoutePrefix("api/MstUnit")]
    public class MstUnitController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        //List 
        [HttpGet, Route("List")]
        public List<MstUnit> GetMstUnit()
        {
            var MstUnitData = from d in db.MstUnits
                              select new MstUnit
                              {

                                  Id = d.Id,
                                  UnitCode = d.UnitCode,
                                  Block = d.Block,
                                  Lot = d.Lot,
                                  ProjectId=d.ProjectId,
                                  Project=d.MstProject.Project,
                                  HouseModelId=d.HouseModelId,
                                  TLA=d.TLA,
                                  TFA=d.TFA,
                                  Price=d.Price,
                                  Status = d.Status,
                                  IsLocked = d.IsLocked,
                                  CreatedBy = d.CreatedBy,
                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                  UpdatedBy = d.UpdatedBy,
                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                              };
            return MstUnitData.ToList();
        }

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public MstUnit GetMstUnitId(string id)
        {
            var MstUnitData = from d in db.MstUnits
                                 where d.Id == Convert.ToInt32(id)
                                 select new MstUnit
                                 {
                                     Id = d.Id,
                                     UnitCode = d.UnitCode,
                                     Block = d.Block,
                                     Lot = d.Lot,
                                     ProjectId = d.ProjectId,
                                     HouseModelId = d.HouseModelId,
                                     TLA = d.TLA,
                                     TFA = d.TFA,
                                     Price = d.Price,
                                     Status = d.Status,
                                     IsLocked = d.IsLocked,
                                     CreatedBy = d.CreatedBy,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedBy = d.UpdatedBy,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return (MstUnit)MstUnitData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostMstUnit(MstUnit addMstUnit)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;
                if (currentUser.Any())
                {
                    Data.MstUnit newMstUnit = new Data.MstUnit()
                    {
                        UnitCode = addMstUnit.UnitCode,
                        Block = addMstUnit.Block,
                        Lot = addMstUnit.Lot,
                        ProjectId = addMstUnit.ProjectId,
                        HouseModelId = addMstUnit.HouseModelId,
                        TLA = addMstUnit.TLA,
                        TFA = addMstUnit.TFA,
                        Price = addMstUnit.Price,
                        Status = addMstUnit.Status,
                        IsLocked = addMstUnit.IsLocked,
                        CreatedBy = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedBy = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now
                    };

                    db.MstUnits.InsertOnSubmit(newMstUnit);
                    db.SubmitChanges();

                    return newMstUnit.Id;
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
        public HttpResponseMessage DeleteMstUnit(string id)
        {
            try
            {
                var MstUnitData = from d in db.MstUnits where d.Id == Convert.ToInt32(id) select d;
                if (MstUnitData.Any())
                {
                    if (!MstUnitData.First().IsLocked)
                    {
                        db.MstUnits.DeleteOnSubmit(MstUnitData.First());
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
        public HttpResponseMessage SaveUnit(MstUnit unit)
        {
            try
            {
                var MstUnitData = from d in db.MstUnits where d.Id == Convert.ToInt32(unit.Id) select d;
                if (MstUnitData.Any())
                {
                    if (MstUnitData.First().IsLocked == false)
                    {
                        var currentUser = from d in db.MstUsers
                                          where d.AspNetId == User.Identity.GetUserId()
                                          select d;

                        if (currentUser.Any())
                        {
                            var UpdateMstUnitData = MstUnitData.FirstOrDefault();

                            UpdateMstUnitData.UnitCode = unit.UnitCode;
                            UpdateMstUnitData.Block = unit.Block;
                            UpdateMstUnitData.Lot = unit.Lot;
                            UpdateMstUnitData.ProjectId = unit.ProjectId;
                            UpdateMstUnitData.HouseModelId = unit.HouseModelId;
                            UpdateMstUnitData.TLA = unit.TLA;
                            UpdateMstUnitData.TFA = unit.TFA;
                            UpdateMstUnitData.Price = unit.Price;
                            UpdateMstUnitData.Status = unit.Status;
                            UpdateMstUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                            UpdateMstUnitData.UpdatedDateTime = DateTime.Now;

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
        public HttpResponseMessage UpdateUnit(MstUnit UpdateMstUnit)
        {
            try
            {
                var MstUnitData = from d in db.MstUnits where d.Id == Convert.ToInt32(UpdateMstUnit.Id) select d;
                if (MstUnitData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UpdateMstUnitData = MstUnitData.FirstOrDefault();

                        UpdateMstUnitData.UnitCode = UpdateMstUnit.UnitCode;
                        UpdateMstUnitData.Block = UpdateMstUnit.Block;
                        UpdateMstUnitData.Lot = UpdateMstUnit.Lot;
                        UpdateMstUnitData.ProjectId = UpdateMstUnit.ProjectId;
                        UpdateMstUnitData.HouseModelId = UpdateMstUnit.HouseModelId;
                        UpdateMstUnitData.TLA = UpdateMstUnit.TLA;
                        UpdateMstUnitData.TFA = UpdateMstUnit.TFA;
                        UpdateMstUnitData.Price = UpdateMstUnit.Price;
                        UpdateMstUnitData.Status = UpdateMstUnit.Status;
                        UpdateMstUnitData.IsLocked = true;
                        UpdateMstUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateMstUnitData.UpdatedDateTime = DateTime.Now;

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
        public HttpResponseMessage UnLock(MstUnit UnLockMstUnit)
        {
            try
            {
                var MstUnitData = from d in db.MstUnits where d.Id == Convert.ToInt32(UnLockMstUnit.Id) select d;
                if (MstUnitData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UnLockMstUnitData = MstUnitData.FirstOrDefault();

                        UnLockMstUnitData.IsLocked = false;
                        UnLockMstUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UnLockMstUnitData.UpdatedDateTime = DateTime.Now;

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
