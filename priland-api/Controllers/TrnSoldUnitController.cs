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
    [RoutePrefix("api/TrnSoldUnit")]
    public class TrnSoldUnitController : ApiController
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

        //List
        [HttpGet, Route("List")]
        public List<TrnSoldUnit> GetTrnSoldUnit()
        {
            var TrnSoldUnitData = from d in db.TrnSoldUnits
                                  select new TrnSoldUnit
                                  {
                                      Id = d.Id,
                                      SoldUnitNumber = d.SoldUnitNumber,
                                      SoldUnitDate = d.SoldUnitDate.ToShortDateString(),
                                      ProjectId = d.ProjectId,
                                      Project = d.MstProject.Project,
                                      UnitId = d.UnitId,
                                      Unit = d.MstUnit.Block + " " + d.MstUnit.Lot,
                                      CustomerId = d.CustomerId,
                                      Customer = d.MstCustomer.LastName + ", " + d.MstCustomer.FirstName + " " + d.MstCustomer.MiddleName,
                                      BrokerId = d.BrokerId,
                                      Broker= d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName,
                                      Agent = d.Agent,
                                      BrokerCoordinator = d.BrokerCoordinator,
                                      ChecklistId = d.CheckListId,
                                      Checklist=d.MstCheckList.CheckList,
                                      Price = d.Price,
                                      EquityValue = d.EquityValue,
                                      EquityPercent = d.EquityPercent,
                                      Discount = d.Discount,
                                      Reservation = d.Reservation,
                                      NetEquity = d.NetEquity,
                                      NetEquityInterest = d.NetEquityInterest,
                                      NetEquityNoOfPayments = d.NetEquityNoOfPayments,
                                      NetEquityAmortization = d.NetEquityAmortization,
                                      Balance = d.Balance,
                                      BalanceInterest = d.BalanceInterest,
                                      BalanceNoOfPayments = d.BalanceNoOfPayments,
                                      BalanceAmortization = d.BalanceAmortization,
                                      TotalInvestment = d.TotalInvestment,
                                      PaymentOptions = d.PaymentOptions,
                                      Financing = d.Financing,
                                      Remarks = d.Remarks,
                                      PreparedBy = d.PreparedBy,
                                      PreparedByUser = d.MstUser2.Username,
                                      CheckedBy = d.CheckedBy,
                                      CheckedByUser = d.MstUser3.Username,
                                      ApprovedBy = d.ApprovedBy,
                                      ApprovedByUser = d.MstUser4.Username,
                                      Status = d.Status,
                                      IsLocked = d.IsLocked,
                                      CreatedBy = d.CreatedBy,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedBy = d.UpdatedBy,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };
            return TrnSoldUnitData.ToList();
        }

        //List per date range
        [HttpGet, Route("ListPerDates/{dateStart}/{dateEnd}")]
        public List<TrnSoldUnit> GetTrnSoldUnitPerDates(string dateStart, string dateEnd)
        {
            var TrnSoldUnitData = from d in db.TrnSoldUnits
                                  where d.SoldUnitDate >= Convert.ToDateTime(dateStart) &&
                                        d.SoldUnitDate <= Convert.ToDateTime(dateEnd)
                                  orderby d.SoldUnitNumber descending
                                  select new TrnSoldUnit
                                  {
                                      Id = d.Id,
                                      SoldUnitNumber = d.SoldUnitNumber,
                                      SoldUnitDate = d.SoldUnitDate.ToShortDateString(),
                                      ProjectId = d.ProjectId,
                                      Project = d.MstProject.Project,
                                      UnitId = d.UnitId,
                                      Unit = d.MstUnit.Block + " " + d.MstUnit.Lot,
                                      CustomerId = d.CustomerId,
                                      Customer = d.MstCustomer.LastName + ", " + d.MstCustomer.FirstName + " " + d.MstCustomer.MiddleName,
                                      BrokerId = d.BrokerId,
                                      Broker = d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName,
                                      Agent = d.Agent,
                                      BrokerCoordinator = d.BrokerCoordinator,
                                      ChecklistId = d.CheckListId,
                                      Checklist = d.MstCheckList.CheckList,
                                      Price = d.Price,
                                      EquityValue = d.EquityValue,
                                      EquityPercent = d.EquityPercent,
                                      Discount = d.Discount,
                                      Reservation = d.Reservation,
                                      NetEquity = d.NetEquity,
                                      NetEquityInterest = d.NetEquityInterest,
                                      NetEquityNoOfPayments = d.NetEquityNoOfPayments,
                                      NetEquityAmortization = d.NetEquityAmortization,
                                      Balance = d.Balance,
                                      BalanceInterest = d.BalanceInterest,
                                      BalanceNoOfPayments = d.BalanceNoOfPayments,
                                      BalanceAmortization = d.BalanceAmortization,
                                      TotalInvestment = d.TotalInvestment,
                                      PaymentOptions = d.PaymentOptions,
                                      Financing = d.Financing,
                                      Remarks = d.Remarks,
                                      PreparedBy = d.PreparedBy,
                                      PreparedByUser = d.MstUser2.Username,
                                      CheckedBy = d.CheckedBy,
                                      CheckedByUser = d.MstUser3.Username,
                                      ApprovedBy = d.ApprovedBy,
                                      ApprovedByUser = d.MstUser4.Username,
                                      Status = d.Status,
                                      IsLocked = d.IsLocked,
                                      CreatedBy = d.CreatedBy,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedBy = d.UpdatedBy,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };
            return TrnSoldUnitData.ToList();
        }

        //Detail
        [HttpGet, Route("Detail/{id}")]
        public TrnSoldUnit GetTrnSoldUnitId(string id)
        {
            var TrnSoldUnitData = from d in db.TrnSoldUnits
                                  where d.Id == Convert.ToInt32(id)
                                  select new TrnSoldUnit
                                  {
                                      Id = d.Id,
                                      SoldUnitNumber = d.SoldUnitNumber,
                                      SoldUnitDate = d.SoldUnitDate.ToShortDateString(),
                                      ProjectId = d.ProjectId,
                                      Project = d.MstProject.Project,
                                      UnitId = d.UnitId,
                                      Unit = d.MstUnit.UnitCode,
                                      CustomerId = d.CustomerId,
                                      BrokerId = d.BrokerId,
                                      Agent = d.Agent,
                                      BrokerCoordinator = d.BrokerCoordinator,
                                      ChecklistId = d.CheckListId,
                                      Price = d.Price,
                                      EquityValue = d.EquityValue,
                                      EquityPercent = d.EquityPercent,
                                      Discount = d.Discount,
                                      Reservation = d.Reservation,
                                      NetEquity = d.NetEquity,
                                      NetEquityInterest = d.NetEquityInterest,
                                      NetEquityNoOfPayments = d.NetEquityNoOfPayments,
                                      NetEquityAmortization = d.NetEquityAmortization,
                                      Balance = d.Balance,
                                      BalanceInterest = d.BalanceInterest,
                                      BalanceNoOfPayments = d.BalanceNoOfPayments,
                                      BalanceAmortization = d.BalanceAmortization,
                                      TotalInvestment = d.TotalInvestment,
                                      PaymentOptions = d.PaymentOptions,
                                      Financing = d.Financing,
                                      Remarks = d.Remarks,
                                      PreparedBy = d.PreparedBy,
                                      PreparedByUser = d.MstUser3.Username,
                                      CheckedBy = d.CheckedBy,
                                      CheckedByUser = d.MstUser1.Username,
                                      ApprovedBy = d.ApprovedBy,
                                      ApprovedByUser = d.MstUser.Username,
                                      Status = d.Status,
                                      IsLocked = d.IsLocked,
                                      CreatedBy = d.CreatedBy,
                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                      UpdatedBy = d.UpdatedBy,
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                  };
            return (TrnSoldUnit)TrnSoldUnitData.FirstOrDefault();
        }

        //Add
        [HttpPost, Route("Add")]
        public Int32 PostTrnSoldUnit(TrnSoldUnit soldUnit)
        {
            try
            {
                var currentUser = from d in db.MstUsers
                                  where d.AspNetId == User.Identity.GetUserId()
                                  select d;

                if (currentUser.Any())
                {

                    string soldUnitNumber = "0000000001";
                    var soldUnits = from d in db.TrnSoldUnits.OrderByDescending(d => d.Id) select d;
                    if (soldUnits.Any())
                    {
                        Int32 nextSoldUnitNumber = Convert.ToInt32(soldUnits.FirstOrDefault().SoldUnitNumber) + 1;
                        soldUnitNumber = padNumWithZero(nextSoldUnitNumber, 10);
                    }

                    Int32 projectId = 0;
                    Int32 unitId = 0;
                    Int32 checklistId = 0;

                    Int32 customerId = 0;
                    Int32 brokerId = 0;

                    decimal price = 0;

                    var projects = from d in db.MstProjects where d.IsLocked == true && d.Status == "OPEN" select d;
                    if (projects.Any())
                    {
                        projectId = projects.FirstOrDefault().Id;
                        if (projects.FirstOrDefault().MstUnits.Where(d => d.Status == "OPEN" && d.IsLocked == true).Count() > 0)
                        {
                            var unit = projects.FirstOrDefault().MstUnits.Where(d => d.Status == "OPEN" && d.IsLocked == true).FirstOrDefault();
                            unitId = unit.Id;
                            price = unit.Price;
                        }
                        if (projects.FirstOrDefault().MstCheckLists.Where(d => d.Status == "ACTIVE" && d.IsLocked == true).Count() > 0)
                        {
                            checklistId = projects.FirstOrDefault().MstCheckLists.Where(d => d.Status == "ACTIVE" && d.IsLocked == true).FirstOrDefault().Id;
                        }
                    }
                    if (db.MstCustomers.Where(d => d.Status == "ACTIVE" && d.IsLocked == true).Count() > 0)
                    {
                        customerId = db.MstCustomers.Where(d => d.Status == "ACTIVE" && d.IsLocked == true).FirstOrDefault().Id;
                    }
                    if (db.MstBrokers.Where(d => d.Status == "ACTIVE" && d.IsLocked == true).Count() > 0)
                    {
                        brokerId = db.MstBrokers.Where(d => d.Status == "ACTIVE" && d.IsLocked == true).FirstOrDefault().Id;
                    }

                    String totalInvestment = "";
                    String paymentOptions  = "";
                    String financing = "";

                    var settings = from d in db.SysSettings select d;
                    if (settings.Any())
                    {
                        totalInvestment = settings.FirstOrDefault().TotalInvestment;
                        paymentOptions = settings.FirstOrDefault().PaymentOptions;
                        financing = settings.FirstOrDefault().Financing;
                    }


                    if (projectId > 0 && unitId > 0 && checklistId > 0 && customerId > 0 && brokerId > 0)
                    {
                        int checkedBy = currentUser.FirstOrDefault().Id;
                        int approvedBy = currentUser.FirstOrDefault().Id;

                        var systemSettings = from d in db.SysSettings select d;
                        if (systemSettings.Any())
                        {
                            if(systemSettings.FirstOrDefault().SoldUnitCheckedBy>0) checkedBy = systemSettings.FirstOrDefault().SoldUnitCheckedBy;
                            if(systemSettings.FirstOrDefault().SoldUnitApprovedBy>0) approvedBy = systemSettings.FirstOrDefault().SoldUnitApprovedBy;
                        }

                        Data.TrnSoldUnit newTrnSoldUnit = new Data.TrnSoldUnit()
                        {
                            SoldUnitNumber = soldUnitNumber,
                            SoldUnitDate = Convert.ToDateTime(soldUnit.SoldUnitDate),

                            ProjectId = projectId,
                            UnitId = unitId,
                            CustomerId = customerId,
                            BrokerId = brokerId,
                            Agent = soldUnit.Agent,
                            BrokerCoordinator = soldUnit.BrokerCoordinator,
                            CheckListId = checklistId,

                            Price = price,

                            EquityValue = 0,
                            EquityPercent = 0,
                            Discount = 0,
                            Reservation = 0,
                            NetEquity = 0,
                            NetEquityInterest = 0,
                            NetEquityNoOfPayments = 0,
                            NetEquityAmortization = 0,
                            Balance = 0,
                            BalanceInterest = 0,
                            BalanceNoOfPayments = 0,
                            BalanceAmortization = 0,

                            TotalInvestment = totalInvestment,
                            PaymentOptions = paymentOptions,
                            Financing = financing,

                            Remarks = soldUnit.Remarks,

                            PreparedBy = currentUser.FirstOrDefault().Id,
                            CheckedBy = checkedBy,
                            ApprovedBy = approvedBy,
                            Status = soldUnit.Status,

                            IsLocked = soldUnit.IsLocked,
                            CreatedBy = currentUser.FirstOrDefault().Id,
                            CreatedDateTime = DateTime.Now,
                            UpdatedBy = currentUser.FirstOrDefault().Id,
                            UpdatedDateTime = DateTime.Now
                        };

                        db.TrnSoldUnits.InsertOnSubmit(newTrnSoldUnit);
                        db.SubmitChanges();

                        return newTrnSoldUnit.Id;
                    }
                    else
                    {
                        return 0;
                    }
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
        public HttpResponseMessage DeleteTrnSoldUnit(string id)
        {
            try
            {
                var TrnSoldUnitData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(id) select d;
                if (TrnSoldUnitData.Any())
                {
                    if (TrnSoldUnitData.First().IsLocked == false)
                    {
                        db.TrnSoldUnits.DeleteOnSubmit(TrnSoldUnitData.First());
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

        //Save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveSoldUnit(TrnSoldUnit soldUnit)
        {
            try
            {
                var TrnSoldUniData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(soldUnit.Id) select d;
                if (TrnSoldUniData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    if (currentUser.Any())
                    {
                        var UpdateTrnSoldUnitData = TrnSoldUniData.FirstOrDefault();

                        UpdateTrnSoldUnitData.SoldUnitDate = Convert.ToDateTime(soldUnit.SoldUnitDate);
                        UpdateTrnSoldUnitData.ProjectId = soldUnit.ProjectId;
                        UpdateTrnSoldUnitData.UnitId = soldUnit.UnitId;
                        UpdateTrnSoldUnitData.CustomerId = soldUnit.CustomerId;
                        UpdateTrnSoldUnitData.BrokerId = soldUnit.BrokerId;
                        UpdateTrnSoldUnitData.Agent = soldUnit.Agent;
                        UpdateTrnSoldUnitData.BrokerCoordinator = soldUnit.BrokerCoordinator;
                        UpdateTrnSoldUnitData.CheckListId = soldUnit.ChecklistId;
                        UpdateTrnSoldUnitData.Price = soldUnit.Price;
                        UpdateTrnSoldUnitData.EquityValue = soldUnit.EquityValue;
                        UpdateTrnSoldUnitData.EquityPercent = soldUnit.EquityPercent;
                        UpdateTrnSoldUnitData.Discount = soldUnit.Discount;
                        UpdateTrnSoldUnitData.Reservation = soldUnit.Reservation;
                        UpdateTrnSoldUnitData.NetEquity = soldUnit.NetEquity;
                        UpdateTrnSoldUnitData.NetEquityInterest = soldUnit.NetEquityInterest;
                        UpdateTrnSoldUnitData.NetEquityNoOfPayments = soldUnit.NetEquityNoOfPayments;
                        UpdateTrnSoldUnitData.NetEquityAmortization = soldUnit.NetEquityAmortization;
                        UpdateTrnSoldUnitData.Balance = soldUnit.Balance;
                        UpdateTrnSoldUnitData.BalanceInterest = soldUnit.BalanceInterest;
                        UpdateTrnSoldUnitData.BalanceNoOfPayments = soldUnit.BalanceNoOfPayments;
                        UpdateTrnSoldUnitData.BalanceAmortization = soldUnit.BalanceAmortization;
                        UpdateTrnSoldUnitData.TotalInvestment = soldUnit.TotalInvestment;
                        UpdateTrnSoldUnitData.PaymentOptions = soldUnit.PaymentOptions;
                        UpdateTrnSoldUnitData.Financing = soldUnit.Financing;
                        UpdateTrnSoldUnitData.Remarks = soldUnit.Remarks;
                        UpdateTrnSoldUnitData.PreparedBy = soldUnit.PreparedBy;
                        UpdateTrnSoldUnitData.CheckedBy = soldUnit.CheckedBy;
                        UpdateTrnSoldUnitData.ApprovedBy = soldUnit.ApprovedBy;
                        UpdateTrnSoldUnitData.Status = soldUnit.Status;
                        UpdateTrnSoldUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateTrnSoldUnitData.UpdatedDateTime = DateTime.Now;

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
        
        //Lock
        [HttpPut, Route("Lock")]
        public HttpResponseMessage LockSoldUnit(TrnSoldUnit soldUnit)
        {
            try
            {
                var TrnSoldUniData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(soldUnit.Id) select d;
                
                if (TrnSoldUniData.Any())
                {

                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;

                    if (currentUser.Any())
                    {
                        var UpdateTrnSoldUnitData = TrnSoldUniData.FirstOrDefault();

                        UpdateTrnSoldUnitData.SoldUnitDate = Convert.ToDateTime(soldUnit.SoldUnitDate);
                        UpdateTrnSoldUnitData.ProjectId = soldUnit.ProjectId;
                        UpdateTrnSoldUnitData.UnitId = soldUnit.UnitId;
                        UpdateTrnSoldUnitData.CustomerId = soldUnit.CustomerId;
                        UpdateTrnSoldUnitData.BrokerId = soldUnit.BrokerId;
                        UpdateTrnSoldUnitData.Agent = soldUnit.Agent;
                        UpdateTrnSoldUnitData.BrokerCoordinator = soldUnit.BrokerCoordinator;
                        UpdateTrnSoldUnitData.CheckListId = soldUnit.ChecklistId;
                        UpdateTrnSoldUnitData.Price = soldUnit.Price;
                        UpdateTrnSoldUnitData.EquityValue = soldUnit.EquityValue;
                        UpdateTrnSoldUnitData.EquityPercent = soldUnit.EquityPercent;
                        UpdateTrnSoldUnitData.Discount = soldUnit.Discount;
                        UpdateTrnSoldUnitData.Reservation = soldUnit.Reservation;
                        UpdateTrnSoldUnitData.NetEquity = soldUnit.NetEquity;
                        UpdateTrnSoldUnitData.NetEquityInterest = soldUnit.NetEquityInterest;
                        UpdateTrnSoldUnitData.NetEquityNoOfPayments = soldUnit.NetEquityNoOfPayments;
                        UpdateTrnSoldUnitData.NetEquityAmortization = soldUnit.NetEquityAmortization;
                        UpdateTrnSoldUnitData.Balance = soldUnit.Balance;
                        UpdateTrnSoldUnitData.BalanceInterest = soldUnit.BalanceInterest;
                        UpdateTrnSoldUnitData.BalanceNoOfPayments = soldUnit.BalanceNoOfPayments;
                        UpdateTrnSoldUnitData.BalanceAmortization = soldUnit.BalanceAmortization;
                        UpdateTrnSoldUnitData.TotalInvestment = soldUnit.TotalInvestment;
                        UpdateTrnSoldUnitData.PaymentOptions = soldUnit.PaymentOptions;
                        UpdateTrnSoldUnitData.Financing = soldUnit.Financing;
                        UpdateTrnSoldUnitData.Remarks = soldUnit.Remarks;
                        UpdateTrnSoldUnitData.PreparedBy = soldUnit.PreparedBy;
                        UpdateTrnSoldUnitData.CheckedBy = soldUnit.CheckedBy;
                        UpdateTrnSoldUnitData.ApprovedBy = soldUnit.ApprovedBy;
                        UpdateTrnSoldUnitData.Status = soldUnit.Status;
                        UpdateTrnSoldUnitData.IsLocked = true;
                        UpdateTrnSoldUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UpdateTrnSoldUnitData.UpdatedDateTime = DateTime.Now;

                        // update unit status
                        var currentUnit = from d in db.MstUnits where d.Id == soldUnit.UnitId select d;
                        currentUnit.FirstOrDefault().Status = "CLOSE";

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

        //Unlock
        [HttpPut, Route("UnLock")]
        public HttpResponseMessage UnLockSoldUnit(TrnSoldUnit soldUnit)
        {
            try
            {
                var TrnSoldUnitData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(soldUnit.Id) select d;
                
                if (TrnSoldUnitData.Any())
                {
                    var currentUser = from d in db.MstUsers
                                      where d.AspNetId == User.Identity.GetUserId()
                                      select d;
                    
                    if (currentUser.Any())
                    {
                        var UnLockTrnSoldUnitData = TrnSoldUnitData.FirstOrDefault();

                        UnLockTrnSoldUnitData.IsLocked = false;
                        UnLockTrnSoldUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                        UnLockTrnSoldUnitData.UpdatedDateTime = DateTime.Now;

                        // update unit status
                        var currentUnit = from d in db.MstUnits where d.Id == UnLockTrnSoldUnitData.UnitId select d;
                        currentUnit.FirstOrDefault().Status = "OPEN";

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

        //Cancelled
        [HttpPut, Route("Cancel")]
        public HttpResponseMessage CancelSoldUnit(TrnSoldUnit soldUnit)
        {
            try
            {
                var TrnSoldUnitData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(soldUnit.Id) select d;

                if (TrnSoldUnitData.Any())
                {
                    if (TrnSoldUnitData.FirstOrDefault().IsLocked == true)
                    {
                        var currentUser = from d in db.MstUsers
                                          where d.AspNetId == User.Identity.GetUserId()
                                          select d;

                        if (currentUser.Any())
                        {
                            var UnLockTrnSoldUnitData = TrnSoldUnitData.FirstOrDefault();

                            UnLockTrnSoldUnitData.Remarks = soldUnit.Remarks;
                            UnLockTrnSoldUnitData.Status = "CANCELLED";
                            UnLockTrnSoldUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                            UnLockTrnSoldUnitData.UpdatedDateTime = DateTime.Now;

                            // update unit status
                            var currentUnit = from d in db.MstUnits where d.Id == UnLockTrnSoldUnitData.UnitId select d;
                            currentUnit.FirstOrDefault().Status = "OPEN";

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

        //Transferred
        [HttpPut, Route("Transfer")]
        public Int32 TransferSoldUnit(TrnSoldUnit soldUnit)
        {
            try
            {
                var TrnSoldUnitData = from d in db.TrnSoldUnits where d.Id == Convert.ToInt32(soldUnit.Id) select d;

                if (TrnSoldUnitData.Any())
                {
                    if (TrnSoldUnitData.FirstOrDefault().IsLocked == true)
                    {
                        var currentUser = from d in db.MstUsers
                                          where d.AspNetId == User.Identity.GetUserId()
                                          select d;

                        if (currentUser.Any())
                        {
                            // change sold unit status
                            // =======================
                            var UnLockTrnSoldUnitData = TrnSoldUnitData.FirstOrDefault();

                            UnLockTrnSoldUnitData.Remarks = soldUnit.Remarks;
                            UnLockTrnSoldUnitData.Status = "TRANSFERRED";
                            UnLockTrnSoldUnitData.UpdatedBy = currentUser.FirstOrDefault().Id;
                            UnLockTrnSoldUnitData.UpdatedDateTime = DateTime.Now;

                            // update unit status
                            // ==================
                            var currentUnit = from d in db.MstUnits where d.Id == UnLockTrnSoldUnitData.UnitId select d;
                            currentUnit.FirstOrDefault().Status = "OPEN";

                            db.SubmitChanges();

                            // add new sold unit
                            // =================
                            string soldUnitNumber = "0000000001";
                            var soldUnits = from d in db.TrnSoldUnits.OrderByDescending(d => d.Id) select d;
                            if (soldUnits.Any())
                            {
                                Int32 nextSoldUnitNumber = Convert.ToInt32(soldUnits.FirstOrDefault().SoldUnitNumber) + 1;
                                soldUnitNumber = padNumWithZero(nextSoldUnitNumber, 10);
                            }

                            Int32 projectId = soldUnit.ProjectId;
                            Int32 unitId = soldUnit.UnitId;
                            Int32 checklistId = 0;

                            Int32 customerId = soldUnit.CustomerId;
                            Int32 brokerId = soldUnit.BrokerId;

                            decimal price = soldUnit.Price;

                            var projects = from d in db.MstProjects where d.Id == projectId select d;
                            if (projects.Any())
                            {
                                if (projects.FirstOrDefault().MstCheckLists.Where(d => d.Status == "ACTIVE" && d.IsLocked == true).Count() > 0)
                                {
                                    checklistId = projects.FirstOrDefault().MstCheckLists.Where(d => d.Status == "ACTIVE" && d.IsLocked == true).FirstOrDefault().Id;
                                }
                            }

                            String totalInvestment = "";
                            String paymentOptions = "";
                            String financing = "";

                            var settings = from d in db.SysSettings select d;
                            if (settings.Any())
                            {
                                totalInvestment = settings.FirstOrDefault().TotalInvestment;
                                paymentOptions = settings.FirstOrDefault().PaymentOptions;
                                financing = settings.FirstOrDefault().Financing;
                            }


                            if (projectId > 0 && unitId > 0 && checklistId > 0 && customerId > 0 && brokerId > 0)
                            {
                                Data.TrnSoldUnit newTrnSoldUnit = new Data.TrnSoldUnit()
                                {
                                    SoldUnitNumber = soldUnitNumber,
                                    SoldUnitDate = Convert.ToDateTime(soldUnit.SoldUnitDate),

                                    ProjectId = projectId,
                                    UnitId = unitId,
                                    CustomerId = customerId,
                                    BrokerId = brokerId,
                                    Agent = soldUnit.Agent,
                                    BrokerCoordinator = soldUnit.BrokerCoordinator,
                                    CheckListId = checklistId,

                                    Price = price,

                                    EquityValue = 0,
                                    EquityPercent = 0,
                                    Discount = 0,
                                    Reservation = 0,
                                    NetEquity = 0,
                                    NetEquityInterest = 0,
                                    NetEquityNoOfPayments = 0,
                                    NetEquityAmortization = 0,
                                    Balance = 0,
                                    BalanceInterest = 0,
                                    BalanceNoOfPayments = 0,
                                    BalanceAmortization = 0,

                                    TotalInvestment = totalInvestment,
                                    PaymentOptions = paymentOptions,
                                    Financing = financing,

                                    Remarks = soldUnit.Remarks,

                                    PreparedBy = currentUser.FirstOrDefault().Id,
                                    CheckedBy = currentUser.FirstOrDefault().Id,
                                    ApprovedBy = currentUser.FirstOrDefault().Id,
                                    Status = soldUnit.Status,

                                    IsLocked = false,
                                    CreatedBy = currentUser.FirstOrDefault().Id,
                                    CreatedDateTime = DateTime.Now,
                                    UpdatedBy = currentUser.FirstOrDefault().Id,
                                    UpdatedDateTime = DateTime.Now
                                };

                                db.TrnSoldUnits.InsertOnSubmit(newTrnSoldUnit);
                                db.SubmitChanges();

                                return newTrnSoldUnit.Id;
                            }
                            else
                            {
                                return 0;
                            }
                         
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

    }
}
