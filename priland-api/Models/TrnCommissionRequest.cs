using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class TrnCommissionRequest
    {
        public Int32 Id { get; set; }
        public String CommissionRequestNumber { get; set; }
        public String CommissionRequestDate { get; set; }
        public Int32 BrokerId { get; set; }
        public Int32 SoldUnitId { get; set; }
        public String CommissionNumber { get; set; }
        public Decimal Amount { get; set; }
        public String Remarks { get; set; }
        public Int32 PreparedBy { get; set; }
        public Int32 CheckedBy { get; set; }
        public Int32 ApprovedBy { get; set; }
        public String Status { get; set; }
        public Int32 CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}