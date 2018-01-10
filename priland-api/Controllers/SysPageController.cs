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
    [RoutePrefix("api/SysPage")]
    public class SysPageController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        // List
        [HttpGet, Route("List")]
        public List<SysPage> GetSysPage()
        {
            var SysPageData = from d in db.SysPages
                              select new SysPage
                              {
                                  Id = d.Id,
                                  Page = d.Page,
                                  Url = d.Url
                              };
            return SysPageData.ToList();
        }

        // Detail
        [HttpGet, Route("Detail/{id}")]
        public SysPage GetSysPageId(string id)
        {
            var SysPageData = from d in db.SysPages
                              where d.Id == Convert.ToInt32(id)
                              select new SysPage
                              {
                                  Id = d.Id,
                                  Page = d.Page,
                                  Url = d.Url
                              };
            return (SysPage)SysPageData.FirstOrDefault();
        }

        // Add
        [HttpPost, Route("Add")]
        public Int32 PostSysPage(SysPage addSysPage)
        {
            try
            {
                Data.SysPage newSysPage = new Data.SysPage()
                {
                    Page = addSysPage.Page,
                    Url = addSysPage.Page
                };

                db.SysPages.InsertOnSubmit(newSysPage);
                db.SubmitChanges();

                return newSysPage.Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteSysPage(string id)
        {
            try
            {
                var SysPageData = from d in db.SysPages where d.Id == Convert.ToInt32(id) select d;
                if (SysPageData.Any())
                {
                    db.SysPages.DeleteOnSubmit(SysPageData.First());
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
        [HttpPut, Route("Save/{id}")]
        public HttpResponseMessage SaveSysPage(SysPage sysPage)
        {
            try
            {
                var SysPageData = from d in db.SysPages where d.Id == Convert.ToInt32(sysPage.Id) select d;
                if (SysPageData.Any())
                {
                    var UpdateSysPageData = SysPageData.FirstOrDefault();

                    UpdateSysPageData.Page = sysPage.Page;
                    UpdateSysPageData.Url = sysPage.Url;

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
