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

        // list 
        [HttpGet, Route("ListPerChecklist/{id}")]
        public List<MstCheckListRequirement> GetMstChecklistRequirementsPerChecklist(string id)
        {
            var MstCheckListRequirementData = from d in db.MstCheckListRequirements
                                              where d.CheckListId == Convert.ToInt32(id)
                                              select new MstCheckListRequirement
                                              {
                                                  Id = d.Id,
                                                  ChecklistId = d.CheckListId,
                                                  Checklist = d.MstCheckList.CheckList,
                                                  RequirementNo = d.RequirementNo,
                                                  Requirement = d.Requirement,
                                                  Category = d.Category,
                                                  Type = d.Type,
                                                  WithAttachments = d.WithAttachments
                                              };
            return MstCheckListRequirementData.ToList();
        }

        // detail
        [HttpGet, Route("Detail/{id}")]
        public MstCheckListRequirement GetMstCheckListRequirement(string id)
        {
            var MstCheckListRequirementData = from d in db.MstCheckListRequirements
                                              where d.Id == Convert.ToInt32(id)
                                              select new MstCheckListRequirement
                                              {
                                                  Id = d.Id,
                                                  ChecklistId = d.CheckListId,
                                                  Checklist = d.MstCheckList.CheckList,
                                                  RequirementNo = d.RequirementNo,
                                                  Requirement = d.Requirement,
                                                  Category = d.Category,
                                                  Type = d.Type,
                                                  WithAttachments = d.WithAttachments
                                              };
            return MstCheckListRequirementData.FirstOrDefault();
        }

        // add
        [HttpPost, Route("Add")]
        public Int32 PostMstCheckListRequirement(MstCheckListRequirement checklistRequirement)
        {
            try
            {
                Data.MstCheckListRequirement newMstCheckListRequirement = new Data.MstCheckListRequirement()
                {
                    CheckListId = checklistRequirement.ChecklistId,
                    RequirementNo = checklistRequirement.RequirementNo,
                    Requirement = checklistRequirement.Requirement,
                    Category = checklistRequirement.Category,
                    Type = checklistRequirement.Type,
                    WithAttachments = checklistRequirement.WithAttachments
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

        // delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstCheckListRequirement(string id)
        {
            try
            {
                var MstCheckListRequirementData = from d in db.MstCheckListRequirements 
                                                  where d.Id == Convert.ToInt32(id) select d;

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

        // save
        [HttpPut, Route("Save")]
        public HttpResponseMessage UpdateCheckListRequirements(MstCheckListRequirement checklistRequirement)
        {
            try
            {
                var MstCheckListRequirementsData = from d in db.MstCheckListRequirements
                                                   where d.Id == Convert.ToInt32(checklistRequirement.Id)
                                                   select d;

                if (MstCheckListRequirementsData.Any())
                {

                    var UpdateCheckListRequirementsData = MstCheckListRequirementsData.FirstOrDefault();

                    UpdateCheckListRequirementsData.RequirementNo = checklistRequirement.RequirementNo;
                    UpdateCheckListRequirementsData.Requirement = checklistRequirement.Requirement;
                    UpdateCheckListRequirementsData.Category = checklistRequirement.Category;
                    UpdateCheckListRequirementsData.Type = checklistRequirement.Type;
                    UpdateCheckListRequirementsData.WithAttachments = checklistRequirement.WithAttachments;

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