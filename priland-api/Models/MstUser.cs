using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class MstUser
    {
        public Int32 Id { get; set; }
        public String Username {get;set ;}
        public String FullName { get; set; }
        public String Password { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public String AspNetId { get; set; }
    }
}