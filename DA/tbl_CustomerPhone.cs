//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DA
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_CustomerPhone
    {
        public int ID { get; set; }
        public Nullable<int> cu_ID { get; set; }
        public string cu_Field { get; set; }
        public string cu_Data { get; set; }
    
        public virtual tbl_Customer tbl_Customer { get; set; }
    }
}
