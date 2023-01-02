using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class TaskModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string task_deadline { get; set; }
        public byte isFinalTask { get; set; }
        public string assigned_by { get; set; }
        public int assigned_by_id { get; set; }
        public string authority_role { get; set; }
        public int assigned_to { get; set; }
        public int rating { get; set; }
        public string status { get; set; }
        public int status_id { get; set; }
        public string file_path { get; set; }
    }
}