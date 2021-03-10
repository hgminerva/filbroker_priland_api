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
        public String CollectionDate { get; set; }
        public String CollectionManualNumber { get; set; }
        public String CollectionCustomer { get; set; }
        public String CollectionPreparedBy { get; set; }
        public String Collection { get; set; }
        public Int32 SoldUnitId { get; set; }
        public String SoldUnit { get; set; }
        public Int32 SoldUnitEquityScheduleId { get; set; }
        public String SoldUnitEquitySchedule { get; set; }
        public String Project { get; set; }
        public String PayType { get; set; }
        public Decimal Amount { get; set; }
        public String CheckNumber { get; set; }
        public String CheckDate { get; set; }
        public String CheckBank { get; set; }
        public String OtherInformation { get; set; }
        public String Agent { get; set; }
        public Int32 BrokerId { get; set; }
        public String Broker { get; set; }

    }
}