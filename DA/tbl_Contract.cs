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
    
    public partial class tbl_Contract
    {
        public int ID { get; set; }
        public Nullable<int> order_ID { get; set; }
        public string payment_Text { get; set; }
        public Nullable<int> user_ID { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> report_ID { get; set; }
        public string sh_Date { get; set; }
    
        public virtual tbl_OrderHeader tbl_OrderHeader { get; set; }
        public virtual tbl_PrintDesigns tbl_PrintDesigns { get; set; }
        public virtual tbl_Users tbl_Users { get; set; }
    }
}
