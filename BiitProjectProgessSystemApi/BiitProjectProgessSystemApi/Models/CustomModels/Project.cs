using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class Project
    {
        public int id { get; set; }
        public int supervisor_id { get; set; }
        public string title { get; set; }
        public string supervisor_name { get; set; }
        public string description { get; set; }

    }
}