//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MyBlog.Domain.Entities
{
    public partial class Comment
    {
    
        public virtual int Id { get; private set; }
    
        public virtual int PostId { get; private set; }
    
        public virtual string Body { get; private set; }
    
        public virtual bool Approved { get; private set; }
    
        public virtual System.DateTime CreationDate { get; private set; }
    
        public UserInfo UserInfo { get; private set;}
    
    
        public virtual Post Post{ get; private set; }
    }
}
