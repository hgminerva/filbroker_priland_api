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
        [HttpPost,Route("Add")]
        public Int32 PostMstUser()
        {
            try
            {
                Data.MstUser newMstUser = new Data.MstUser()
                {
                    Username = "NA",
                    FullName = "NA",
                    Password = "NA",
                    Status = "NA",
                    AspNetId = null
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

    }
}
