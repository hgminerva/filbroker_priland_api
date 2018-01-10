using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using priland_api.Models;
using Microsoft.AspNet.Identity;

namespace priland_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/MstUser")]
    public class MstUserController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();
         
        // List
        [HttpGet,Route("List")]
        public List<MstUser> GetMstUsers()
        {
            var MstUserData = from d in db.MstUsers 
                                 select new Models.MstUser
                                 {

                                     Id = d.Id,
                                     Username=d.Username,
                                     FullName=d.FullName,
                                     Password=d.Password,
                                     Status=d.Status,
                                     AspNetId=d.AspNetId
                                 };
            return MstUserData.ToList();
        }

        [HttpGet, Route("Detail/{id}")]
        public Models.MstUser GetMstUser(string id)
        {
            var MstUserData = from d in db.MstUsers 
                              where d.Id == Convert.ToInt32(id)
                              select new Models.MstUser
                              {
                                    Id = d.Id,
                                    Username = d.Username,
                                    FullName = d.FullName,
                                    Password = d.Password,
                                    Status = d.Status,
                                    AspNetId = d.AspNetId
                              };
            return MstUserData.FirstOrDefault();
        }

        // Add
        [HttpPost,Route("Add")]
        public Int32 PostMstUser(MstUser user)
        {
            try
            {
                Data.MstUser newMstUser = new Data.MstUser()
                {
                    Username = user.Username,
                    FullName = user.FullName,
                    Password = user.Password,
                    Status = user.Status,
                    AspNetId = user.AspNetId
                };

                db.MstUsers.InsertOnSubmit(newMstUser);
                db.SubmitChanges();

                return newMstUser.Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstUser(string id)
        {
            try
            {
                var MstUserData = from d in db.MstUsers  
                                  where d.Id == Convert.ToInt32(id) select d;

                if (MstUserData.Any())
                {
                    db.MstUsers.DeleteOnSubmit(MstUserData.First());
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
        public HttpResponseMessage UpdateMstUser(MstUser user)
        {
            try
            {
                var MstUserData = from d in db.MstUsers
                                  where d.Id == Convert.ToInt32(user.Id)
                                  select d;

                if (MstUserData.Any())
                {
                    var UpdateUserData = MstUserData.FirstOrDefault();

                    UpdateUserData.FullName = user.FullName;
                    UpdateUserData.Status = user.Status;

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
