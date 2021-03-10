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
    [RoutePrefix("api/TrnCollection")]
    public class TrnCollectionController : ApiController
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

        //Customer List
        [HttpGet, Route("CustomerList")]
        public List<MstCustomer> GetCollectionList()
        {
            var customers = from d in db.MstCustomers
                            select new MstCustomer
                            {
                                Id = d.Id,
                                FullName = d.LastName + "," + d.FirstName + " " + d.MiddleName,
                            };
            return customers.ToList();
        }

        // UserList
        [HttpGet, Route("UserList")]
        public List<MstUser> GetMstUsers()
        {
            var MstUserData = from d in db.MstUsers
                              select new Models.MstUser
                              {

                                  Id = d.Id,
                                  Username = d.Username,
                                  FullName = d.FullName,
                                  Password = d.Password,
                                  Status = d.Status,
                                  AspNetId = d.AspNetId
                              };
            return MstUserData.ToList();
        }

        //List per date range
        [HttpGet, Route("CollectionFillterByDate/{dateStart}/{dateEnd}")]
        public List<TrnCollection> GetCollectionList(string dateStart, string dateEnd)
        {
            var collectionList = from d in db.TrnCollections
                                 where d.CollectionDate >= Convert.ToDateTime(dateStart)
                                 && d.CollectionDate <= Convert.ToDateTime(dateEnd)
                                 select new TrnCollection
                                 {
                                     Id = d.Id,
                                     CollectionNumber = d.CollectionNumber,
                                     CollectionDate = d.CollectionDate.ToLongDateString(),
                                     ManualNumber = d.ManualNumber,
                                     CustomerId = d.CustomerId,
                                     Customer = d.MstCustomer.LastName + "," + d.MstCustomer.FirstName + " " + d.MstCustomer.MiddleName,
                                     Particulars = d.Particulars,
                                     PreparedBy = d.PreparedBy,
                                     CheckedBy = d.CheckedBy,
                                     ApprovedBy = d.ApprovedBy,
                                     IsLocked = d.IsLocked,
                                     CreatedBy = d.CreatedBy,
                                     CreatedDateTime = d.CreatedDateTime.ToLongDateString(),
                                     UpdatedBy = d.UpdatedBy,
                                     UpdatedDateTime = d.UpdatedDateTime.ToLongDateString(),
                                 };
            return collectionList.ToList();
        }

        [HttpGet, Route("Detail/{id}")]
        public TrnCollection GetCollectionDetail(string id)
        {
            var collection = from d in db.TrnCollections
                             where d.Id == Convert.ToInt32(id)
                             select new TrnCollection
                             {
                                 Id = d.Id,
                                 CollectionNumber = d.CollectionNumber,
                                 CollectionDate = d.CollectionDate.ToLongDateString(),
                                 ManualNumber = d.ManualNumber,
                                 CustomerId = d.CustomerId,
                                 Customer = d.MstCustomer.LastName + "," + d.MstCustomer.FirstName + " " + d.MstCustomer.MiddleName,
                                 Particulars = d.Particulars,
                                 PreparedBy = d.PreparedBy,
                                 CheckedBy = d.CheckedBy,
                                 ApprovedBy = d.ApprovedBy,
                                 IsLocked = d.IsLocked,
                                 CreatedBy = d.CreatedBy,
                                 CreatedDateTime = d.CreatedDateTime.ToLongDateString(),
                                 UpdatedBy = d.UpdatedBy,
                                 UpdatedDateTime = d.UpdatedDateTime.ToLongDateString(),
                             };
            return collection.FirstOrDefault();
        }

        public void UpdateAccountsReceivable(Int32 soldUnitId)
        {
            Decimal pricePayment = 0;

            var collectionPayments = from d in db.TrnCollectionPayments
                                     where d.SoldUnitId == soldUnitId
                                     && d.TrnCollection.IsLocked == true
                                     select d;

            if (collectionPayments.Any())
            {
                pricePayment = collectionPayments.Sum(d => d.Amount);

                foreach (var collectionPayment in collectionPayments)
                {
                    var equitySchedule = from d in db.TrnSoldUnitEquitySchedules
                                         where d.Id == collectionPayment.SoldUnitEquityScheduleId
                                         select d;

                    if (equitySchedule.Any())
                    {
                        var updateEquitySchedule = equitySchedule.FirstOrDefault();
                        updateEquitySchedule.CheckNumber = collectionPayment.CheckNumber;
                        updateEquitySchedule.CheckDate = collectionPayment.CheckDate;
                        updateEquitySchedule.CheckBank = collectionPayment.CheckBank;
                        updateEquitySchedule.PaidAmount = collectionPayment.Amount;
                        updateEquitySchedule.BalanceAmount = equitySchedule.FirstOrDefault().Amortization - collectionPayment.Amount;
                        db.SubmitChanges();
                    }
                }
            }
            else
            {
                var unlockedCollectionPayments = from d in db.TrnCollectionPayments
                                                 where d.SoldUnitId == soldUnitId
                                                 && d.TrnCollection.IsLocked == false
                                                 select d;

                if (unlockedCollectionPayments.Any())
                {
                    foreach (var unlockedCollectionPayment in unlockedCollectionPayments)
                    {
                        var equitySchedule = from d in db.TrnSoldUnitEquitySchedules
                                             where d.Id == unlockedCollectionPayment.SoldUnitEquityScheduleId
                                             select d;

                        if (equitySchedule.Any())
                        {
                            var updateEquitySchedule = equitySchedule.FirstOrDefault();
                            updateEquitySchedule.CheckNumber = "";
                            updateEquitySchedule.CheckDate = null;
                            updateEquitySchedule.CheckBank = "";
                            updateEquitySchedule.PaidAmount = 0;
                            updateEquitySchedule.BalanceAmount = equitySchedule.FirstOrDefault().Amortization;
                            db.SubmitChanges();
                        }
                    }
                }
            }

            var soldUnit = from d in db.TrnSoldUnits
                           where d.Id == soldUnitId
                           select d;

            if (soldUnit.Any())
            {
                Decimal price = soldUnit.FirstOrDefault().Price;
                Decimal priceDiscount = soldUnit.FirstOrDefault().PriceDiscount;
                Decimal priceBalance = price - priceDiscount - pricePayment;

                var updateSoldUnit = soldUnit.FirstOrDefault();
                updateSoldUnit.PricePayment = pricePayment;
                updateSoldUnit.PriceBalance = priceBalance;
                db.SubmitChanges();
            }
        }

        //Add
        [HttpPost, Route("Add")]
        public Int32 PostCollection()
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    string newCollectionNumber = "0000000001";
                    var lastCollection = from d in db.TrnCollections.OrderByDescending(d => d.Id) select d;
                    if (lastCollection.Any())
                    {
                        Int32 nextCollection = Convert.ToInt32(lastCollection.FirstOrDefault().CollectionNumber) + 1;
                        newCollectionNumber = padNumWithZero(nextCollection, 10);
                    }
                    Int32 defaultCustomer = 0;

                    var customer = from d in db.MstCustomers
                                   select d;

                    if (customer.Any())
                    {
                        defaultCustomer = customer.FirstOrDefault().Id;
                    }

                    Data.TrnCollection newCollection = new Data.TrnCollection()
                    {
                        CollectionNumber = newCollectionNumber,
                        CollectionDate = DateTime.Today,
                        ManualNumber = "NA",
                        CustomerId = defaultCustomer,
                        Particulars = "NA",
                        PreparedBy = currentUser.FirstOrDefault().Id,
                        CheckedBy = currentUser.FirstOrDefault().Id,
                        ApprovedBy = currentUser.FirstOrDefault().Id,
                        IsLocked = false,
                        CreatedBy = currentUser.FirstOrDefault().Id,
                        CreatedDateTime = DateTime.Today,
                        UpdatedBy = currentUser.FirstOrDefault().Id,
                        UpdatedDateTime = DateTime.Today
                    };

                    db.TrnCollections.InsertOnSubmit(newCollection);
                    db.SubmitChanges();

                    return newCollection.Id;
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

        //Save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveCollection(TrnCollection collection)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentCollection = from d in db.TrnCollections where d.Id == Convert.ToInt32(collection.Id) select d;

                    if (currentCollection.Any())
                    {
                        if (currentCollection.FirstOrDefault().IsLocked == false)
                        {
                            var UpdateTrnCollection = currentCollection.FirstOrDefault();

                            UpdateTrnCollection.CollectionDate = Convert.ToDateTime(collection.CollectionDate);
                            UpdateTrnCollection.ManualNumber = collection.ManualNumber;
                            UpdateTrnCollection.CustomerId = collection.CustomerId;
                            UpdateTrnCollection.Particulars = collection.Particulars;
                            UpdateTrnCollection.PreparedBy = collection.PreparedBy;
                            UpdateTrnCollection.CheckedBy = collection.CheckedBy;
                            UpdateTrnCollection.ApprovedBy = collection.ApprovedBy;
                            UpdateTrnCollection.CreatedBy = collection.CheckedBy;
                            UpdateTrnCollection.UpdatedBy = collection.UpdatedBy;
                            UpdateTrnCollection.UpdatedDateTime = DateTime.Today;

                            db.SubmitChanges();

                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Collection is Saved!");
                        }

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
        //Lock
        [HttpPut, Route("Lock")]
        public HttpResponseMessage LockCollection(TrnCollection collection)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {
                    var currentCollection = from d in db.TrnCollections where d.Id == Convert.ToInt32(collection.Id) select d;

                    if (currentCollection.Any())
                    {
                        if (currentCollection.FirstOrDefault().IsLocked == false)
                        {
                            var UpdateTrnCollection = currentCollection.FirstOrDefault();
                            UpdateTrnCollection.CollectionDate = Convert.ToDateTime(collection.CollectionDate);
                            UpdateTrnCollection.ManualNumber = collection.ManualNumber;
                            UpdateTrnCollection.CustomerId = collection.CustomerId;
                            UpdateTrnCollection.Particulars = collection.Particulars;
                            UpdateTrnCollection.PreparedBy = collection.PreparedBy;
                            UpdateTrnCollection.CheckedBy = collection.CheckedBy;
                            UpdateTrnCollection.ApprovedBy = collection.ApprovedBy;
                            UpdateTrnCollection.IsLocked = true;
                            UpdateTrnCollection.CreatedBy = collection.CheckedBy;
                            UpdateTrnCollection.UpdatedBy = collection.UpdatedBy;
                            UpdateTrnCollection.UpdatedDateTime = DateTime.Today;
                            db.SubmitChanges();

                            var collectionPayments = from d in db.TrnCollectionPayments
                                                     where d.CollectionId == Convert.ToInt32(collection.Id)
                                                     group d by new { d.SoldUnitId } into g
                                                     select g.Key;

                            if (collectionPayments.ToList().Any())
                            {
                                foreach (var collectionPayment in collectionPayments)
                                {
                                    UpdateAccountsReceivable(collectionPayment.SoldUnitId);
                                }
                            }

                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Collection is Locked!");
                        }

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

        //Unlock
        [HttpPut, Route("UnLock")]
        public HttpResponseMessage unCollection(TrnCollection collection)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;
                if (currentUser.Any())
                {
                    var trnCollection = from d in db.TrnCollections where d.Id == Convert.ToInt32(collection.Id) select d;

                    if (trnCollection.Any())
                    {
                        var UnLockTrnCollection = trnCollection.FirstOrDefault();
                        UnLockTrnCollection.IsLocked = false;
                        UnLockTrnCollection.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UnLockTrnCollection.UpdatedDateTime = DateTime.Now;
                        db.SubmitChanges();

                        var collectionPayments = from d in db.TrnCollectionPayments
                                                 where d.CollectionId == Convert.ToInt32(collection.Id)
                                                 group d by new { d.SoldUnitId } into g
                                                 select g.Key;

                        if (collectionPayments.ToList().Any())
                        {
                            foreach (var collectionPayment in collectionPayments)
                            {
                                UpdateAccountsReceivable(collectionPayment.SoldUnitId);
                            }
                        }

                        return Request.CreateResponse(HttpStatusCode.OK);

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
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
        public HttpResponseMessage DeleteTrnCollection(string id)
        {
            try
            {
                var collection = from d in db.TrnCollections
                                 where d.Id == Convert.ToInt32(id)
                                 select d;

                if (collection.Any())
                {
                    if (collection.First().IsLocked == false)
                    {
                        db.TrnCollections.DeleteOnSubmit(collection.First());
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
