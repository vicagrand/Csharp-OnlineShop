using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static OnlineShop.Controllers.PaymentPController;

namespace OnlineShop.Models
{
	public class CardDetails
	{
		[Required(ErrorMessage = "CvC code requierd")]
		public string CvCCode { get; set; }
		public string cardType { get; set; }
	    [Required(ErrorMessage = "Card number requierd")]
		[StringLength(16, ErrorMessage = "There is missing numbers")]
		public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiration date required")]

        public System.DateTime ExpirationDate { get; set; }
   
    }




}
