using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Models
{
	public class CategoryDetail
	{
		public int CategoryId { get; set; }
		[Required(ErrorMessage ="Category name Requierd")]
		[StringLength(100,ErrorMessage ="Minimum 3 and maximum 100 characters are allowed",MinimumLength =3)]
		public string CategoryName { get; set; }
		public Nullable<bool> IsActive { get; set; }
		public Nullable<bool> IsDelete { get; set; }
	}

	public class ProductDetail
	{
		public int ProcuctId { get; set; }
		[Required(ErrorMessage = "Product name Requierd")]
		[StringLength(100, ErrorMessage = "Minimum 3 and maximum 100 characters are allowed", MinimumLength = 3)]
		public string ProductName { get; set; }
		[Required]
		[Range(1,50)]
		public Nullable<int> CategoryId { get; set; }
		public Nullable<bool> IsActive { get; set; }
		public Nullable<bool> IsDelete { get; set; }
		public Nullable<System.DateTime> CreateDate { get; set; }
		public Nullable<System.DateTime> ModifiedDate { get; set; }
		[Required(ErrorMessage = "Description is Requierd")]
		public Nullable<System.DateTime> Description { get; set; }
		public string ProductImg { get; set; }
		public Nullable<bool> IsFeatured { get; set; }
		[Required]
		[Range(typeof(int),"1","500",ErrorMessage = "Invalid Quantity")]
		public Nullable<int> Qnantity { get; set; }
		[Required]
		[Range(typeof(decimal), "1", "200000", ErrorMessage = "Invalid Price")]
		public Nullable<decimal> Price { get; set; }
		public SelectList Categories { get; set; }

	}
}