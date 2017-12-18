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

        //List
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

        //Detail
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
            return (SysDropDown)SysDropDownData.FirstOrDefault();
        }

        //ADD
        [HttpPost, Route("Add")]
        public Int32 PostSysDropDown(SysDropDown addSysDropDown)
        {
            try
            {
                Data.SysDropDown newSysDropDown = new Data.SysDropDown()
                {
                    Category = addSysDropDown.Category,
                    Description = addSysDropDown.Description,
                    Value = addSysDropDown.Value
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

        //Delete
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

        //Lock
        [HttpPut, Route("Update/{id}")]
        public HttpResponseMessage UpdateDropDown(string id, SysDropDown UpdateSysDropDown)
        {
            try
            {
                var SysDropDownData = from d in db.SysDropDowns where d.Id == Convert.ToInt32(id) select d;
                if (SysDropDownData.Any())
                {
                    var UpdateSysDropDownData = SysDropDownData.FirstOrDefault();

                    UpdateSysDropDownData.Category = UpdateSysDropDown.Category;
                    UpdateSysDropDownData.Description = UpdateSysDropDown.Description;
                    UpdateSysDropDownData.Value = UpdateSysDropDown.Value;
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
