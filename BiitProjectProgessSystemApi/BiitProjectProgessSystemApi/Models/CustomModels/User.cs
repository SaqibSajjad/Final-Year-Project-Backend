using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class User
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string cgpa { get; set; }
        public string address { get; set; }
        public string platform { get; set; }
        public int group_id { get; set; }
        public string project { get; set; }
        public string supervisor { get; set; }
        public string semester { get; set; }
        public string group_name { get; set; }
        public string profile_pic { get; set; }

        public byte is_present { get; set; }
        public byte is_fyp_1 { get; set; }
        public byte is_allowed_in_meeting  { set; get; }

        public String fyp_1_final_grade { set; get; }
        public String fyp_2_final_grade { set; get; }

    }
}