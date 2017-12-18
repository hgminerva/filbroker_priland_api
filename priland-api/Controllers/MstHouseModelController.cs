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
                                    TFA = d.TFA,
                                    Price = d.Price,
                                    IsLocked = d.IsLocked,
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
                                        IsLocked = d.IsLocked,
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
                Data.MstHouseModel newMstHouseModel = new Data.MstHouseModel()
                {
                    HouseModelCode = addMstHouseModel.HouseModelCode,
                    HouseModel = addMstHouseModel.HouseModel,
                    ProjectId = addMstHouseModel.ProjectId,
                    TFA = addMstHouseModel.TFA,
                    Price = addMstHouseModel.Price,
                    IsLocked = addMstHouseModel.IsLocked,
                    CreatedBy = addMstHouseModel.CreatedBy,
                    CreatedDateTime = Convert.ToDateTime(addMstHouseModel.CreatedDateTime),
                    UpdatedBy = addMstHouseModel.UpdatedBy,
                    UpdatedDateTime = Convert.ToDateTime(addMstHouseModel.UpdatedDateTime)
                };
                db.MstHouseModels.InsertOnSubmit(newMstHouseModel);
                db.SubmitChanges();

                return newMstHouseModel.Id;
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
                    db.MstHouseModels.DeleteOnSubmit(MstHouseModelData.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
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

        //Update Record Data
        [HttpPut, Route("Lock/{id}")]
        public HttpResponseMessage UpdateHouseModel(string id, Models.MstHouseModel UpdateMstHouseModel)
        {
            try
            {
                var MstHouseModelData = from d in db.MstHouseModels where d.Id == Convert.ToInt32(id) select d;
                if (MstHouseModelData.Any())
                {
                    if (!MstHouseModelData.First().IsLocked)
                    {
                        var UpdateHouseModelData = MstHouseModelData.FirstOrDefault();

                        UpdateHouseModelData.HouseModelCode = UpdateMstHouseModel.HouseModelCode;
                        UpdateHouseModelData.HouseModel = UpdateMstHouseModel.HouseModel;
                        UpdateHouseModelData.ProjectId = UpdateMstHouseModel.ProjectId;
                        UpdateHouseModelData.TFA = UpdateMstHouseModel.TFA;
                        UpdateHouseModelData.Price = UpdateMstHouseModel.Price;
                        UpdateHouseModelData.CreatedBy = UpdateMstHouseModel.CreatedBy;
                        UpdateHouseModelData.CreatedDateTime = Convert.ToDateTime(UpdateMstHouseModel.CreatedDateTime);
                        UpdateHouseModelData.UpdatedBy = UpdateMstHouseModel.UpdatedBy;
                        UpdateHouseModelData.UpdatedDateTime = Convert.ToDateTime(UpdateMstHouseModel.UpdatedDateTime);

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

        //UnLock Record
        [HttpPut, Route("UnLock/{id}")]
        public HttpResponseMessage UnLock(string id, MstHouseModel UnLockMstHouseModel)
        {
            try
            {
                var MstHouseModelData = from d in db.MstHouseModels where d.Id == Convert.ToInt32(id) select d;
                if (MstHouseModelData.Any())
                {
                    if (MstHouseModelData.First().IsLocked)
                    {
                        var UnLockHouseModel = MstHouseModelData.FirstOrDefault();

                        UnLockHouseModel.IsLocked = false;

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
