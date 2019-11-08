using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class TrnCollection
    {
        public Int32 Id { get; set; }
        public String CollectionNumber { get; set; }
        public String CollectionDate { get; set; }
        public Int32 CustomerId { get; set; }
        public String Customer { get; set; }
        public String Particulars { get; set; }
        public Int32 PreparedBy { get; set; }
        public Int32 CheckedBy { get; set; }
        public Int32 ApprovedBy { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}