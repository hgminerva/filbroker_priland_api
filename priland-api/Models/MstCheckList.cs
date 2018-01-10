using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priland_api.Models
{
    public class MstCheckList
    {
        public Int32 Id { get; set; }
        public String ChecklistCode { get; set; }
        public String Checklist { get; set; }
        public String ChecklistDate { get; set; }
        public Int32 ProjectId { get; set; }
        public String Project { get; set; }
        public String Remarks { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedBy { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedBy { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}