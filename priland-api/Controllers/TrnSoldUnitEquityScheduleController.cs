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
    [RoutePrefix("api/TrnSoldUnitEquitySchedule")]
    public class TrnSoldUnitEquityScheduleController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        private String formatNullableDate(DateTime? nullableDate)
        {
            if (nullableDate.HasValue)
            {
                return nullableDate.Value.ToShortDateString();
            }
            else
            {
                return "";
            }
        }

        // List
        [HttpGet, Route("List")]
        public List<TrnSoldUnitEquitySchedule> GetTrnSoldUnitEquitySchedule()
        {
            var TrnSoldUnitEquityScheduleData = from d in db.TrnSoldUnitEquitySchedules
                                                orderby d.PaymentDate ascending
                                                select new TrnSoldUnitEquitySchedule
                                                {
                                                    Id = d.Id,
                                                    SoldUnitId = d.SoldUnitId,
                                                    PaymentDate = Convert.ToString(d.PaymentDate.Year) + "-" + Convert.ToString(d.PaymentDate.Month + 100).Substring(1, 2) + "-" + Convert.ToString(d.PaymentDate.Day + 100).Substring(1, 2),
                                                    Amortization = d.Amortization,
                                                    CheckNumber = d.CheckNumber,
                                                    CheckDate = formatNullableDate(d.CheckDate),
                                                    CheckBank = d.CheckBank,
                                                    Remarks = d.Remarks,
                                                    PaidAmount = d.PaidAmount,
                                                    BalanceAmount = d.BalanceAmount
                                                };

            return TrnSoldUnitEquityScheduleData.ToList();
        }

        // List per sold unit
        [HttpGet, Route("ListPerUnitSold/{id}")]
        public List<TrnSoldUnitEquitySchedule> GetTrnSoldUnitEquitySchedulePerUnitSold(string id)
        {
            var TrnSoldUnitEquityScheduleData = from d in db.TrnSoldUnitEquitySchedules
                                                where d.SoldUnitId == Convert.ToInt32(id)
                                                orderby d.PaymentDate ascending
                                                select new TrnSoldUnitEquitySchedule
                                                {
                                                    Id = d.Id,
                                                    SoldUnitId = d.SoldUnitId,
                                                    PaymentDate = Convert.ToString(d.PaymentDate.Year) + "-" + Convert.ToString(d.PaymentDate.Month + 100).Substring(1, 2) + "-" + Convert.ToString(d.PaymentDate.Day + 100).Substring(1, 2),
                                                    Amortization = d.Amortization,
                                                    CheckNumber = d.CheckNumber,
                                                    CheckDate = formatNullableDate(d.CheckDate),
                                                    CheckBank = d.CheckBank,
                                                    Remarks = d.Remarks,
                                                    PaidAmount = d.PaidAmount,
                                                    BalanceAmount = d.BalanceAmount
                                                };

            return TrnSoldUnitEquityScheduleData.ToList();
        }

        // List per sold unit
        [HttpGet, Route("Detail/{id}")]
        public TrnSoldUnitEquitySchedule GetEquityScheduleDetail(string id)
        {
            var TrnSoldUnitEquityScheduleData = from d in db.TrnSoldUnitEquitySchedules
                                                where d.Id == Convert.ToInt32(id)
                                                select new TrnSoldUnitEquitySchedule
                                                {
                                                    Id = d.Id,
                                                    SoldUnitId = d.SoldUnitId,
                                                    PaymentDate = Convert.ToString(d.PaymentDate.Year) + "-" + Convert.ToString(d.PaymentDate.Month + 100).Substring(1, 2) + "-" + Convert.ToString(d.PaymentDate.Day + 100).Substring(1, 2),
                                                    Amortization = d.Amortization,
                                                    CheckNumber = d.CheckNumber,
                                                    CheckDate = formatNullableDate(d.CheckDate),
                                                    CheckBank = d.CheckBank,
                                                    Remarks = d.Remarks,
                                                    PaidAmount = d.PaidAmount,
                                                    BalanceAmount = d.BalanceAmount
                                                };

            return TrnSoldUnitEquityScheduleData.FirstOrDefault();
        }

        // New Sold Unit Equity Schedule per sold unit id
        [HttpGet, Route("ListNewTrnSoldUnitEquitySchedule/{id}")]
        public HttpResponseMessage GetNewTrnSoldUnitEquitySchedule(string id)
        {
            try
            {
                var soldUnits = from d in db.TrnSoldUnits
                                where d.Id == Convert.ToInt32(id)
                                select d;
                if (soldUnits.Any())
                {
                    // Remove existing schedule
                    var deleteSchedules = from d in db.TrnSoldUnitEquitySchedules
                                          where d.SoldUnitId == Convert.ToInt32(id)
                                          select d;

                    if (deleteSchedules.Any())
                    {
                        foreach (var deleteSchedule in deleteSchedules)
                        {
                            db.TrnSoldUnitEquitySchedules.DeleteOnSubmit(deleteSchedule);
                        }
                        db.SubmitChanges();
                    }

                    Data.TrnSoldUnitEquitySchedule insertReservationSchedule = new Data.TrnSoldUnitEquitySchedule()
                    {
                        SoldUnitId = Convert.ToInt32(id),
                        PaymentDate = soldUnits.FirstOrDefault().SoldUnitDate,
                        Amortization = soldUnits.FirstOrDefault().Reservation,
                        PaidAmount = 0,
                        BalanceAmount = soldUnits.FirstOrDefault().Reservation,
                        Remarks = "Reservation Fee"
                    };

                    db.TrnSoldUnitEquitySchedules.InsertOnSubmit(insertReservationSchedule);
                    db.SubmitChanges();

                    Data.TrnSoldUnitEquitySchedule insertDownpaymentSchedule = new Data.TrnSoldUnitEquitySchedule()
                    {
                        SoldUnitId = Convert.ToInt32(id),
                        PaymentDate = soldUnits.FirstOrDefault().SoldUnitDate,
                        Amortization = soldUnits.FirstOrDefault().DownpaymentValue,
                        PaidAmount = 0,
                        BalanceAmount = soldUnits.FirstOrDefault().DownpaymentValue,
                        Remarks = "Downpayment"
                    };

                    db.TrnSoldUnitEquitySchedules.InsertOnSubmit(insertDownpaymentSchedule);
                    db.SubmitChanges();

                    // Insert new schedules
                    Int32 noOfPayments = Decimal.ToInt32(soldUnits.FirstOrDefault().NetEquityNoOfPayments);
                    decimal amortization = soldUnits.FirstOrDefault().NetEquityAmortization;
                    DateTime startDate = soldUnits.FirstOrDefault().SoldUnitDate;
                    decimal equitySpotPayment1 = soldUnits.FirstOrDefault().EquitySpotPayment1;
                    decimal equitySpotPayment2 = soldUnits.FirstOrDefault().EquitySpotPayment2;
                    decimal equitySpotPayment3 = soldUnits.FirstOrDefault().EquitySpotPayment3;

                    for (Int32 p = 1; p <= noOfPayments; p++)
                    {
                        DateTime paymentDate = startDate.AddMonths(p);
                        decimal monthlyAmortization = amortization;

                        if (equitySpotPayment1 > 0 && p == 1) monthlyAmortization = equitySpotPayment1;
                        if (equitySpotPayment2 > 0 && p == noOfPayments / 2) monthlyAmortization = equitySpotPayment2;
                        if (equitySpotPayment3 > 0 && p == noOfPayments) monthlyAmortization = equitySpotPayment3;

                        Data.TrnSoldUnitEquitySchedule insertSchedule = new Data.TrnSoldUnitEquitySchedule()
                        {
                            SoldUnitId = Convert.ToInt32(id),
                            PaymentDate = paymentDate,
                            Amortization = monthlyAmortization,
                            PaidAmount = 0,
                            BalanceAmount = monthlyAmortization,
                            Remarks = "Equity " + p
                        };

                        db.TrnSoldUnitEquitySchedules.InsertOnSubmit(insertSchedule);
                        db.SubmitChanges();

                        if (p == noOfPayments)
                        {
                            Data.TrnSoldUnitEquitySchedule insertMiscFeeSchedule = new Data.TrnSoldUnitEquitySchedule()
                            {
                                SoldUnitId = Convert.ToInt32(id),
                                PaymentDate = paymentDate.AddMonths(p + 1),
                                Amortization = soldUnits.FirstOrDefault().MiscellaneousFeeAmount,
                                PaidAmount = 0,
                                BalanceAmount = soldUnits.FirstOrDefault().MiscellaneousFeeAmount,
                                Remarks = "Miscellaneous Fee"
                            };

                            db.TrnSoldUnitEquitySchedules.InsertOnSubmit(insertMiscFeeSchedule);
                            db.SubmitChanges();

                            Data.TrnSoldUnitEquitySchedule insertBalanceSchedule = new Data.TrnSoldUnitEquitySchedule()
                            {
                                SoldUnitId = Convert.ToInt32(id),
                                PaymentDate = paymentDate.AddMonths(p + 1),
                                Amortization = soldUnits.FirstOrDefault().Balance,
                                PaidAmount = 0,
                                BalanceAmount = soldUnits.FirstOrDefault().Balance,
                                Remarks = "Balance"
                            };

                            db.TrnSoldUnitEquitySchedules.InsertOnSubmit(insertBalanceSchedule);
                            db.SubmitChanges();
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Import Sold Unit Equity Schedule per sold unit id
        [HttpPost, Route("ImportNewTrnSoldUnitEquitySchedule/{id}")]
        public List<TrnSoldUnitEquitySchedule> PostImportTrnSoldUnitEquitySchedule(List<TrnSoldUnitEquitySchedule> equitySchedules, string id)
        {
            var soldUnits = from d in db.TrnSoldUnits
                            where d.Id == Convert.ToInt32(id)
                            select d;
            if (soldUnits.Any())
            {
                // Remove existing schedule
                var deleteSchedules = from d in db.TrnSoldUnitEquitySchedules
                                      where d.SoldUnitId == Convert.ToInt32(id)
                                      select d;

                if (deleteSchedules.Any())
                {
                    foreach (var deleteSchedule in deleteSchedules)
                    {
                        db.TrnSoldUnitEquitySchedules.DeleteOnSubmit(deleteSchedule);
                    }
                    db.SubmitChanges();
                }

                // Insert new schedules
                if (equitySchedules.Any())
                {
                    foreach (var equitySchedule in equitySchedules)
                    {
                        Data.TrnSoldUnitEquitySchedule insertSchedule = new Data.TrnSoldUnitEquitySchedule()
                        {
                            SoldUnitId = Convert.ToInt32(id),
                            PaymentDate = Convert.ToDateTime(equitySchedule.PaymentDate),
                            Amortization = equitySchedule.Amortization,
                            CheckNumber = equitySchedule.CheckNumber,
                            CheckDate = Convert.ToDateTime(equitySchedule.CheckDate),
                            CheckBank = equitySchedule.CheckBank,
                            Remarks = equitySchedule.Remarks
                        };

                        db.TrnSoldUnitEquitySchedules.InsertOnSubmit(insertSchedule);
                    }
                }

                db.SubmitChanges();
            }

            // Return new requirements
            var TrnSoldUnitEquityScheduleData = from d in db.TrnSoldUnitEquitySchedules
                                                where d.SoldUnitId == Convert.ToInt32(id)
                                                orderby d.PaymentDate ascending
                                                select new TrnSoldUnitEquitySchedule
                                                {
                                                    Id = d.Id,
                                                    SoldUnitId = d.SoldUnitId,
                                                    PaymentDate = Convert.ToString(d.PaymentDate.Year) + "-" + Convert.ToString(d.PaymentDate.Month + 100).Substring(1, 2) + "-" + Convert.ToString(d.PaymentDate.Day + 100).Substring(1, 2),
                                                    Amortization = d.Amortization,
                                                    CheckNumber = d.CheckNumber == null ? "" : d.CheckNumber,
                                                    CheckDate = formatNullableDate(d.CheckDate),
                                                    CheckBank = d.CheckBank == null ? "" : d.CheckBank,
                                                    Remarks = d.Remarks
                                                };
            return TrnSoldUnitEquityScheduleData.ToList();
        }

        // Save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveTrnSoldUnitEquitySchedule(TrnSoldUnitEquitySchedule soldUnitEquitySchedule)
        {
            try
            {
                var soldUnitSchedules = from d in db.TrnSoldUnitEquitySchedules
                                        where d.Id == Convert.ToInt32(soldUnitEquitySchedule.Id)
                                        select d;

                if (soldUnitSchedules.Any())
                {
                    var updateSoldUnitSchedule = soldUnitSchedules.FirstOrDefault();

                    updateSoldUnitSchedule.PaymentDate = Convert.ToDateTime(soldUnitEquitySchedule.PaymentDate);
                    updateSoldUnitSchedule.Amortization = soldUnitEquitySchedule.Amortization;
                    updateSoldUnitSchedule.CheckNumber = soldUnitEquitySchedule.CheckNumber;
                    updateSoldUnitSchedule.CheckDate = Convert.ToDateTime(soldUnitEquitySchedule.CheckDate);
                    updateSoldUnitSchedule.CheckBank = soldUnitEquitySchedule.CheckBank;
                    updateSoldUnitSchedule.Remarks = soldUnitEquitySchedule.Remarks;

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
