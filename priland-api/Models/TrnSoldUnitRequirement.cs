using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class TrnSoldUnitRequirement
    {
        public Int32 Id { get; set; }
        public Int32 SoldUnitId { get; set; }
        public Int32 CheckListRequirementId { get; set; }
        public String CheckListRequirement { get; set; }
        public String Attachment1 { get; set; }
        public String Attachment2 { get; set; }
        public String Attachment3 { get; set; }
        public String Attachment4 { get; set; }
        public String Attachment5 { get; set; }
        public String Remarks { get; set; }
        public String Status { get; set; }
        public String StatusDate { get; set; }
    }
}