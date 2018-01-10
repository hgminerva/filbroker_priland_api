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
        public List<MstCheckList> GetMstCheckList()
        {
            var MstCheckListData = from d in db.MstCheckLists
                                 select new Models.MstCheckList
                                 {
                                     Id = d.Id,
                                     ChecklistCode = d.CheckListCode,
                                     Checklist = d.CheckList,
                                     ChecklistDate = d.CheckListDate.ToShortDateString(),
                                     ProjectId = d.ProjectId,
                                     Project = d.MstProject.Project,
                                     Remarks = d.Remarks,
                                     Status = d.Status,
                                     IsLocked = d.IsLocked,
                                     CreatedBy = d.CreatedBy,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedBy = d.UpdatedBy,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return MstCheckListData.ToList();
        }

        //List per Project ID
        [HttpGet, Route("ListPerProjectId/{id}")]
        public List<MstCheckList> GetMstCheckListPerProjectId(string id)
        {
            var MstChecklistData = from d in db.MstCheckLists
                                   where d.ProjectId == Convert.ToInt32(id)
                                   orderby d.CheckListCode
                                   select new MstCheckList
                                   {
                                        Id = d.Id,
                                        ChecklistCode = d.CheckListCode,
                                        Checklist = d.CheckList,
                                        ChecklistDate = d.CheckListDate.ToShortDateString(),
                                        ProjectId = d.ProjectId,
                                        Project = d.MstProject.Project,
                                        Remarks = d.Remarks,
                                        Status = d.Status,
                                        IsLocked = d.IsLocked,
                                        CreatedBy = d.CreatedBy,
                                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                        UpdatedBy = d.UpdatedBy,
                                        UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                   };
            return MstChecklistData.ToList();
        }

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public MstCheckList GetMstCheckList(string id)
        {
            var MstChecListData = from d in db.MstCheckLists
                                 where d.Id == Convert.ToInt32(id)
                                 select new Models.MstCheckList
                                 {
                                     Id = d.Id,
                                     ChecklistCode = d.CheckListCode,
                                     Checklist = d.CheckList,
                                     ChecklistDate = d.CheckListDate.ToShortDateString(),
                                     ProjectId = d.ProjectId,
                                     Project = d.MstProject.Project,
                                     Remarks = d.Remarks,
                                     Status = d.Status,
                                     IsLocked = d.IsLocked,
                                     CreatedBy = d.CreatedBy,
                                     CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                     UpdatedBy = d.UpdatedBy,
                                     UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                 };
            return (Models.MstCheckList)MstChecListData.FirstOrDefault();
        }

        //Add
        [HttpPost, Route("Add")]
        public Int32 PostMstCheckList(MstCheckList checklist)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    string checklistCode = "0001";
                    var checklists = from d in db.MstCheckLists.OrderByDescending(d => d.Id) select d;
                    if (checklists.Any())
                    {
                        Int32 nextChecklistCode = Convert.ToInt32(checklists.FirstOrDefault().CheckListCode) + 1;
                        checklistCode = padNumWithZero(nextChecklistCode, 4);
                    }

                    Data.MstCheckList newMstChecList = new Data.MstCheckList()
                    {
                        CheckListCode = checklistCode,
                        CheckList = checklist.Checklist,
                        CheckListDate = Convert.ToDateTime(checklist.ChecklistDate),
                        ProjectId = checklist.ProjectId,
                        Remarks = checklist.Remarks,
                        Status = checklist.Status,
                        IsLocked = checklist.IsLocked,
                        CreatedBy = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Now,
                        UpdatedBy = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Now,
                    };

                    db.MstCheckLists.InsertOnSubmit(newMstChecList);
                    db.SubmitChanges();

                    return newMstChecList.Id;
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
        public HttpResponseMessage DeleteMstCheckList(string id)
        {
            try
            {
                var MstCheckListData = from d in db.MstCheckLists where d.Id == Convert.ToInt32(id) select d;
                if (MstCheckListData.Any())
                {
                    if (MstCheckListData.First().IsLocked == false)
                    {
                        db.MstCheckLists.DeleteOnSubmit(MstCheckListData.First());
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
        public HttpResponseMessage SaveMstChecList(MstCheckList checklist)
        {
            try
            {
                var MstChecListData = from d in db.MstCheckLists where d.Id == Convert.ToInt32(checklist.Id) select d;
                if (MstChecListData.Any())
                {
                    if (MstChecListData.First().IsLocked == false)
                    {
                        var currentUser = from d in db.MstUsers
                                          where d.AspNetId == User.Identity.GetUserId()
                                          select d;

                        if (currentUser.Any())
                        {
                            var UpdateMstChecListData = MstChecListData.FirstOrDefault();

                            UpdateMstChecListData.CheckListCode = checklist.ChecklistCode;
                            UpdateMstChecListData.CheckList = checklist.Checklist;
                            UpdateMstChecListData.CheckListDate = Convert.ToDateTime(checklist.ChecklistDate);
                            UpdateMstChecListData.ProjectId = checklist.ProjectId;
                            UpdateMstChecListData.Remarks = checklist.Remarks;
                            UpdateMstChecListData.Status = checklist.Status;
                            UpdateMstChecListData.UpdatedBy = currentUser.FirstOrDefault().Id;
                            UpdateMstChecListData.UpdatedDateTime = DateTime.Now;

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
        public HttpResponseMessage UpdateCheckList(MstCheckList checklist)
        {
            try
            {
                var MstCheckListData = from d in db.MstCheckLists where d.Id == Convert.ToInt32(checklist.Id) select d;

                if (MstCheckListData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;

                    if (currentUser.Any())
                    {
                        var UpdateCheckListData = MstCheckListData.FirstOrDefault();

                        UpdateCheckListData.CheckListCode = checklist.ChecklistCode;
                        UpdateCheckListData.CheckList = checklist.Checklist;
                        UpdateCheckListData.CheckListDate = Convert.ToDateTime(checklist.ChecklistDate);
                        UpdateCheckListData.ProjectId = checklist.ProjectId;
                        UpdateCheckListData.Remarks = checklist.Remarks;
                        UpdateCheckListData.Status = checklist.Status;
                        UpdateCheckListData.IsLocked = true;
                        UpdateCheckListData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateCheckListData.UpdatedDateTime = DateTime.Now;

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
        public HttpResponseMessage UnLock(MstCheckList checklist)
        {
            try
            {
                var MstChecListData = from d in db.MstCheckLists where d.Id == Convert.ToInt32(checklist.Id) select d;

                if (MstChecListData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;

                    if (currentUser.Any())
                    {
                        var UnLockMstChecListData = MstChecListData.FirstOrDefault();

                        UnLockMstChecListData.IsLocked = false;
                        UnLockMstChecListData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UnLockMstChecListData.UpdatedDateTime = DateTime.Now;

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
