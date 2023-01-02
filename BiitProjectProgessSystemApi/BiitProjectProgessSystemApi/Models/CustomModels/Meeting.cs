using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class Meeting
    {
        public int id { get; set; }
        public string title { get; set; }
        public string meeting_notes { get; set; }
        public string meeting_timing { get; set; }
        public string location { get; set; }
        public byte isRecurring { get; set; }
        public byte is_fyp_1 { get; set; }
        public string scheduled_by { get; set; }
        public string authority_role { get; set; }
        public string group_name { get; set; }
        public int group_id { get; set; }
        public int scheduled_for { get; set; }
        public int status { get; set; }
        public int role_id { get; set; }
        public byte is_attended { get; set; }


    }
}