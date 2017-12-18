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

        //List
        [HttpGet, Route("List")]
        public List<MstUserRight> GetMstUserRight(string id)
        {
            var MstUserRightData = from d in db.MstUserRights
                                   where d.UserId == Convert.ToInt32(id)
                                   select new Models.MstUserRight
                                   {
                                       Id = d.Id,
                                       UserId = d.UserId,
                                       PageId = d.PageId,
                                       CanEdit = d.CanEdit,
                                       CanSave = d.CanSave,
                                       CanLock = d.CanLock,
                                       CanUnLock = d.CanUnlock,
                                       CanPrint = d.CanPrint
                                   };
            return MstUserRightData.ToList();
        }

        //Detail
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
                                       CanEdit = d.CanEdit,
                                       CanSave = d.CanSave,
                                       CanLock = d.CanLock,
                                       CanUnLock = d.CanUnlock,
                                       CanPrint = d.CanPrint
                                   };
            return (MstUserRight)MstUserRightData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostMstUserRight(MstUserRight addMstUserRight)
        {
            try
            {
                Data.MstUserRight newMstUserRight = new Data.MstUserRight()
                {
                    UserId = addMstUserRight.UserId,
                    PageId = addMstUserRight.PageId,
                    CanEdit = addMstUserRight.CanEdit,
                    CanSave = addMstUserRight.CanSave,
                    CanLock = addMstUserRight.CanLock,
                    CanUnlock = addMstUserRight.CanLock,
                    CanPrint = addMstUserRight.CanPrint
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

        //Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstUserRight(string id)
        {
            try
            {
                var MstUserRightData = from d in db.MstUserRights where d.Id == Convert.ToInt32(id) select d;
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

        //Lock
        [HttpPut, Route("Update/{id}")]
        public HttpResponseMessage UpdateUserRight(string id, MstUserRight UpdateMstUserRight)
        {
            try
            {
                var MstUserRightData = from d in db.MstUserRights where d.Id == Convert.ToInt32(id) select d;
                if (MstUserRightData.Any())
                {
                    var UpdateMstUserRightData = MstUserRightData.FirstOrDefault();

                    UpdateMstUserRightData.UserId = UpdateMstUserRight.UserId;
                    UpdateMstUserRightData.PageId = UpdateMstUserRight.PageId;
                    UpdateMstUserRightData.CanEdit = UpdateMstUserRight.CanEdit;
                    UpdateMstUserRightData.CanSave = UpdateMstUserRight.CanSave;
                    UpdateMstUserRightData.CanLock = UpdateMstUserRight.CanLock;
                    UpdateMstUserRightData.CanUnlock = UpdateMstUserRight.CanUnLock;
                    UpdateMstUserRightData.CanPrint = UpdateMstUserRight.CanPrint;

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
