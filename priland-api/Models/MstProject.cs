using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class MstProject
    {
        public Int32 Id {get; set;}
        public String ProjectCode {get; set;}
        public String Project {get; set;}
        public String Address {get; set;}
        public String Status {get; set;}
        public String ProjectLogo { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedBy {get; set;}
        public String  CreatedDateTime {get; set;}
        public Int32 UpdatedBy {get; set;}
        public String  UpdatedDateTime { get; set; }
    }
}