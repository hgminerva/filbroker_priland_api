using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class MstCheckListRequirement
    {
        public Int32 Id { get; set; }
        public Int32 CheckListId { get; set; }
        public String CheckList { get; set; }
        public Int32 RequirementNo { get; set; }
        public String Requirement { get; set; }
        public String Category { get; set; }
        public String Type { get; set; }
        public Boolean WithAttachments { get; set; }
    }
}