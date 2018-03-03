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

        // List
        [HttpGet, Route("List")]
        public List<TrnSoldUnitEquitySchedule> GetTrnSoldUnitEquitySchedule()
        {
            var TrnSoldUnitEquityScheduleData = from d in db.TrnSoldUnitEquitySchedules
                                                orderby d.PaymentDate descending
                                                select new TrnSoldUnitEquitySchedule
                                                {
                                                    Id = d.Id,
                                                    SoldUnitId = d.SoldUnitId,
                                                    PaymentDate = d.PaymentDate.ToShortDateString(),
                                                    Amortization = d.Amortization,
                                                    CheckNumber = d.CheckNumber,
                                                    CheckDate = d.CheckDate == null ? "" : d.CheckDate.Value.ToShortDateString(),
                                                    CheckBank = d.CheckBank
                                                };
            return TrnSoldUnitEquityScheduleData.ToList();
        }

        // List per sold unit
        [HttpGet, Route("ListPerUnitSold/{id}")]
        public List<TrnSoldUnitEquitySchedule> GetTrnSoldUnitEquitySchedulePerUnitSold(string id)
        {
            var TrnSoldUnitEquityScheduleData = from d in db.TrnSoldUnitEquitySchedules
                                                where d.SoldUnitId == Convert.ToInt32(id)
                                                orderby d.PaymentDate descending
                                                select new TrnSoldUnitEquitySchedule
                                                 {
                                                     Id = d.Id,
                                                     SoldUnitId = d.SoldUnitId,
                                                     PaymentDate = d.PaymentDate.ToShortDateString(),
                                                     Amortization = d.Amortization,
                                                     CheckNumber = d.CheckNumber,
                                                     CheckDate = d.CheckDate == null ? "" : d.CheckDate.Value.ToShortDateString(),
                                                     CheckBank = d.CheckBank
                                                 };
            return TrnSoldUnitEquityScheduleData.ToList();
        }

        // New Sold Unit Equity Schedule per sold unit id
        [HttpGet, Route("ListNewTrnSoldUnitEquitySchedule/{id}")]
        public List<TrnSoldUnitEquitySchedule> GetNewTrnSoldUnitEquitySchedule(string id)
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
                Int32 noOfPayments = Decimal.ToInt32(soldUnits.FirstOrDefault().NetEquityNoOfPayments);
                decimal amortization = soldUnits.FirstOrDefault().NetEquityAmortization;
                DateTime startDate = soldUnits.FirstOrDefault().SoldUnitDate;

                for (Int32 p = 1; p <= noOfPayments; p++)
                {
                    DateTime paymentDate = startDate.AddMonths(p);
                    Data.TrnSoldUnitEquitySchedule insertSchedule = new Data.TrnSoldUnitEquitySchedule()
                    {
                        SoldUnitId = Convert.ToInt32(id),
                        PaymentDate = paymentDate,
                        Amortization = amortization
                    };

                    db.TrnSoldUnitEquitySchedules.InsertOnSubmit(insertSchedule);
                }
                db.SubmitChanges();
            }

            // Return new requirements
            var TrnSoldUnitEquityScheduleData = from d in db.TrnSoldUnitEquitySchedules
                                                where d.SoldUnitId == Convert.ToInt32(id)
                                                orderby d.PaymentDate descending
                                                select new TrnSoldUnitEquitySchedule
                                                {
                                                    Id = d.Id,
                                                    SoldUnitId = d.SoldUnitId,
                                                    PaymentDate = d.PaymentDate.ToShortDateString(),
                                                    Amortization = d.Amortization,
                                                    CheckNumber = d.CheckNumber,
                                                    CheckDate = d.CheckDate == null ? "" : d.CheckDate.Value.ToShortDateString(),
                                                    CheckBank = d.CheckBank
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
                    updateSoldUnitSchedule.CheckDate =  Convert.ToDateTime(soldUnitEquitySchedule.CheckDate);
                    updateSoldUnitSchedule.CheckBank = soldUnitEquitySchedule.CheckBank;

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
