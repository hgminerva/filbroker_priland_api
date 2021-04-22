using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using priland_api.Models;

namespace priland_api.Controllers
{
    [Authorize]
    [RoutePrefix("api/RepSummary")]
    public class RepSummaryController : ApiController
    {
        private Data.FilbrokerDBDataContext db = new Data.FilbrokerDBDataContext();

        // sold unit list per date range
        [HttpGet, Route("ListSoldUnitPerDates/{dateStart}/{dateEnd}")]
        public List<TrnSoldUnit> GetTrnSoldUnitPerDates(string dateStart, string dateEnd)
        {
            var TrnSoldUnitData = from d in db.TrnSoldUnits
                                  where d.SoldUnitDate >= Convert.ToDateTime(dateStart) &&
                                        d.SoldUnitDate <= Convert.ToDateTime(dateEnd)
                                  orderby d.SoldUnitDate, d.SoldUnitNumber ascending
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
                                      Customer = d.MstCustomer.LastName + ", " + d.MstCustomer.FirstName + " " + d.MstCustomer.MiddleName,
                                      BrokerId = d.BrokerId,
                                      Broker = d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName + " (" + d.MstBroker.RealtyFirm + ")",
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
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                                      PricePayment = d.PricePayment != null ? d.PricePayment : 0,
                                      PriceBalance = d.PriceBalance != null ? d.PriceBalance : 0
                                  };
            return TrnSoldUnitData.ToList();
        }

