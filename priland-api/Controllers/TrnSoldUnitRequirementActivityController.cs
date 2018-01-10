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
    [RoutePrefix("api/TrnSoldUnitRequirementActivity")]
    public class TrnSoldUnitRequirementActivityController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        // List
        [HttpGet, Route("List")]
        public List<TrnSoldUnitRequirementActivity> GetTrnSoldUnitRequirementActivities()
        {
            var TrnSoldUnitRequirementActivityData = from d in db.TrnSoldUnitRequirementActivities
                                                     orderby d.ActivityDate descending
                                                     select new TrnSoldUnitRequirementActivity
                                                     {
                                                         Id = d.Id,
                                                         SoldUnitRequirementId = d.SoldUnitRequirementId,
                                                         ActivityDate = d.ActivityDate.ToShortDateString(),
                                                         Activity = d.Activity,
                                                         Remarks = d.Remarks,
                                                         UserId = d.UserId,
                                                         User = d.MstUser.Username
                                                     };
            return TrnSoldUnitRequirementActivityData.ToList();
        }

        // List per Sold unit requirement id
        [HttpGet, Route("ListPerSoldUnitRequirement/{id}")]
        public List<TrnSoldUnitRequirementActivity> GetTrnSoldUnitRequirementActivitiesPerSoldUnitRequirement(string id)
        {
            var TrnSoldUnitRequirementActivityData = from d in db.TrnSoldUnitRequirementActivities
                                                     where d.SoldUnitRequirementId == Convert.ToInt32(id)
                                                     orderby d.ActivityDate descending
                                                     select new TrnSoldUnitRequirementActivity
                                                     {
                                                         Id = d.Id,
                                                         SoldUnitRequirementId = d.SoldUnitRequirementId,
                                                         ActivityDate = d.ActivityDate.ToShortDateString(),
                                                         Activity = d.Activity,
                                                         Remarks = d.Remarks,
                                                         UserId = d.UserId,
                                                         User = d.MstUser.Username
                                                     };
            return TrnSoldUnitRequirementActivityData.ToList();
        }

        // Detail
        [HttpGet, Route("Detail/{id}")]
        public TrnSoldUnitRequirementActivity GetTrnSoldUnitRequirementActivity(string id)
        {
            var TrnSoldUnitRequirementActivityData = from d in db.TrnSoldUnitRequirementActivities
                                                     where d.Id == Convert.ToInt32(id)
                                                     select new TrnSoldUnitRequirementActivity
                                                     {
                                                         Id = d.Id,
                                                         SoldUnitRequirementId = d.SoldUnitRequirementId,
                                                         ActivityDate = d.ActivityDate.ToShortDateString(),
                                                         Activity = d.Activity,
                                                         Remarks = d.Remarks,
                                                         UserId = d.UserId,
                                                         User = d.MstUser.Username
                                                     };
            return TrnSoldUnitRequirementActivityData.FirstOrDefault();
        }

        // Add
        [HttpPost, Route("Add")]
        public Int32 PostTrnSoldUnitRequirementActivity(TrnSoldUnitRequirementActivity soldUnitRequirementActivity)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    Data.TrnSoldUnitRequirementActivity newTrnSoldUnitRequirementActivity = new Data.TrnSoldUnitRequirementActivity()
                    {
                        SoldUnitRequirementId = soldUnitRequirementActivity.SoldUnitRequirementId,
                        ActivityDate = DateTime.Now,
                        Activity = soldUnitRequirementActivity.Activity,
                        Remarks = soldUnitRequirementActivity.Remarks,
                        UserId = currentUser.FirstOrDefault().Id
                    };

                    db.TrnSoldUnitRequirementActivities.InsertOnSubmit(newTrnSoldUnitRequirementActivity);
                    db.SubmitChanges();

                    return newTrnSoldUnitRequirementActivity.Id;
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

        // Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteTrnSoldUnitRequirementActivity(string id)
        {
            try
            {
                var TrnSoldUnitRequirementActivityData = from d in db.TrnSoldUnitRequirementActivities 
                                                         where d.Id == Convert.ToInt32(id) select d;

                if (TrnSoldUnitRequirementActivityData.Any())
                {
                    db.TrnSoldUnitRequirementActivities.DeleteOnSubmit(TrnSoldUnitRequirementActivityData.First());
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

        // Save
        [HttpPut, Route("Save")]
        public HttpResponseMessage Update(TrnSoldUnitRequirementActivity soldUnitRequirementActivity)
        {
            try
            {
                var TrnSoldUnitRequirementActivityData = from d in db.TrnSoldUnitRequirementActivities
                                                         where d.Id == Convert.ToInt32(soldUnitRequirementActivity.Id) 
                                                         select d;

                if (TrnSoldUnitRequirementActivityData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;

                    if (currentUser.Any())
                    {
                        var UpdateTrnSoldUnitRequirementActivityData = TrnSoldUnitRequirementActivityData.FirstOrDefault();

                        UpdateTrnSoldUnitRequirementActivityData.ActivityDate = DateTime.Now;
                        UpdateTrnSoldUnitRequirementActivityData.Activity = soldUnitRequirementActivity.Activity;
                        UpdateTrnSoldUnitRequirementActivityData.Remarks = soldUnitRequirementActivity.Remarks;
                        UpdateTrnSoldUnitRequirementActivityData.UserId = currentUser.FirstOrDefault().Id;

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
