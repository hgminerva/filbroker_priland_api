using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using priland_api.Models;
using Microsoft.AspNet.Identity;

namespace priland_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/MstHouseModel")]
    public class MstHouseModelController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();
         
        //List
        [HttpGet, Route("List")]
        public List<Models.MstHouseModel> GetMstHouseModel()
        {
            var MstHouseModel = from d in db.MstHouseModels
                                select new Models.MstHouseModel
                                {

                                    Id = d.Id,
                                    HouseModelCode = d.HouseModelCode,
                                    HouseModel = d.HouseModel,
                                    ProjectId = d.ProjectId,
                                    Project=d.MstProject.Project,
                                    TFA = d.TFA,
                                    Price = d.Price,
                                    IsLocked=d.IsLocked,
                                    CreatedBy = d.CreatedBy,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedBy = d.UpdatedBy,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                };
            return MstHouseModel.ToList();
        }

        //Get Record Detail
        [HttpGet, Route("Detail/{id}")]
        public Models.MstHouseModel GetMstHouseModelId(string id)
        {
            var MstHouseModelData = from d in db.MstHouseModels
                                    where d.Id == Convert.ToInt32(id)
                                    select new Models.MstHouseModel
                                    {
                                        Id = d.Id,
                                        HouseModelCode = d.HouseModelCode,
                                        HouseModel = d.HouseModel,
                                        ProjectId = d.ProjectId,
                                        TFA = d.TFA,
                                        Price = d.Price,
                                        IsLocked=d.IsLocked,
                                        CreatedBy = d.CreatedBy,
                                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                        UpdatedBy = d.UpdatedBy,
                                        UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                    };
            return (Models.MstHouseModel)MstHouseModelData.FirstOrDefault();
        }

        //Add New Record
        [HttpPost, Route("Add")]
        public int PostMstHouseModel(Models.MstHouseModel addMstHouseModel)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;
                if (currentUser.Any())
                {
                    Data.MstHouseModel newMstHouseModel = new Data.MstHouseModel()
                    {
                        HouseModelCode = addMstHouseModel.HouseModelCode,
                        HouseModel = addMstHouseModel.HouseModel,
                        ProjectId = addMstHouseModel.ProjectId,
                        TFA = addMstHouseModel.TFA,
                        Price = addMstHouseModel.Price,
                        IsLocked=false,
                        CreatedBy = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedBy =currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now
                    };
                    db.MstHouseModels.InsertOnSubmit(newMstHouseModel);
                    db.SubmitChanges();

                    return newMstHouseModel.Id;
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
        
        //Delete Record
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstHouseModel(string id)
        {
            try
            {
                var MstHouseModelData = from d in db.MstHouseModels where d.Id == Convert.ToInt32(id) select d;
                if (MstHouseModelData.Any())
                {
                    if (MstHouseModelData.FirstOrDefault().IsLocked == false)
                    {
                        db.MstHouseModels.DeleteOnSubmit(MstHouseModelData.First());
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
        public HttpResponseMessage SaveMstHouseModel(MstHouseModel housemodel)
        {
            try
            {
                var MstHouseModelData = from d in db.MstHouseModels where d.Id == Convert.ToInt32(housemodel.Id) select d;
                if (MstHouseModelData.Any())
                {
                    if (MstHouseModelData.First().IsLocked == false)
                    {
                        var currentUser = from d in db.MstUsers
                                          where d.AspNetId == User.Identity.GetUserId()
                                          select d;

                        if (currentUser.Any())
                        {
                            var UpdateMstHouseModelData = MstHouseModelData.FirstOrDefault();

                            UpdateMstHouseModelData.HouseModelCode = housemodel.HouseModelCode;
                            UpdateMstHouseModelData.HouseModel = housemodel.HouseModel;
                            UpdateMstHouseModelData.ProjectId = housemodel.ProjectId;
                            UpdateMstHouseModelData.TFA = housemodel.TFA;
                            UpdateMstHouseModelData.Price = housemodel.Price;
                            UpdateMstHouseModelData.UpdatedBy = currentUser.FirstOrDefault().Id;
                            UpdateMstHouseModelData.UpdatedDateTime = DateTime.Now;

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

        //Lock Record Data
        [HttpPut, Route("Lock")]
        public HttpResponseMessage UpdateHouseModel(MstHouseModel UpdateMstHouseModel)
        {
            try
            {
                var MstHouseModelData = from d in db.MstHouseModels where d.Id == Convert.ToInt32(UpdateMstHouseModel.Id) select d;
                if (MstHouseModelData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UpdateHouseModelData = MstHouseModelData.FirstOrDefault();

                        UpdateHouseModelData.HouseModelCode = UpdateMstHouseModel.HouseModelCode;
                        UpdateHouseModelData.HouseModel = UpdateMstHouseModel.HouseModel;
                        UpdateHouseModelData.ProjectId = UpdateMstHouseModel.ProjectId;
                        UpdateHouseModelData.TFA = UpdateMstHouseModel.TFA;
                        UpdateHouseModelData.Price = UpdateMstHouseModel.Price;
                        UpdateHouseModelData.IsLocked = true;
                        UpdateHouseModelData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateHouseModelData.UpdatedDateTime = DateTime.Now;

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

        //Lock Record Data
        [HttpPut, Route("UnLock")]
        public HttpResponseMessage UnLockHouseModel(MstHouseModel UnLockMstHouseModel)
        {
            try
            {
                var MstHouseModelData = from d in db.MstHouseModels where d.Id == Convert.ToInt32(UnLockMstHouseModel.Id) select d;
                if (MstHouseModelData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UpdateHouseModelData = MstHouseModelData.FirstOrDefault();

                        UpdateHouseModelData.IsLocked = false;
                        UpdateHouseModelData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateHouseModelData.UpdatedDateTime = DateTime.Now;

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
