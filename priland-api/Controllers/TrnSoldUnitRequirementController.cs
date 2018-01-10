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

        // List
        [HttpGet, Route("List")]
        public List<TrnSoldUnitRequirement> GetTrnSoldUnitRequirements()
        {
            var TrnSoldUnitRequirementData = from d in db.TrnSoldUnitRequirements
                                             select new TrnSoldUnitRequirement
                                             {
                                                 Id = d.Id,
                                                 SoldUnitId = d.SoldUnitId,
                                                 ChecklistRequirementId = d.CheckListRequirementId,
                                                 ChecklistRequirement = d.MstCheckListRequirement.Requirement,
                                                 ChecklistRequirementNo = d.MstCheckListRequirement.RequirementNo,
                                                 ChecklistCategory = d.MstCheckListRequirement.Category,
                                                 ChecklistType = d.MstCheckListRequirement.Type,
                                                 ChecklistWithAttachments = d.MstCheckListRequirement.WithAttachments,
                                                 Attachment1 = d.Attachment1,
                                                 Attachment2 = d.Attachment2,
                                                 Attachment3 = d.Attachment3,
                                                 Attachment4 = d.Attachment4,
                                                 Attachment5 = d.Attachment5,
                                                 Remarks = d.Remarks,
                                                 Status = d.Status,
                                                 StatusDate = d.StatusDate.ToShortDateString()
                                             };
            return TrnSoldUnitRequirementData.ToList();
        }

        // List
        [HttpGet, Route("ListPerUnitSold/{id}")]
        public List<TrnSoldUnitRequirement> GetTrnSoldUnitRequirementsPerUnitSold(string id)
        {
            var TrnSoldUnitRequirementData = from d in db.TrnSoldUnitRequirements
                                             where d.SoldUnitId ==  Convert.ToInt32(id)
                                             orderby d.MstCheckListRequirement.RequirementNo
                                             select new TrnSoldUnitRequirement
                                             {
                                                 Id = d.Id,
                                                 SoldUnitId = d.SoldUnitId,
                                                 ChecklistRequirementId = d.CheckListRequirementId,
                                                 ChecklistRequirement = d.MstCheckListRequirement.Requirement,
                                                 ChecklistRequirementNo = d.MstCheckListRequirement.RequirementNo,
                                                 ChecklistCategory = d.MstCheckListRequirement.Category,
                                                 ChecklistType = d.MstCheckListRequirement.Type,
                                                 ChecklistWithAttachments = d.MstCheckListRequirement.WithAttachments,
                                                 Attachment1 = d.Attachment1,
                                                 Attachment2 = d.Attachment2,
                                                 Attachment3 = d.Attachment3,
                                                 Attachment4 = d.Attachment4,
                                                 Attachment5 = d.Attachment5,
                                                 Remarks = d.Remarks,
                                                 Status = d.Status,
                                                 StatusDate = d.StatusDate.ToShortDateString()
                                             };
            return TrnSoldUnitRequirementData.ToList();
        }

        // Detail
        [HttpGet, Route("Detail/{id}")]
        public TrnSoldUnitRequirement GetTrnSoldUnitRequirementId(string id)
        {
            var TrnSoldUnitRequirementData = from d in db.TrnSoldUnitRequirements
                                             where d.Id == Convert.ToInt32(id)
                                             select new TrnSoldUnitRequirement
                                             {
                                                 Id = d.Id,
                                                 SoldUnitId = d.SoldUnitId,
                                                 ChecklistRequirementId = d.CheckListRequirementId,
                                                 ChecklistRequirement = d.MstCheckListRequirement.Requirement,
                                                 ChecklistRequirementNo = d.MstCheckListRequirement.RequirementNo,
                                                 ChecklistCategory = d.MstCheckListRequirement.Category,
                                                 ChecklistType = d.MstCheckListRequirement.Type,
                                                 ChecklistWithAttachments = d.MstCheckListRequirement.WithAttachments,
                                                 Attachment1 = d.Attachment1,
                                                 Attachment2 = d.Attachment2,
                                                 Attachment3 = d.Attachment3,
                                                 Attachment4 = d.Attachment4,
                                                 Attachment5 = d.Attachment5,
                                                 Remarks = d.Remarks,
                                                 Status = d.Status,
                                                 StatusDate = d.StatusDate.ToShortDateString()
                                             };
            return (TrnSoldUnitRequirement)TrnSoldUnitRequirementData.FirstOrDefault();
        }

        // New Sold Unit requirements
        [HttpGet, Route("ListNewTrnSoldUnitRequirements/{soldUnitId}/{checklistId}")]
        public List<TrnSoldUnitRequirement> GetNewTrnSoldUnitRequirements(string soldUnitId, string checklistId)
        {
            // Remove existing requirements
            var deleteRequirements = from d in db.TrnSoldUnitRequirements 
                                     where d.SoldUnitId == Convert.ToInt32(soldUnitId) select d;

            if (deleteRequirements.Any())
            {
                foreach (var deleteRequirement in deleteRequirements)
                {
                    db.TrnSoldUnitRequirements.DeleteOnSubmit(deleteRequirement);
                }
                db.SubmitChanges();
            }

            // Insert new requirements
            var checklistRequirements = from d in db.MstCheckListRequirements
                                        where d.CheckListId == Convert.ToInt32(checklistId)
                                        select d;

            if (checklistRequirements.Any())
            {
                foreach (var checklistRequirement in checklistRequirements)
                {
                    Data.TrnSoldUnitRequirement insertRequirement = new Data.TrnSoldUnitRequirement()
                    {
                        SoldUnitId = Convert.ToInt32(soldUnitId),
                        CheckListRequirementId = checklistRequirement.Id,
                        Remarks = "",
                        Status = "NOT OK",
                        StatusDate = DateTime.Now
                    };

                    db.TrnSoldUnitRequirements.InsertOnSubmit(insertRequirement);
                }
                db.SubmitChanges();
            }

            // Return new requirements
            var newRequirements = from d in db.TrnSoldUnitRequirements
                                  where d.SoldUnitId == Convert.ToInt32(soldUnitId)
                                  select new TrnSoldUnitRequirement
                                  {
                                        Id = d.Id,
                                        SoldUnitId = d.SoldUnitId,
                                        ChecklistRequirementId = d.CheckListRequirementId,
                                        ChecklistRequirement = d.MstCheckListRequirement.Requirement,
                                        ChecklistRequirementNo = d.MstCheckListRequirement.RequirementNo,
                                        ChecklistCategory = d.MstCheckListRequirement.Category,
                                        ChecklistType = d.MstCheckListRequirement.Type,
                                        ChecklistWithAttachments = d.MstCheckListRequirement.WithAttachments,
                                        Attachment1 = d.Attachment1,
                                        Attachment2 = d.Attachment2,
                                        Attachment3 = d.Attachment3,
                                        Attachment4 = d.Attachment4,
                                        Attachment5 = d.Attachment5,
                                        Remarks = d.Remarks,
                                        Status = d.Status,
                                        StatusDate = d.StatusDate.ToShortDateString()
                                  };

            return newRequirements.ToList();
        }

        // Save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveTrnSoldUnitRequirement(TrnSoldUnitRequirement soldUnitRequirement)
        {
            try
            {
                var soldUnitRequirements = from d in db.TrnSoldUnitRequirements
                                           where d.Id == Convert.ToInt32(soldUnitRequirement.Id)
                                           select d;

                if (soldUnitRequirements.Any())
                {
                    var updateSoldUnitRequirement = soldUnitRequirements.FirstOrDefault();

                    updateSoldUnitRequirement.Attachment1 = soldUnitRequirement.Attachment1;
                    updateSoldUnitRequirement.Attachment2 = soldUnitRequirement.Attachment2;
                    updateSoldUnitRequirement.Attachment3 = soldUnitRequirement.Attachment3;
                    updateSoldUnitRequirement.Attachment4 = soldUnitRequirement.Attachment4;
                    updateSoldUnitRequirement.Attachment5 = soldUnitRequirement.Attachment5;
                    updateSoldUnitRequirement.Remarks = soldUnitRequirement.Remarks;

                    if (updateSoldUnitRequirement.Status != soldUnitRequirement.Status)
                    {
                        updateSoldUnitRequirement.Status = soldUnitRequirement.Status;
                        updateSoldUnitRequirement.StatusDate = DateTime.Now;
                    }

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
