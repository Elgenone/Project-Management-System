//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectsManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class User_Projects
    {
        public int id { get; set; }
        public Nullable<int> project_id { get; set; }
        public Nullable<int> myUser_id { get; set; }
        [Display(Name ="Rating")]
   
        public Nullable<int> rating { get; set; }
        public virtual Projects Projects { get; set; }
        public virtual Users Users { get; set; }
    }
}
