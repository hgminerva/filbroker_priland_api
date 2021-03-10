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
    [RoutePrefix("api/MstUserRight")]
    public class MstUserRightController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        // List 
        [HttpGet, Route("List")]
        public List<MstUserRight> GetMstUserRights()
        {
            var MstUserRightData = from d in db.MstUserRights
                                   select new Models.MstUserRight
                                   {

                                       Id = d.Id,
                                       UserId = d.UserId,
                                       PageId = d.PageId,
                                       Page = d.SysPage.Page,
                                       PageURL = d.SysPage.Url,
                                       CanEdit = d.CanEdit,
                                       CanSave = d.CanSave,
                                       CanLock = d.CanLock,
                                       CanUnLock = d.CanUnlock,
                                       CanPrint = d.CanPrint,
                                       CanDelete = d.CanDelete
                                   };

            return MstUserRightData.ToList();
        }

        [HttpGet, Route("ListPerUser/{id}")]
        public List<MstUserRight> GetMstUserRightsPerUser(string id)
        {
            var MstUserRightData = from d in db.MstUserRights
                                   where d.UserId == Convert.ToInt32(id)
                                   select new Models.MstUserRight
                                   {
                                       Id = d.Id,
                                       UserId = d.UserId,
                                       PageId = d.PageId,
                                       Page = d.SysPage.Page,
                                       PageURL = d.SysPage.Url,
                                       CanEdit = d.CanEdit,
                                       CanSave = d.CanSave,
                                       CanLock = d.CanLock,
                                       CanUnLock = d.CanUnlock,
                                       CanPrint = d.CanPrint,
                                       CanDelete = d.CanDelete
                                   };
            return MstUserRightData.ToList();
        }

        [HttpGet, Route("ListPerUserByUsername/{username}")]
        public List<MstUserRight> GetMstUserRightsPerUserByUsername(string username)
        {
            var MstUserRightData = from d in db.MstUserRights
                                   where d.MstUser.Username == username
                                   select new Models.MstUserRight
                                   {
                                       Id = d.Id,
                                       UserId = d.UserId,
                                       PageId = d.PageId,
                                       Page = d.SysPage.Page,
                                       PageURL = d.SysPage.Url,
                                       CanEdit = d.CanEdit,
                                       CanSave = d.CanSave,
                                       CanLock = d.CanLock,
                                       CanUnLock = d.CanUnlock,
                                       CanPrint = d.CanPrint,
                                       CanDelete = d.CanDelete
                                   };
            return MstUserRightData.ToList();
        }

        [HttpGet, Route("ListPerCurrentUser/{page}")]
        public MstUserRight GetMstUserRightsPerCurrentUser(string page)
        {
            var currentUser = from d in db.MstUsers
                              where d.AspNetId == User.Identity.GetUserId()
                              select d;

            var MstUserRightData = from d in db.MstUserRights
                                   where d.SysPage.Page == page
                                   && d.UserId == currentUser.FirstOrDefault().Id
                                   select new Models.MstUserRight
                                   {
                                       Id = d.Id,
                                       UserId = d.UserId,
                                       PageId = d.PageId,
                                       Page = d.SysPage.Page,
                                       PageURL = d.SysPage.Url,
                                       CanEdit = d.CanEdit,
                                       CanSave = d.CanSave,
                                       CanLock = d.CanLock,
                                       CanUnLock = d.CanUnlock,
                                       CanPrint = d.CanPrint,
                                       CanDelete = d.CanDelete
                                   };

            return MstUserRightData.FirstOrDefault();
        }

        // Detail
        [HttpGet, Route("Detail/{id}")]
        public MstUserRight GetMstUserRightId(string id)
        {
            var MstUserRightData = from d in db.MstUserRights
                                   where d.Id == Convert.ToInt32(id)
                                   select new Models.MstUserRight
                                   {
                                       Id = d.Id,
                                       UserId = d.UserId,
                                       PageId = d.PageId,
                                       Page = d.SysPage.Page,
                                       CanEdit = d.CanEdit,
                                       CanSave = d.CanSave,
                                       CanLock = d.CanLock,
                                       CanUnLock = d.CanUnlock,
                                       CanPrint = d.CanPrint,
                                       CanDelete = d.CanDelete
                                   };
            return MstUserRightData.FirstOrDefault();
        }

        // Add
        [HttpPost, Route("Add")]
        public Int32 PostMstUserRight(MstUserRight userRight)
        {
            try
            {
                Data.MstUserRight newMstUserRight = new Data.MstUserRight()
                {
                    UserId = userRight.UserId,
                    PageId = userRight.PageId,
                    CanEdit = userRight.CanEdit,
                    CanSave = userRight.CanSave,
                    CanLock = userRight.CanLock,
                    CanUnlock = userRight.CanLock,
                    CanPrint = userRight.CanPrint,
                    CanDelete = userRight.CanDelete
                };

                db.MstUserRights.InsertOnSubmit(newMstUserRight);
                db.SubmitChanges();

                return newMstUserRight.Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // Save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveMstUserRight(MstUserRight userRight)
        {
            try
            {
                var MstUserRightData = from d in db.MstUserRights
                                       where d.Id == Convert.ToInt32(userRight.Id)
                                       select d;

                if (MstUserRightData.Any())
                {
                    var UpdateMstUserRightData = MstUserRightData.FirstOrDefault();

                    UpdateMstUserRightData.CanEdit = userRight.CanEdit;
                    UpdateMstUserRightData.CanSave = userRight.CanSave;
                    UpdateMstUserRightData.CanLock = userRight.CanLock;
                    UpdateMstUserRightData.CanUnlock = userRight.CanUnLock;
                    UpdateMstUserRightData.CanPrint = userRight.CanPrint;
                    UpdateMstUserRightData.CanDelete = userRight.CanDelete;

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

        //Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstUserRight(string id)
        {
            try
            {
                var MstUserRightData = from d in db.MstUserRights
                                       where d.Id == Convert.ToInt32(id)
                                       select d;

                if (MstUserRightData.Any())
                {
                    db.MstUserRights.DeleteOnSubmit(MstUserRightData.First());
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
    }
}
