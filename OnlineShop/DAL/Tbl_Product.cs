//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineShop.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Product
    {
        public int ProcuctId { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ProductImg { get; set; }
        public Nullable<bool> IsFeatured { get; set; }
        public Nullable<int> Qnantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Sale { get; set; }
    
        public virtual Tbl_Category Tbl_Category { get; set; }
    }
}
