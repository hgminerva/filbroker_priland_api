using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class TrnSoldUnitRequirementActivity
    {
        public Int32 Id { get; set; }
        public Int32 SoldUnitRequirementId { get; set; }
        public String ActivityDate { get; set; }
        public String Activity { get; set; }
        public String Remarks { get; set; }
        public Int32 UserId { get; set; }
        public String User { get; set; }
    }
}