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

    public partial class Projects
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Projects()
        {
            this.Request = new HashSet<Request>();
            this.User_Projects = new HashSet<User_Projects>();
        }
        [Display(Name = "Project")]
        public int projectID { get; set; }
        [Display(Name ="Project name")]
        [Required]
        public string name { get; set; }
        public string state { get; set; }
        [Display(Name = "Project Description")]
        [Required]
        public string jobDescr { get; set; }
        public Nullable<bool> assigned { get; set; }
        public Nullable<bool> IsDelieverd { get; set; }
        
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
        public Nullable<int> Price { get; set; }
        public string Comments { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Request { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_Projects> User_Projects { get; set; }
    }
}