        // sold unit list per date as of
        [HttpGet, Route("ListSoldUnitPerDateAsOf/{dateAsOf}")]
        public List<TrnSoldUnit> GetTrnSoldUnitPerDateAsOf(string dateAsOf)
        {
            var TrnSoldUnitData = from d in db.TrnSoldUnits
                                  where d.SoldUnitDate <= Convert.ToDateTime(dateAsOf)
                                  orderby d.SoldUnitDate, d.SoldUnitNumber ascending
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
                                      Customer = d.MstCustomer.LastName + ", " + d.MstCustomer.FirstName + " " + d.MstCustomer.MiddleName,
                                      BrokerId = d.BrokerId,
                                      Broker = d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName + " (" + d.MstBroker.RealtyFirm + ")",
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
                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                                      PricePayment = d.PricePayment != null ? d.PricePayment : 0,
                                      PriceBalance = d.PriceBalance != null ? d.PriceBalance : 0
                                  };
            return TrnSoldUnitData.ToList();
        }

        // sold unit checklist requirements per date range
        [HttpGet, Route("ListSoldUnitChecklistPerDates/{dateStart}/{dateEnd}")]
        public List<TrnSoldUnitRequirement> GetTrnSoldUnitChecklistPerDates(string dateStart, string dateEnd)
        {
            var TrnSoldUnitRequirementData = from d in db.TrnSoldUnitRequirements
                                             where d.TrnSoldUnit.SoldUnitDate >= Convert.ToDateTime(dateStart) &&
                                                   d.TrnSoldUnit.SoldUnitDate <= Convert.ToDateTime(dateEnd)
                                             orderby d.TrnSoldUnit.SoldUnitDate, d.TrnSoldUnit.SoldUnitNumber, d.MstCheckListRequirement.RequirementNo ascending
                                             select new TrnSoldUnitRequirement
                                             {
                                                 Id = d.Id,
                                                 SoldUnitId = d.SoldUnitId,
                                                 ChecklistRequirementId = d.MstCheckListRequirement.Id,
                                                 ChecklistRequirement = d.MstCheckListRequirement.Requirement,
                                                 ChecklistRequirementNo = d.MstCheckListRequirement.RequirementNo,
                                                 ChecklistCategory = d.MstCheckListRequirement.Category,
                                                 ChecklistType = d.MstCheckListRequirement.Type,
                                                 ChecklistWithAttachments = d.MstCheckListRequirement.WithAttachments,
                                                 Attachment1 = d.Attachment1,
                                                 Attachment2 = d.Attachment2,
                                                 Attachment3 = d.Attachment3,
                                                 Attachment4 = d.Attachment4,
                                                 Attachment5 = d.Attachment5,
                                                 Remarks = d.Remarks,
                                                 Status = d.Status,
                                                 StatusDate = d.StatusDate.ToShortDateString(),
                                                 SoldUnitNumber = d.TrnSoldUnit.SoldUnitNumber,
                                                 SoldUnitDate = d.TrnSoldUnit.SoldUnitDate.ToShortDateString(),
                                                 Project = d.TrnSoldUnit.MstProject.Project,
                                                 Unit = d.TrnSoldUnit.MstUnit.UnitCode,
                                                 Customer = d.TrnSoldUnit.MstCustomer.LastName + ", " + d.TrnSoldUnit.MstCustomer.FirstName + " " + d.TrnSoldUnit.MstCustomer.MiddleName
                                             };
            return TrnSoldUnitRequirementData.ToList();
        }

        // commission request per date range
        [HttpGet, Route("ListCommissionRequestPerDates/{dateStart}/{dateEnd}")]
        public List<TrnCommissionRequest> GetTrnCommissionRequestPerDates(string dateStart, string dateEnd)
        {
            var TrnCommissionRequestData = from d in db.TrnCommissionRequests
                                           where d.CommissionRequestDate >= Convert.ToDateTime(dateStart) &&
                                                 d.CommissionRequestDate <= Convert.ToDateTime(dateEnd)
                                           orderby d.CommissionRequestDate, d.CommissionNumber ascending
                                           select new Models.TrnCommissionRequest
                                           {
                                               Id = d.Id,
                                               CommissionRequestNumber = d.CommissionRequestNumber,
                                               CommissionRequestDate = d.CommissionRequestDate.ToShortDateString(),
                                               BrokerId = d.BrokerId,
                                               Broker = d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName,
                                               SoldUnitId = d.SoldUnitId,
                                               SoldUnit = d.TrnSoldUnit.SoldUnitNumber,
                                               CommissionNumber = d.CommissionRequestNumber,
                                               Amount = d.Amount,
                                               Remarks = d.Remarks,
                                               PreparedBy = d.PreparedBy,
                                               PrepearedByUser = d.MstUser2.Username,
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
            return TrnCommissionRequestData.ToList();
        }

        // sold unit checklist requirement activities per date range
        [HttpGet, Route("ListSoldUnitRequirementActivityPerDates/{dateStart}/{dateEnd}")]
        public List<TrnSoldUnitRequirementActivity> GetTrnSoldUnitRequirementActivityPerDates(string dateStart, string dateEnd)
        {
            var TrnSoldUnitRequirementActivities = from d in db.TrnSoldUnitRequirementActivities
                                                   where d.ActivityDate >= Convert.ToDateTime(dateStart) &&
                                                         d.ActivityDate <= Convert.ToDateTime(dateEnd)
                                                   orderby d.ActivityDate, d.Id ascending
                                                   select new TrnSoldUnitRequirementActivity
                                                   {
                                                       Id = d.Id,
                                                       SoldUnitRequirementId = d.SoldUnitRequirementId,
                                                       ChecklistRequirement = d.TrnSoldUnitRequirement.MstCheckListRequirement.Requirement,
                                                       SoldUnitNumber = d.TrnSoldUnitRequirement.TrnSoldUnit.SoldUnitNumber,
                                                       Project = d.TrnSoldUnitRequirement.TrnSoldUnit.MstProject.Project,
                                                       UnitCode = d.TrnSoldUnitRequirement.TrnSoldUnit.MstUnit.UnitCode,
                                                       Customer = d.TrnSoldUnitRequirement.TrnSoldUnit.MstCustomer.LastName + ", " + d.TrnSoldUnitRequirement.TrnSoldUnit.MstCustomer.FirstName + " " + d.TrnSoldUnitRequirement.TrnSoldUnit.MstCustomer.MiddleName,
                                                       ActivityDate = d.ActivityDate.ToShortDateString(),
                                                       Activity = d.Activity,
                                                       Remarks = d.Remarks,
                                                       User = d.MstUser.FullName
                                                   };
            return TrnSoldUnitRequirementActivities.ToList();
        }

        // Customer List Filtered By UpdateDateTime
        [HttpGet, Route("ListCustomerFilterByUpdateDateTime/{dateStart}/{dateEnd}")]
        public List<MstCustomer> GetCustomerListFilterByUpdateDateTime(string dateStart, string dateEnd)
        {
            var MstCustomerList = from d in db.MstCustomers
                                  where d.UpdatedDateTime >= Convert.ToDateTime(dateStart) &&
                                        d.UpdatedDateTime <= Convert.ToDateTime(dateEnd)
                                  select new MstCustomer
                                  {
                                      Id = d.Id,
                                      CustomerCode = d.CustomerCode,
                                      LastName = d.LastName,
                                      FirstName = d.FirstName,
                                      Gender = d.Gender,
                                      Address = d.Address,
                                      EmailAddress = d.EmailAddress,
                                      TelephoneNumber = d.TelephoneNumber
                                  };

            return MstCustomerList.ToList();
        }

        // Broker List Filtered By UpdateDateTime
        [HttpGet, Route("ListBrokerFilterByUpdateDateTime/{dateStart}/{dateEnd}")]
        public List<MstBroker> GetBrokerListFilterByUpdateDateTime(string dateStart, string dateEnd)
        {
            var MstBrokerList = from d in db.MstBrokers
                                where d.UpdatedDateTime >= Convert.ToDateTime(dateStart) &&
                                      d.UpdatedDateTime <= Convert.ToDateTime(dateEnd)
                                select new MstBroker
                                {
                                    Id = d.Id,
                                    BrokerCode = d.BrokerCode,
                                    LastName = d.LastName,
                                    FirstName = d.FirstName,
                                    Gender = d.Gender,
                                    Address = d.Address,
                                    EmailAddress = d.EmailAddress,
                                    TelephoneNumber = d.TelephoneNumber
                                };

            return MstBrokerList.ToList();
        }

        // capital gains per date range
        [HttpGet, Route("ListCapitalGains/{dateStart}/{dateEnd}")]
        public List<TrnSoldUnit> GetTrnSoldUnitForCapitalGains(string dateStart, string dateEnd)
        {
            List<TrnSoldUnit> soldUnits = new List<TrnSoldUnit>();

            var collectionPaymentData = from d in db.TrnCollectionPayments
                                        where d.TrnCollection.CollectionDate >= Convert.ToDateTime(dateStart)
                                        && d.TrnCollection.CollectionDate <= Convert.ToDateTime(dateEnd)
                                        && d.TrnCollection.IsLocked == true
                                        group d by new
                                        {
                                            d.SoldUnitId,
                                            SoldUnit = d.TrnSoldUnit,
                                        } into g
                                        select g;

            if (collectionPaymentData.ToList().Any())
            {
                foreach (var collectionData in collectionPaymentData.ToList())
                {
                    var data = collectionData.Key;

                    Decimal ratio = 0;

                    if (data.SoldUnit.Price > 0)
                    {
                        if (data.SoldUnit.PricePayment != null)
                        {
                            ratio = Math.Round((Convert.ToDecimal(data.SoldUnit.PricePayment) / data.SoldUnit.Price) * 100, MidpointRounding.AwayFromZero);
                        }
                    }

                    if (ratio > 25)
                    {
                        String lastPaymentDate = "";

                        var equitySchedule = from d in db.TrnSoldUnitEquitySchedules
                                             where d.SoldUnitId == data.SoldUnitId
                                             && d.PaidAmount > 0
                                             orderby d.Id descending
                                             select d;

                        if (equitySchedule.Any())
                        {
                            lastPaymentDate = equitySchedule.FirstOrDefault().PaymentDate.ToShortDateString();
                        }

                        Decimal collectedAmount = collectionData.Sum(d => d.Amount);

                        soldUnits.Add(new TrnSoldUnit()
                        {
                            SoldUnitNumber = data.SoldUnit.SoldUnitNumber,
                            Customer = data.SoldUnit.MstCustomer.LastName + ", " + data.SoldUnit.MstCustomer.FirstName + " " + data.SoldUnit.MstCustomer.MiddleName,
                            Unit = data.SoldUnit.MstUnit.UnitCode,
                            TSP = data.SoldUnit.MstUnit.TSP,
                            PricePayment = collectedAmount,
                            LastPaymentDate = lastPaymentDate,
                            Ratio = ratio
                        });
                    }
                }
            }

            return soldUnits;
        }

        // PDC monitoring per date range
        [HttpGet, Route("ListPDCMonitoring/{dateStart}/{dateEnd}")]
        public List<TrnSoldUnitEquitySchedule> GetTrnSoldUnitPDCMonitoring(string dateStart, string dateEnd)
        {
            var TrnSoldUnitEquityScheduleData = from d in db.TrnSoldUnitEquitySchedules
                                                where d.TrnSoldUnit.SoldUnitDate >= Convert.ToDateTime(dateStart)
                                                && d.TrnSoldUnit.SoldUnitDate <= Convert.ToDateTime(dateEnd)
                                                && d.TrnSoldUnit.IsLocked == true
                                                && d.CheckNumber != ""
                                                select new TrnSoldUnitEquitySchedule
                                                {
                                                    Id = d.Id,
                                                    SoldUnitId = d.SoldUnitId,
                                                    SoldUnitNumber = d.TrnSoldUnit.SoldUnitNumber,
                                                    SoldUnitCustomer = d.TrnSoldUnit.MstCustomer.LastName + ", " + d.TrnSoldUnit.MstCustomer.FirstName + " " + d.TrnSoldUnit.MstCustomer.MiddleName,
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

    }
}
