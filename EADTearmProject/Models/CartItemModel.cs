using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EADTearmProject.Models
{
    public class CartItemModel
    {
        public int cartItem_id { get; set; }
        public int quantity { get; set; }
        public double total_cost { get; set; }
        public int p_id { get; set; }
    }
}