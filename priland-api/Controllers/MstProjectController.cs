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

        private String padNumWithZero(Int32 number, Int32 length)
        {
            var result = number.ToString();
            var pad = length - result.Length;
            while (pad > 0)
            {
                result = '0' + result;
                pad--;
            }

            return result;
        }

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
        public MstProject GetMstProjectId(string id)
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

        //Add
        [HttpPost, Route("Add")]
        public Int32 PostMstProject(MstProject project)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    string projectCode = "0001";
                    var projects = from d in db.MstProjects.OrderByDescending(d => d.Id) select d;
                    if (projects.Any())
                    {
                        Int32 nextProjectCode = Convert.ToInt32(projects.FirstOrDefault().ProjectCode) + 1;
                        projectCode = padNumWithZero(nextProjectCode, 4);
                    }

                    Data.MstProject newMstProject = new Data.MstProject()
                    {
                        ProjectCode = projectCode,
                        Project = project.Project,
                        Address = project.Address,
                        Status = project.Status,
                        IsLocked = project.IsLocked,
                        CreatedBy = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedBy = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now
                    };

                    db.MstProjects.InsertOnSubmit(newMstProject);
                    db.SubmitChanges();

                    return newMstProject.Id;
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
        public HttpResponseMessage DeleteMstProject(string id)
        {
            try
            {
                var MstProjectData = from d in db.MstProjects where d.Id == Convert.ToInt32(id) select d;
                if (MstProjectData.Any())
                {
                    if (MstProjectData.FirstOrDefault().IsLocked == false)
                    {
                        db.MstProjects.DeleteOnSubmit(MstProjectData.FirstOrDefault());
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
        public HttpResponseMessage SaveMstProject(MstProject project)
        {
            try
            {
                var MstProjectData = from d in db.MstProjects where d.Id == Convert.ToInt32(project.Id) select d;
                if (MstProjectData.Any())
                {
                    if (MstProjectData.FirstOrDefault().IsLocked == false)
                    {
                        var currentUser = from d in db.MstUsers
                                          where d.AspNetId == User.Identity.GetUserId()
                                          select d;

                        if (currentUser.Any())
                        {
                            var UpdateProjectData = MstProjectData.FirstOrDefault();

                            UpdateProjectData.ProjectCode = project.ProjectCode;
                            UpdateProjectData.Project = project.Project;
                            UpdateProjectData.Address = project.Address;
                            UpdateProjectData.Status = project.Status;
                            UpdateProjectData.UpdatedBy = currentUser.FirstOrDefault().Id;
                            UpdateProjectData.UpdatedDateTime = DateTime.Now;

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
        public HttpResponseMessage LockMstProject(MstProject project)
        {
            try
            {
                var MstProjectData = from d in db.MstProjects where d.Id == Convert.ToInt32(project.Id) select d;
                if (MstProjectData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                        where d.AspNetId == User.Identity.GetUserId()
                                        select d;
                    if (currentUser.Any())
                    {
                        var UpdateProjectData = MstProjectData.FirstOrDefault();

                        UpdateProjectData.ProjectCode = project.ProjectCode;
                        UpdateProjectData.Project = project.Project;
                        UpdateProjectData.Address = project.Address;
                        UpdateProjectData.Status = project.Status;
                        UpdateProjectData.IsLocked = true;
                        UpdateProjectData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateProjectData.UpdatedDateTime = DateTime.Now;

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
        public HttpResponseMessage UnLockMstProject(MstProject project)
        {
            try
            {
                var MstProjectData = from d in db.MstProjects where d.Id == Convert.ToInt32(project.Id) select d;
                if (MstProjectData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                        where d.AspNetId == User.Identity.GetUserId()
                                        select d;

                    if (currentUser.Any())
                    {
                        var UpdateProjectData = MstProjectData.FirstOrDefault();

                        UpdateProjectData.IsLocked = false;
                        UpdateProjectData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateProjectData.UpdatedDateTime = DateTime.Now;

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
