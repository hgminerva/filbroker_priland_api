using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class MstUnitPrice
    {
        public Int32 Id { get; set; }
        public Int32 UnitId { get; set; }
        public String PriceDate { get; set; }
        public Decimal Price { get; set; }
    }
}