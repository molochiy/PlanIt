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
    
    public partial class CommentNotification
    {
        public int Id { get; set; }
        public Nullable<int> CommentId { get; set; }
        public Nullable<int> ReceiverId { get; set; }
        public bool WasNotified { get; set; }
    
        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }
    }
}
