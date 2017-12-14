using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class MstHouseModel
    {
        public Int32 Id { get; set; }
        public String HouseModelCode { get; set; }
        public String HouseModel { get; set; }
        public Int32 ProjectId { get; set; }
        public Decimal TFA { get; set; }
        public Decimal Price { get; set; }
        public Int32 CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}