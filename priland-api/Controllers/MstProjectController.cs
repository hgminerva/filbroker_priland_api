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
    [RoutePrefix("api/MstProject")]
    public class MstProjectController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();
         
        //List
        [HttpGet, Route("List")]
        public List<Models.MstProject> GetMstProject()
        {
            var MstProjectData = from d in db.MstProjects
                                 select new Models.MstProject
                                 {
                                     Id = d.Id,
                                     ProjectCode = d.ProjectCode,
                                     Project = d.Project,
                                     Address = d.Address,
                                     Status = d.Status,
                                     IsLocked = d.IsLocked,
                                     CreatedBy = d.CreatedBy,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedBy = d.UpdatedBy,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return MstProjectData.ToList();
        }

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public Models.MstProject GetMstProjectId(string id)
        {
            var MstProjectData = from d in db.MstProjects
                                 where d.Id == Convert.ToInt32(id)
                                 select new Models.MstProject
                                 {
                                     Id = d.Id,
                                     ProjectCode = d.ProjectCode,
                                     Project = d.Project,
                                     Address = d.Address,
                                     Status = d.Status,
                                     IsLocked = d.IsLocked,
                                     CreatedBy = d.CreatedBy,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedBy = d.UpdatedBy,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return (Models.MstProject)MstProjectData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostMstProject(MstProject addMstProject)
        {
            try
            {
                Data.MstProject newMstProject = new Data.MstProject()
                {
                    ProjectCode = addMstProject.ProjectCode,
                    Project = addMstProject.Project,
                    Address = addMstProject.Address,
                    Status = addMstProject.Status,
                    IsLocked = addMstProject.IsLocked,
                    CreatedBy = addMstProject.CreatedBy,
                    CreatedDateTime = Convert.ToDateTime(addMstProject.CreatedDateTime),
                    UpdatedBy = addMstProject.UpdatedBy,
                    UpdatedDateTime = Convert.ToDateTime(addMstProject.UpdatedDateTime)
                };

                db.MstProjects.InsertOnSubmit(newMstProject);
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
        public HttpResponseMessage DeleteMstProject(string id)
        {
            try
            {
                var MstProjectData = from d in db.MstProjects where d.Id == Convert.ToInt32(id) select d;
                if (MstProjectData.Any())
                {
                    if (!MstProjectData.First().IsLocked)
                    {
                        db.MstProjects.DeleteOnSubmit(MstProjectData.First());
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
        public HttpResponseMessage UpdateProject(string id, Models.MstProject UpdateMstProject)
        {
            try
            {
                var MstProjectData = from d in db.MstProjects where d.Id == Convert.ToInt32(id) select d;
                if (MstProjectData.Any())
                {
                    if (!MstProjectData.First().IsLocked)
                    {
                        var UpdateProjectData = MstProjectData.FirstOrDefault();

                        UpdateProjectData.ProjectCode = UpdateMstProject.ProjectCode;
                        UpdateProjectData.Project = UpdateMstProject.Project;
                        UpdateProjectData.Address = UpdateMstProject.Address;
                        UpdateProjectData.Status = UpdateMstProject.Status;
                        UpdateProjectData.IsLocked = true;
                        UpdateProjectData.CreatedBy = UpdateMstProject.CreatedBy;
                        UpdateProjectData.CreatedDateTime = Convert.ToDateTime(UpdateMstProject.CreatedDateTime);
                        UpdateProjectData.UpdatedBy = UpdateMstProject.UpdatedBy;
                        UpdateProjectData.UpdatedDateTime = Convert.ToDateTime(UpdateMstProject.UpdatedDateTime);

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
        public HttpResponseMessage UnLock(string id, Models.MstProject UnLockMstProject)
        {
            try
            {
                var MstProjectData = from d in db.MstProjects where d.Id == Convert.ToInt32(id) select d;
                if (MstProjectData.Any())
                {
                    if (MstProjectData.First().IsLocked)
                    {
                        var UnLockProjectData = MstProjectData.FirstOrDefault();

                        UnLockProjectData.IsLocked = false;

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
