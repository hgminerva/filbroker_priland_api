using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class TrnSoldUnitEquitySchedule
    {
        public Int32 Id { get; set; }
        public Int32 SoldUnitId { get; set; }
        public String SoldUnitNumber { get; set; }
        public String SoldUnitCustomer { get; set; }
        public String PaymentDate { get; set; }
        public Decimal Amortization { get; set; }
        public String CheckNumber { get; set; }
        public String CheckDate { get; set; }
        public String CheckBank { get; set; }
        public String Remarks { get; set; }
        public Decimal PaidAmount { get; set; }
        public Decimal BalanceAmount { get; set; }
    }
}