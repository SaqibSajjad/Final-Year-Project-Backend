using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class AssignTask
    {

        public int id { get; set; }
        public int group_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string task_deadline { get; set; }
        public byte isFinalTask { get; set; }
        public int assigned_by { get; set; }
        public int assigned_to { get; set; }
    }
}