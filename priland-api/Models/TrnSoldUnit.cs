﻿using System;
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
        public String Project { get; set; }
        public Int32 UnitId { get; set; }
        public String Unit { get; set; }
        public Int32 CustomerId { get; set; }
        public String Customer { get; set; }
        public Int32 BrokerId { get; set; }
        public String Broker { get; set; }
        public Int32 ChecklistId { get; set; }
        public String Checklist { get; set; }
        public Decimal Price { get; set; }
        public String TotalInvestment { get; set; }
        public String PaymentOptions { get; set; }
        public String Financing { get; set; }
        public String Remarks { get; set; }
        public Int32 PreparedBy { get; set; }
        public String PreparedByUser { get; set; }
        public Int32 CheckedBy { get; set; }
        public String CheckedByUser { get; set; }
        public Int32 ApprovedBy { get; set; }
        public String ApprovedByUser { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}