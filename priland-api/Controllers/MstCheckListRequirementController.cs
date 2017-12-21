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
    [RoutePrefix("api/MstCheckListRequirement")]
    public class MstCheckListRequirementController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        //List 
        [HttpGet, Route("List")]
        public List<Models.MstCheckListRequirement> GetMstCheckListRequirement(string id)
        {
            var MstCheckListRequirementData = from d in db.MstCheckListRequirements
                                              where d.CheckListId == Convert.ToInt32(id)
                                              select new Models.MstCheckListRequirement
                                              {
                                                  Id = d.Id,
                                                  RequirementNo = d.RequirementNo,
                                                  Requirement = d.Requirement,
                                                  Category = d.Category,
                                                  Type = d.Type,
                                                  WithAttachments = d.WithAttachments
                                              };
            return MstCheckListRequirementData.ToList();
        }

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public Models.MstCheckListRequirement GetMstCheckListRequirementId(string id)
        {
            var MstCheckListRequirementData = from d in db.MstCheckListRequirements
                                              where d.Id == Convert.ToInt32(id)
                                              select new Models.MstCheckListRequirement
                                              {
                                                  Id = d.Id,
                                                  CheckListId = d.CheckListId,
                                                  RequirementNo = d.RequirementNo,
                                                  Requirement = d.Requirement,
                                                  Category = d.Category,
                                                  Type = d.Type,
                                                  WithAttachments = d.WithAttachments
                                              };
            return (Models.MstCheckListRequirement)MstCheckListRequirementData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostMstCheckListRequirement(MstCheckListRequirement addPostMstCheckListRequirement)
        {
            try
            {
                Data.MstCheckListRequirement newMstCheckListRequirement = new Data.MstCheckListRequirement()
                {
                    Id = addPostMstCheckListRequirement.Id,
                    CheckListId = addPostMstCheckListRequirement.CheckListId,
                    RequirementNo = addPostMstCheckListRequirement.RequirementNo,
                    Requirement = addPostMstCheckListRequirement.Requirement,
                    Category = addPostMstCheckListRequirement.Category,
                    Type = addPostMstCheckListRequirement.Type,
                    WithAttachments = addPostMstCheckListRequirement.WithAttachments
                };

                db.MstCheckListRequirements.InsertOnSubmit(newMstCheckListRequirement);
                db.SubmitChanges();

                return newMstCheckListRequirement.Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }

        }

        //Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstCheckListRequirement(string id)
        {
            try
            {
                var MstCheckListRequirementData = from d in db.MstCheckListRequirements where d.Id == Convert.ToInt32(id) select d;
                if (MstCheckListRequirementData.Any())
                {
                    db.MstCheckListRequirements.DeleteOnSubmit(MstCheckListRequirementData.First());
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

        //UPDATE
        [HttpPut, Route("Update/{id}")]
        public HttpResponseMessage UpdateCheckListRequirements(string id, MstCheckListRequirement UpdateMstCheckListRequirements)
        {
            try
            {
                var MstCheckListRequirementsData = from d in db.MstCheckListRequirements where d.Id == Convert.ToInt32(id) select d;
                if (MstCheckListRequirementsData.Any())
                {

                    var UpdateCheckListRequirementsData = MstCheckListRequirementsData.FirstOrDefault();

                    UpdateCheckListRequirementsData.CheckListId = UpdateMstCheckListRequirements.CheckListId;
                    UpdateCheckListRequirementsData.RequirementNo = UpdateMstCheckListRequirements.RequirementNo;
                    UpdateCheckListRequirementsData.Requirement = UpdateMstCheckListRequirements.Requirement;
                    UpdateCheckListRequirementsData.Category = UpdateMstCheckListRequirements.Category;
                    UpdateCheckListRequirementsData.Type = UpdateMstCheckListRequirements.Type;
                    UpdateCheckListRequirementsData.WithAttachments = UpdateMstCheckListRequirements.WithAttachments;

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