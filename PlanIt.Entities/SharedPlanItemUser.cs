//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlanIt.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class SharedPlanItemUser
    {
        public int Id { get; set; }
        public int PlanItemId { get; set; }
        public System.DateTime SharingDateTime { get; set; }
        public int SharingStatusId { get; set; }
        public int UserOwnerId { get; set; }
        public int UserReceiverId { get; set; }
    
        public virtual PlanItem PlanItem { get; set; }
        public virtual SharingStatus SharingStatus { get; set; }
        public virtual User UserOwner { get; set; }
        public virtual User UserReceiver { get; set; }
    }
}
