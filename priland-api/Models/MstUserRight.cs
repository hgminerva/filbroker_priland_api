using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class MstUserRight
    {
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public Int32 PageId { get; set; }
        public String Page { get; set; }
        public Boolean CanEdit { get; set; }
        public Boolean CanSave { get; set; }
        public Boolean CanLock { get; set; }
        public Boolean CanUnLock { get; set; }
        public Boolean CanPrint { get; set; }
        public Boolean CanDelete { get; set; }
    }
}