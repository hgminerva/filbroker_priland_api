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

        //List
        [HttpGet, Route("List")]
        public List<TrnSoldUnitRequirementActivity> GetTrnSoldUnitRequirementActivity()
        {
            var TrnSoldUnitRequirementActivityData = from d in db.TrnSoldUnitRequirementActivities
                                                     select new TrnSoldUnitRequirementActivity
                                                     {
                                                         Id = d.Id,
                                                         SoldUnitRequirementId = d.SoldUnitRequirementId,
                                                         ActivityDate = d.ActivityDate.ToShortDateString(),
                                                         Activity = d.Activity,
                                                         Remarks = d.Remarks,
                                                         UserId = d.UserId
                                                     };
            return TrnSoldUnitRequirementActivityData.ToList();
        }

        //Detail
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
                                                         UserId = d.UserId
                                                     };
            return (TrnSoldUnitRequirementActivity)TrnSoldUnitRequirementActivityData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostTrnSoldUnitRequirementActivity(TrnSoldUnitRequirementActivity addTrnSoldUnitRequirementActivity)
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
                        SoldUnitRequirementId = addTrnSoldUnitRequirementActivity.SoldUnitRequirementId,
                        ActivityDate = Convert.ToDateTime(addTrnSoldUnitRequirementActivity.ActivityDate),
                        Activity = addTrnSoldUnitRequirementActivity.Activity,
                        Remarks = addTrnSoldUnitRequirementActivity.Remarks,
                        UserId = addTrnSoldUnitRequirementActivity.UserId
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

        //Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteTrnSoldUnitRequirementActivity(string id)
        {
            try
            {
                var TrnSoldUnitRequirementActivityData = from d in db.TrnSoldUnitRequirementActivities where d.Id == Convert.ToInt32(id) select d;
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

        //Update
        [HttpPut, Route("Update")]
        public HttpResponseMessage Update(TrnSoldUnitRequirementActivity UpdateTrnSoldUnitRequirementActivity)
        {
            try
            {
                var TrnSoldUnitRequirementActivityData = from d in db.TrnSoldUnitRequirementActivities where d.Id == Convert.ToInt32(UpdateTrnSoldUnitRequirementActivity.Id) select d;
                if (TrnSoldUnitRequirementActivityData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UpdateTrnSoldUnitRequirementActivityData = TrnSoldUnitRequirementActivityData.FirstOrDefault();

                        UpdateTrnSoldUnitRequirementActivityData.SoldUnitRequirementId = UpdateTrnSoldUnitRequirementActivity.SoldUnitRequirementId;
                        UpdateTrnSoldUnitRequirementActivityData.ActivityDate = Convert.ToDateTime(UpdateTrnSoldUnitRequirementActivity.ActivityDate);
                        UpdateTrnSoldUnitRequirementActivityData.Activity = UpdateTrnSoldUnitRequirementActivity.Activity;
                        UpdateTrnSoldUnitRequirementActivityData.Remarks = UpdateTrnSoldUnitRequirementActivity.Remarks;
                        UpdateTrnSoldUnitRequirementActivityData.UserId = UpdateTrnSoldUnitRequirementActivity.UserId;

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
