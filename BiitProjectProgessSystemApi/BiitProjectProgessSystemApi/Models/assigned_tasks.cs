//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BiitProjectProgessSystemApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class assigned_tasks
    {
        public int id { get; set; }
        public int task_id { get; set; }
        public int assigned_by { get; set; }
        public int assigned_to { get; set; }
        public Nullable<int> rating { get; set; }
        public Nullable<int> status_id { get; set; }
    
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
        public virtual status status { get; set; }
        public virtual task task { get; set; }
    }
}
