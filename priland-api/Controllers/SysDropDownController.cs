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
    [RoutePrefix("api/SysDropDown")]
    public class SysDropDownController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        // list
        [Route("List")]
        public List<SysDropDown> GetSysDropDown()
        {
            var SysDropDownData = from d in db.SysDropDowns
                                  select new SysDropDown
                                  {
                                      Id = d.Id,
                                      Category = d.Category,
                                      Description = d.Description,
                                      Value = d.Value
                                  };
            return SysDropDownData.ToList();
        }

        // detail
        [Route("Detail/{id}")]
        public SysDropDown GetSysDropDownId(string id)
        {
            var SysDropDownData = from d in db.SysDropDowns
                                  where d.Id == Convert.ToInt32(id)
                                  select new SysDropDown
                                  {
                                      Id = d.Id,
                                      Category = d.Category,
                                      Description = d.Description,
                                      Value = d.Value
                                  };
            return SysDropDownData.FirstOrDefault();
        }

        // add
        [HttpPost, Route("Add")]
        public Int32 PostSysDropDown(SysDropDown dropDown)
        {
            try
            {
                Data.SysDropDown newSysDropDown = new Data.SysDropDown()
                {
                    Category = dropDown.Category,
                    Description = dropDown.Description,
                    Value = dropDown.Value
                };

                db.SysDropDowns.InsertOnSubmit(newSysDropDown);
                db.SubmitChanges();

                return newSysDropDown.Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveSysDropDown(SysDropDown dropDown)
        {
            try
            {
                var SysDropDowns = from d in db.SysDropDowns
                                   where d.Id == Convert.ToInt32(dropDown.Id)
                                   select d;

                if (SysDropDowns.Any())
                {
                    var updateSysDropDowns = SysDropDowns.FirstOrDefault();

                    updateSysDropDowns.Category = dropDown.Category;
                    updateSysDropDowns.Description = dropDown.Description;
                    updateSysDropDowns.Value = dropDown.Value;

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

        // delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteSysDropDown(string id)
        {
            try
            {
                var SysDropDownData = from d in db.SysDropDowns where d.Id == Convert.ToInt32(id) select d;
                if (SysDropDownData.Any())
                {
                    db.SysDropDowns.DeleteOnSubmit(SysDropDownData.First());
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
