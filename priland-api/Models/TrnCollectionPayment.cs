using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class TrnCollectionPayment
    {
        public Int32 Id { get; set; }
        public Int32 CollectionId { get; set; }
        public Int32 SoldUnitId { get; set; }
        public String SoldUnit { get; set; }
        public String Project { get; set; }
        public String PayType { get; set; }
        public Decimal Amount { get; set; }
        public String CheckNumber { get; set; }
        public String CheckDate { get; set; }
        public String CheckBank { get; set; }
        public String OtherInformation { get; set; }
    }
}