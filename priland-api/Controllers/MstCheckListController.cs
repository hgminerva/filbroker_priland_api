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
    [RoutePrefix("api/MstCheckList")]
    public class MstCheckListController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        //List
        [HttpGet, Route("List")]
        public List<Models.MstChecList> GetMstCheckList()
        {
            var MstCheckListData = from d in db.MstCheckLists
                                 select new Models.MstChecList
                                 {
                                     Id = d.Id,
                                     CheckListCode=d.CheckListCode,
                                     CheckList=d.CheckList,
                                     CheckListDate=d.CheckListDate.ToShortDateString(),
                                     ProjectId=d.ProjectId,
                                     Remarks=d.Remarks,
                                     Status = d.Status,
                                     IsLocked = d.IsLocked,
                                     CreatedBy = d.CreatedBy,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedBy = d.UpdatedBy,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return MstCheckListData.ToList();
        }

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public Models.MstChecList GetMstChecList(string id)
        {
            var MstChecListData = from d in db.MstCheckLists
                                 where d.Id == Convert.ToInt32(id)
                                 select new Models.MstChecList
                                 {
                                     Id = d.Id,
                                     CheckListCode = d.CheckListCode,
                                     CheckList = d.CheckList,
                                     CheckListDate = d.CheckListDate.ToShortDateString(),
                                     ProjectId = d.ProjectId,
                                     Remarks = d.Remarks,
                                     Status = d.Status,
                                     IsLocked = d.IsLocked,
                                     CreatedBy = d.CreatedBy,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedBy = d.UpdatedBy,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return (Models.MstChecList)MstChecListData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostMstCheckList(MstChecList addMstChecList)
        {
            try
            {
                Data.MstCheckList newMstChecList = new Data.MstCheckList()
                {
                    CheckListCode = addMstChecList.CheckListCode,
                    CheckList = addMstChecList.CheckList,
                    CheckListDate = Convert.ToDateTime(addMstChecList.CheckListDate),
                    ProjectId = addMstChecList.ProjectId,
                    Remarks = addMstChecList.Remarks,
                    Status = addMstChecList.Status,
                    IsLocked = addMstChecList.IsLocked,
                    CreatedBy = addMstChecList.CreatedBy,
                    CreatedDateTime = Convert.ToDateTime(addMstChecList.CreatedDateTime),
                    UpdatedBy = addMstChecList.UpdatedBy,
                    UpdatedDateTime = Convert.ToDateTime(addMstChecList.UpdatedDateTime)
                };

                db.MstCheckLists.InsertOnSubmit(newMstChecList);
                db.SubmitChanges();

                return newMstChecList.Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        //Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstCheckList(string id)
        {
            try
            {
                var MstCheckListData = from d in db.MstCheckLists where d.Id == Convert.ToInt32(id) select d;
                if (MstCheckListData.Any())
                {
                    if (!MstCheckListData.First().IsLocked)
                    {
                        db.MstCheckLists.DeleteOnSubmit(MstCheckListData.First());
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
        public HttpResponseMessage UpdateCheckList(string id, Models.MstChecList UpdateMstCheckList)
        {
            try
            {
                var MstCheckListData = from d in db.MstCheckLists where d.Id == Convert.ToInt32(id) select d;
                if (MstCheckListData.Any())
                {
                    if (!MstCheckListData.First().IsLocked)
                    {
                        var UpdateCheckListData = MstCheckListData.FirstOrDefault();

                        UpdateCheckListData.CheckListCode = UpdateMstCheckList.CheckListCode;
                        UpdateCheckListData.CheckList = UpdateMstCheckList.CheckList;
                        UpdateCheckListData.CheckListDate = Convert.ToDateTime(UpdateMstCheckList.CheckListDate);
                        UpdateCheckListData.ProjectId = UpdateMstCheckList.ProjectId;
                        UpdateCheckListData.Remarks = UpdateMstCheckList.Remarks;
                        UpdateCheckListData.Status = UpdateMstCheckList.Status;
                        UpdateCheckListData.IsLocked = true;
                        UpdateCheckListData.CreatedBy = UpdateMstCheckList.CreatedBy;
                        UpdateCheckListData.CreatedDateTime = Convert.ToDateTime(UpdateMstCheckList.CreatedDateTime);
                        UpdateCheckListData.UpdatedBy = UpdateMstCheckList.UpdatedBy;
                        UpdateCheckListData.UpdatedDateTime = Convert.ToDateTime(UpdateMstCheckList.UpdatedDateTime);

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
        public HttpResponseMessage UnLock(string id, Models.MstChecList UnLockMstChecList)
        {
            try
            {
                var MstChecListData = from d in db.MstCheckLists where d.Id == Convert.ToInt32(id) select d;
                if (MstChecListData.Any())
                {
                    if (MstChecListData.First().IsLocked)
                    {
                        var UnLockMstChecListData = MstChecListData.FirstOrDefault();

                        UnLockMstChecListData.IsLocked = false;

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
