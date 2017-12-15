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
    public class MstProjectController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        [HttpGet,Route("api/MstProject/List")]
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
                                     CreatedBy = d.CreatedBy,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString() ,
                                     UpdatedBy = d.UpdatedBy,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return MstProjectData.ToList();
        }

        [HttpGet, Route("api/MstProject/Detail/{id}")]
        public Models.MstProject GetMstProjectId(string id)
        {
            var MstProjectData = from d in db.MstProjects
                                 where d.Id==Convert.ToInt32(id)
                                 select new Models.MstProject
                                 {
                                     Id = d.Id,
                                     ProjectCode = d.ProjectCode,
                                     Project = d.Project,
                                     Address = d.Address,
                                     Status = d.Status,
                                     CreatedBy = d.CreatedBy,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedBy = d.UpdatedBy,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return (Models.MstProject)MstProjectData.FirstOrDefault();
        }

        [HttpPost,Route("api/MstProject/Add")]
        public Int32  PostMstProject()
        {
            try
            {
                Data.MstProject newMstProject = new Data.MstProject()
                {
                    ProjectCode = "NA",
                    Project = "NA",
                    Address ="NA",
                    Status = "NA",
                    CreatedBy= 1,
                    CreatedDateTime = DateTime.Today ,
                    UpdatedBy = 1,
                    UpdatedDateTime = DateTime.Today
                };

                db.MstProjects.InsertOnSubmit(newMstProject);
                db.SubmitChanges();

                return newMstProject.Id;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        [HttpDelete,Route("api/MstProject/Delete/{id}")]
        public HttpResponseMessage DeleteMstProject(string id)
        {
            try
            {
                var MstProjectData = from d in db.MstProjects where d.Id == Convert.ToInt32(id) select d;
                if (MstProjectData.Any())
                {
                    db.MstProjects.DeleteOnSubmit(MstProjectData.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut,Route("api/MstProject/Update/{id}")]
        public HttpResponseMessage UpdateProject(string id,Models.MstProject UpdateMstProject)
        {
            try
            {
                var MstProjectData = from d in db.MstProjects where d.Id == Convert.ToInt32(id) select d;
                if (MstProjectData.Any())
                {
                    var UpdateProjectData = MstProjectData.FirstOrDefault();

                    UpdateProjectData.ProjectCode = UpdateMstProject.ProjectCode;
                    UpdateProjectData.Project = UpdateMstProject.Project;
                    UpdateProjectData.Address = UpdateMstProject.Address;
                    UpdateProjectData.Status = UpdateMstProject.Status;
                    UpdateProjectData.CreatedBy = UpdateMstProject.CreatedBy;
                    UpdateProjectData.CreatedDateTime =Convert.ToDateTime( UpdateMstProject.CreatedDateTime);
                    UpdateProjectData.UpdatedBy = UpdateMstProject.UpdatedBy;
                    UpdateProjectData.UpdatedDateTime = Convert.ToDateTime( UpdateMstProject.UpdatedDateTime);

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
