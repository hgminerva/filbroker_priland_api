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
                                      Unit = d.MstUnit.Block + " " + d.MstUnit.Lot,
                                      CustomerId = d.CustomerId,
                                      Customer = d.MstCustomer.LastName + ", " + d.MstCustomer.FirstName + " " + d.MstCustomer.MiddleName,
                                      BrokerId = d.BrokerId,
                                      Broker = d.MstBroker.LastName + ", " + d.MstBroker.FirstName + " " + d.MstBroker.MiddleName,
                                      ChecklistId = d.CheckListId,
                                      Checklist = d.MstCheckList.CheckList,
                                      Price = d.Price,
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
    }
}
