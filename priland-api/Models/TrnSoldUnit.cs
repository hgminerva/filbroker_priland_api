using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class TrnSoldUnit
    {
        public Int32 Id { get; set; }
        public String SoldUnitNumber { get; set; }
        public String SoldUnitDate { get; set; }
        public Int32 ProjectId { get; set; }
        public Int32 UnitId { get; set; }
        public Int32 CustomerId { get; set; }
        public Int32 BrokerId { get; set; }
        public Int32 CheckListId { get; set; }
        public Decimal Price { get; set; }
        public String TotalInvestment { get; set; }
        public String PaymentOptions { get; set; }
        public String Financing { get; set; }
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