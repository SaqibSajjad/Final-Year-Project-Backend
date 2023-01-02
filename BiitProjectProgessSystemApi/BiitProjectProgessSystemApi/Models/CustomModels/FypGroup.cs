using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class FypGroup
    {
        public int id { get; set; }
        public string  name { get; set; }
        public string description { get; set; }
        public int creator_id { get; set; }
        public string created_by { get; set; }
        public string isApproved { get; set; }
       public string supervisor { get; set; }
        public string creator_profile_pic { get; set; }
       public int supervisor_id { get; set; }
       public int project_id { get; set; }
        public byte is_fyp_1 { get; set; }
        public string project { get; set; }
        //public Double group_progress { get; set; }
        public int isProjectRequested { get; set; }




    }
}