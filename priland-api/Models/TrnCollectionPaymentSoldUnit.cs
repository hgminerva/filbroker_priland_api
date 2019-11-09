using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class TrnCollectionPaymentSoldUnit
    {
        public Int32 Id { get; set; }
        public String SoldUnitNumber { get; set; }
        public String Project { get; set; }
        public String UnitCode { get; set; }
    }
}