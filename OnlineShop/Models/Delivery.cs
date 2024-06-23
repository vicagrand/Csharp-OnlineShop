using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
	public class Delivery
	{
		public string UserName { get; set; }
		public string OrderStatus { get; set; }
		public string OrderTracking { get; set; }
		public string OrderId { get; set; }
	}
}