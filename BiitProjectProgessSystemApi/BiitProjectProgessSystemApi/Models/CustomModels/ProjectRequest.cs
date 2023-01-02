using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class ProjectRequest
    {
        public int id { get; set; }
        public int group_id { get; set; }
        public string group_name { get; set; }
        public int project_id { get; set; }
        public string project_name { get; set; }
        public string project_desccription { get; set; }    

        public int supervisor_id { get; set; }
        public string supervisor_name { get; set; }
    }
}