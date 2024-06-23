using OnlineShop.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models.Home
{
    public class Item
    {
        public Tbl_Product product { get; set; }
        public int Qnantity { get; set; }
        public int RemainingQuantity { get; internal set; }
    }

}