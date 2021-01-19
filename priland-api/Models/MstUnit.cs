using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class MstUnit
    {
        public Int32 Id { get; set; }
        public String UnitCode { get; set; }
        public String Block { get; set; }
        public String Lot { get; set; }
        public Int32 ProjectId { get; set; }
        public String Project { get; set; }
        public Int32 HouseModelId { get; set; }
        public String HouseModel { get; set; }
        public Decimal TLA { get; set; }
        public Decimal TFA { get; set; }
        public Decimal Price { get; set; }
        public Decimal MiscellaneousFeeRate { get; set; }
        public Decimal MiscellaneousFeeAmount { get; set; }
        public Decimal VATRate { get; set; }
        public Decimal VATAmount { get; set; }
        public Decimal TSP { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
        
        // Report purposes.
        public String Customer { get; set; }
    }
}