using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class Contact
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }        
        public string platform { get; set; }
        public int group_id { get; set; }
        public string project { get; set; }      
        public string group_name { get; set; }
        public byte is_fyp_1 { get; set; }
        public string message { get; set; }
        public string message_time { get; set; }
        public string profile_pic { get; set; }
        public byte is_allowed_in_meeting { set; get; }
    }
}