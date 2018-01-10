using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class SysSettings
    {
        public Int32 Id { get; set; }
        public String Company { get; set; }
        public String SoftwareVersion { get; set; }
        public String SoftwareDeveloper { get; set; }
        public Int32 SoldUnitCheckedBy { get; set; }
        public Int32 SoldUnitApprovedBy { get; set; }
        public Int32 CommissionRequestCheckedBy { get; set; }
        public Int32 CommissionRequestApprovedBy { get; set; }
        public String ProposalFootNote { get; set; }
        public String BrokerFootNote { get; set; }
    }
}