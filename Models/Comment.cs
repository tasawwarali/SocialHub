//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialHub.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int CommtId { get; set; }
        public string CommentMsg { get; set; }
        public int PostId { get; set; }
        public string UserName { get; set; }
    
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
