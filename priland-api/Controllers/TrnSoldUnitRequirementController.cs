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
    [RoutePrefix("api/TrnSoldUnitRequirement")]
    public class TrnSoldUnitRequirementController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        //List
        [HttpGet, Route("List")]
        public List<TrnSoldUnitRequirement> GetTrnSoldUnitRequirement()
        {
            var TrnSoldUnitRequirementData = from d in db.TrnSoldUnitRequirements
                                             select new TrnSoldUnitRequirement
                                             {
                                                 Id = d.Id,
                                                 SoldUnitId = d.SoldUnitId,
                                                 CheckListRequirementId = d.CheckListRequirementId,
                                                 Attachment1 = d.Attachment1,
                                                 Attachment2 = d.Attachment2,
                                                 Attachment3 = d.Attachment3,
                                                 Attachment4 = d.Attachment4,
                                                 Attachment5 = d.Attachment5,
                                                 Remarks = d.Remarks,
                                                 Status = d.Status,
                                                 StatusDate = d.StatusDate.ToLongDateString()
                                             };
            return TrnSoldUnitRequirementData.ToList();
        }

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public TrnSoldUnitRequirement GetTrnSoldUnitRequirementId(string id)
        {
            var TrnSoldUnitRequirementData = from d in db.TrnSoldUnitRequirements
                                             where d.Id == Convert.ToInt32(id)
                                             select new TrnSoldUnitRequirement
                                             {
                                                 Id = d.Id,
                                                 SoldUnitId = d.SoldUnitId,
                                                 CheckListRequirementId = d.CheckListRequirementId,
                                                 Attachment1 = d.Attachment1,
                                                 Attachment2 = d.Attachment2,
                                                 Attachment3 = d.Attachment3,
                                                 Attachment4 = d.Attachment4,
                                                 Attachment5 = d.Attachment5,
                                                 Remarks = d.Remarks,
                                                 Status = d.Status,
                                                 StatusDate = d.StatusDate.ToLongDateString()
                                             };
            return (TrnSoldUnitRequirement)TrnSoldUnitRequirementData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostTrnSoldUnitRequirement(TrnSoldUnitRequirement addTrnSoldUnitRequirement)
        {
            try
            {
                Data.TrnSoldUnitRequirement newTrnSoldUnitRequirement = new Data.TrnSoldUnitRequirement()
                {
                    SoldUnitId = addTrnSoldUnitRequirement.SoldUnitId,
                    CheckListRequirementId = addTrnSoldUnitRequirement.CheckListRequirementId,
                    Attachment1 = addTrnSoldUnitRequirement.Attachment1,
                    Attachment2 = addTrnSoldUnitRequirement.Attachment2,
                    Attachment3 = addTrnSoldUnitRequirement.Attachment3,
                    Attachment4 = addTrnSoldUnitRequirement.Attachment4,
                    Attachment5 = addTrnSoldUnitRequirement.Attachment5,
                    Remarks = addTrnSoldUnitRequirement.Remarks,
                    Status = addTrnSoldUnitRequirement.Status,
                    StatusDate = Convert.ToDateTime(addTrnSoldUnitRequirement.StatusDate)
                };

                db.TrnSoldUnitRequirements.InsertOnSubmit(newTrnSoldUnitRequirement);
                db.SubmitChanges();

                return newTrnSoldUnitRequirement.Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        //Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteTrnSoldUnitRequirement(string id)
        {
            try
            {
                var TrnSoldUnitRequirementData = from d in db.TrnSoldUnitRequirements where d.Id == Convert.ToInt32(id) select d;
                if (TrnSoldUnitRequirementData.Any())
                {
                    db.TrnSoldUnitRequirements.DeleteOnSubmit(TrnSoldUnitRequirementData.First());
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

        //Lock
        [HttpPut, Route("Update/{id}")]
        public HttpResponseMessage UpdateSoldUnitRequirement(string id, TrnSoldUnitRequirement UpdateTrnSoldUnitRequirement)
        {
            try
            {
                var TrnSoldUnitRequirementData = from d in db.TrnSoldUnitRequirements where d.Id == Convert.ToInt32(id) select d;
                if (TrnSoldUnitRequirementData.Any())
                {
                    var UpdateTrnSoldUnitRequirementData = TrnSoldUnitRequirementData.FirstOrDefault();

                    UpdateTrnSoldUnitRequirementData.SoldUnitId = UpdateTrnSoldUnitRequirement.SoldUnitId;
                    UpdateTrnSoldUnitRequirementData.CheckListRequirementId = UpdateTrnSoldUnitRequirement.CheckListRequirementId;
                    UpdateTrnSoldUnitRequirementData.Attachment1 = UpdateTrnSoldUnitRequirement.Attachment1;
                    UpdateTrnSoldUnitRequirementData.Attachment2 = UpdateTrnSoldUnitRequirement.Attachment2;
                    UpdateTrnSoldUnitRequirementData.Attachment3 = UpdateTrnSoldUnitRequirement.Attachment3;
                    UpdateTrnSoldUnitRequirementData.Attachment4 = UpdateTrnSoldUnitRequirement.Attachment4;
                    UpdateTrnSoldUnitRequirementData.Attachment5 = UpdateTrnSoldUnitRequirement.Attachment5;
                    UpdateTrnSoldUnitRequirementData.Remarks = UpdateTrnSoldUnitRequirement.Remarks;
                    UpdateTrnSoldUnitRequirementData.Status = UpdateTrnSoldUnitRequirement.Status;
                    UpdateTrnSoldUnitRequirementData.StatusDate = Convert.ToDateTime(UpdateTrnSoldUnitRequirement.StatusDate);

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
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
