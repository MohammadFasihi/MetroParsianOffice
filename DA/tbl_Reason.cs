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
    
    public partial class tbl_Reason
    {
        public int ID { get; set; }
        public string res_Text { get; set; }
        public string res_Side { get; set; }
        public Nullable<int> ap_ID { get; set; }
    
        public virtual tbl_Apointments tbl_Apointments { get; set; }
    }
}
