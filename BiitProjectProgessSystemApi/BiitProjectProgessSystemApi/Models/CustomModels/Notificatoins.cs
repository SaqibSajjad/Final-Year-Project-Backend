using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class Notificatoins
    {
        public int id { get; set; }
        public int meeting_id { get; set; }
        public int group_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string remarks_from { get; set; }
        public string remarks_for { get; set; }
        public int remarks_rating { get; set; }
        public string fyp_group { get; set; }
        public string project_name { get; set; }
        public string date { get; set; }
        public string notification_type { get; set; }
    }
}