using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiitProjectProgessSystemApi.Models.CustomModels
{
    public class MessageModel
    {
        public int id { get; set; }
        public string description { get; set; }
        public string file_path { get; set; }
        public string sender { get; set; }
        public int sender_id { get; set; }
        public string receiver { get; set; }
        public int receiver_id { get; set; }
        public string time { get; set; }

    }
}