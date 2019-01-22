using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using priland_api.Models;
using Microsoft.AspNet.Identity;

namespace priland_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/trnSoldUnitOwner")]
    public class TrnSoldUnitCoOwnerController : ApiController
    {
        public Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        [HttpGet, Route("dropdown/customer/list")]
        public List<MstCustomer> ListDropdownCoOwnerCustomer()
        {
            var customers = from d in db.MstCustomers
                            select new MstCustomer
                            {
                                Id = d.Id,
                                CustomerCode = d.CustomerCode,
                                FullName = d.LastName + ", " + d.FirstName + " " + d.MiddleName
                            };

            return customers.ToList();
        }

        [HttpGet, Route("list/{soldUnitId}")]
        public List<TrnSoldUnitCoOwner> ListCoOwner(String soldUnitId)
        {
            var soldUnitOwners = from d in db.TrnSoldUnitCoOwners
                                 where d.SoldUnitId == Convert.ToInt32(soldUnitId)
                                 select new TrnSoldUnitCoOwner
                                 {
                                     Id = d.Id,
                                     SoldUnitId = d.SoldUnitId,
                                     CustomerId = d.CustomerId,
                                     CustomerCode = d.MstCustomer.CustomerCode,
                                     Customer = d.MstCustomer.LastName + ", " + d.MstCustomer.FirstName + " " + d.MstCustomer.MiddleName,
                                     Address = d.MstCustomer.Address
                                 };

            return soldUnitOwners.ToList();
        }

        [HttpPost, Route("add")]
        public HttpResponseMessage AddSoldUnitOwner(TrnSoldUnitCoOwner objSoldUnitOwner)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.AspNetId == User.Identity.GetUserId() select d;

                if (!currentUser.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No current user logged in."; }
                else
                {
                    Data.TrnSoldUnitCoOwner newSoldUnitCoOwner = new Data.TrnSoldUnitCoOwner
                    {
                        SoldUnitId = objSoldUnitOwner.SoldUnitId,
                        CustomerId = objSoldUnitOwner.CustomerId
                    };

                    db.TrnSoldUnitCoOwners.InsertOnSubmit(newSoldUnitCoOwner);
                    db.SubmitChanges();
                }

                return Request.CreateResponse(responseStatusCode, responseMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut, Route("update")]
        public HttpResponseMessage UpdateSoldUnitOwner(TrnSoldUnitCoOwner objSoldUnitOwner)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.AspNetId == User.Identity.GetUserId() select d;
                var currentSoldUnitOwner = from d in db.TrnSoldUnitCoOwners where d.Id == objSoldUnitOwner.Id select d;

                if (!currentUser.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No current user logged in."; }
                if (!currentSoldUnitOwner.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "Reference not found."; }
                else
                {
                    var updateCurrentSoldUnitOwner = currentSoldUnitOwner.FirstOrDefault();
                    updateCurrentSoldUnitOwner.CustomerId = objSoldUnitOwner.CustomerId;
                    db.SubmitChanges();
                }

                return Request.CreateResponse(responseStatusCode, responseMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete, Route("delete/{id}")]
        public HttpResponseMessage DeleteSoldUnitOwner(String id)
        {
            try
            {
                HttpStatusCode responseStatusCode = HttpStatusCode.OK;
                String responseMessage = "";

                var currentUser = from d in db.MstUsers where d.AspNetId == User.Identity.GetUserId() select d;
                var currentSoldUnitOwner = from d in db.TrnSoldUnitCoOwners where d.Id == Convert.ToInt32(id) select d;

                if (!currentUser.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "No current user logged in."; }
                if (!currentSoldUnitOwner.Any()) { responseStatusCode = HttpStatusCode.NotFound; responseMessage = "Reference not found."; }
                else
                {
                    db.TrnSoldUnitCoOwners.DeleteOnSubmit(currentSoldUnitOwner.FirstOrDefault());
                    db.SubmitChanges();
                }

                return Request.CreateResponse(responseStatusCode, responseMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
