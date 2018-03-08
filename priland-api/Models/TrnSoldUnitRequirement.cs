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
        public Int32 ChecklistRequirementId { get; set; }
        public String ChecklistRequirement { get; set; }
        public Int32 ChecklistRequirementNo { get; set; }
        public String ChecklistCategory { get; set; }
        public String ChecklistType { get; set; }
        public Boolean ChecklistWithAttachments { get; set; }
        public String Attachment1 { get; set; }
        public String Attachment2 { get; set; }
        public String Attachment3 { get; set; }
        public String Attachment4 { get; set; }
        public String Attachment5 { get; set; }
        public String Remarks { get; set; }
        public String Status { get; set; }
        public String StatusDate { get; set; }

        // Report purposes.
        public String SoldUnitNumber { get; set; }
        public String SoldUnitDate { get; set; }
        public String Project { get; set; }
        public String Unit { get; set; }
        public String Customer { get; set; }
    }
}