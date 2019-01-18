using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class TrnSoldUnitCoOwner
    {
        public Int32 Id { get; set; }
        public Int32 SoldUnitId { get; set; }
        public Int32 CustomerId { get; set; }
        public String CustomerCode { get; set; }
        public String Customer { get; set; }
    }
}