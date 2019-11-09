﻿using priland_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace priland_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/TrnCollectionPayment")]
    public class TrnCollectionPaymentController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        public String padNumWithZero(Int32 number, Int32 length)
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

        [HttpGet, Route("CollectionPayment/{collectionId}")]
        public List<TrnCollectionPayment> GetCollectionPayment(Int32 collectionId)
        {
            var collectionList = from d in db.TrnCollectionPayments
                                 where d.CollectionId == collectionId
                                 select new TrnCollectionPayment
                                 {
                                     Id = d.Id,
                                     CollectionId = d.CollectionId,
                                     SoldUnitId = d.SoldUnitId,
                                     SoldUnit = d.TrnSoldUnit.SoldUnitNumber,
                                     Project = d.TrnSoldUnit.MstUnit.MstProject.Project,
                                     PayType = d.PayType,
                                     Amount = d.Amount,
                                     CheckNumber = d.CheckNumber,
                                     CheckDate = d.CheckDate.ToString(),
                                     CheckBank = d.CheckBank,
                                     OtherInformation = d.OtherInformation
                                 };
            return collectionList.ToList();
        }

        //Sold Units
        [HttpGet, Route("SoldUnits/{customerId}")]
        public List<TrnCollectionPaymentSoldUnit> GetSoldUnits(string customerId)
        {
            var soldUnits = from d in db.TrnSoldUnits
                            where d.CustomerId == Convert.ToInt32(customerId)
                                 select new TrnCollectionPaymentSoldUnit
                                 {
                                     Id = d.Id,
                                     SoldUnitNumber = d.SoldUnitNumber,
                                     UnitCode = d.MstUnit.UnitCode,
                                     Project = d.MstProject.Project
                                 };
            return soldUnits.ToList();
        }

        //Sold Units
        [HttpGet, Route("SysDropDown")]
        public List<SysDropDown> GetSysDropDown()
        {
            var sysDropDown = from d in db.SysDropDowns
                            where d.Category.Equals("FINANCING TYPE")
                            select new SysDropDown
                            {
                                Id = d.Id,
                                Description = d.Description,
                            };
            return sysDropDown.ToList();
        }

        //Add
        [HttpPost, Route("Add")]
        public Int32 PostCollectionPayment(TrnCollectionPayment collectionPayment)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var soldUnit = from d in db.TrnSoldUnits
                                   select d;
                    Int32 soldUnitId = 0;
                    if (soldUnit.Any())
                    {
                        soldUnitId = soldUnit.FirstOrDefault().Id;
                    }

                    string collectioPayType = "";
                    var paytype = from d in db.SysDropDowns
                                  where d.Id == Convert.ToInt32(collectionPayment.PayType)
                                  select d;

                    if (paytype.Any())
                    {
                        collectioPayType = paytype.FirstOrDefault().Description;
                    }

                    Data.TrnCollectionPayment newCollectionPayment = new Data.TrnCollectionPayment()
                    {
                        CollectionId = collectionPayment.CollectionId,
                        SoldUnitId = collectionPayment.SoldUnitId,
                        PayType = collectioPayType,
                        Amount = collectionPayment.Amount,
                        CheckNumber = collectionPayment.CheckNumber,
                        CheckDate = Convert.ToDateTime(collectionPayment.CheckDate),
                        CheckBank = collectionPayment.CheckBank,
                        OtherInformation = collectionPayment.OtherInformation,
                    };

                    db.TrnCollectionPayments.InsertOnSubmit(newCollectionPayment);
                    db.SubmitChanges();

                    return newCollectionPayment.Id;
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

        //Lock
        [HttpPut, Route("Update")]
        public HttpResponseMessage UpdateCollectionPayment(TrnCollectionPayment collectionPayment)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentCollectionPayments = from d in db.TrnCollectionPayments where d.Id == Convert.ToInt32(collectionPayment.Id) select d;

                    string collectioPayType = "";
                    var paytype = from d in db.SysDropDowns
                                  where d.Id == Convert.ToInt32(collectionPayment.PayType)
                                  select d;

                    if (paytype.Any()) {
                        collectioPayType = paytype.FirstOrDefault().Description;
                    }

                    if (currentCollectionPayments.Any())
                    {
                        var updateCollectionPayment = currentCollectionPayments.FirstOrDefault();
                        updateCollectionPayment.SoldUnitId = collectionPayment.SoldUnitId;
                        updateCollectionPayment.PayType = collectioPayType;
                        updateCollectionPayment.Amount = collectionPayment.Amount;
                        updateCollectionPayment.CheckNumber = collectionPayment.CheckNumber;
                        updateCollectionPayment.CheckDate = Convert.ToDateTime(collectionPayment.CheckDate);
                        updateCollectionPayment.CheckBank = collectionPayment.CheckNumber;
                        updateCollectionPayment.OtherInformation = collectionPayment.OtherInformation;
                        db.SubmitChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Collection not exist!");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        //Delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteTrnCollectionPayment(string id)
        {
            try
            {
                var collectionPayment = from d in db.TrnCollectionPayments where d.Id == Convert.ToInt32(id) select d;
                if (collectionPayment.Any())
                {
                    db.TrnCollectionPayments.DeleteOnSubmit(collectionPayment.First());
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
