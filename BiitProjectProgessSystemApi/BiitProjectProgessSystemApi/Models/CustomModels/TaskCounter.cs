using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class TaskCounter
    {
        public String name { get; set; }
        public String color { get; set; }
        public String legendFontColor { get; set; }        
        public int legendFontSize { get; set; }
        public int population { get; set; }
    }
}