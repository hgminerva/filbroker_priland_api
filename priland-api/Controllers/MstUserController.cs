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
         
        //List
        [HttpGet,Route("List")]
        public List<Models.MstUser> GetMstUser()
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
        public Models.MstUser GetMstUserId(string id)
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
            return (Models.MstUser)MstUserData.FirstOrDefault();
        }

        //ADD
        [HttpPost,Route("Add")]
        public Int32 PostMstUser(MstUser addMstUser)
        {
            try
            {
                Data.MstUser newMstUser = new Data.MstUser()
                {
                    Username = addMstUser.Username,
                    FullName = addMstUser.FullName,
                    Password = addMstUser.Password,
                    Status = addMstUser.Status,
                    AspNetId = addMstUser.AspNetId
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

        //Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstUsers(string id)
        {
            try
            {
                var MstUserData = from d in db.MstUsers  where d.Id == Convert.ToInt32(id) select d;
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

        //List
        [HttpPut, Route("Update/{id}")]
        public HttpResponseMessage UpdateUser(string id, Models.MstUser UpdateMstUser)
        {
            try
            {
                var MstUserData = from d in db.MstUsers where d.Id == Convert.ToInt32(id) select d;
                if (MstUserData.Any())
                {
                    var UpdateUserData = MstUserData.FirstOrDefault();

                    UpdateUserData.Username = UpdateMstUser.Username;
                    UpdateUserData.FullName = UpdateMstUser.FullName;
                    UpdateUserData.Password = UpdateMstUser.Password;
                    UpdateUserData.Status = UpdateMstUser.Status;
                    UpdateUserData.AspNetId = UpdateMstUser.AspNetId;

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
