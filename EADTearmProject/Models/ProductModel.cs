using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EADTearmProject.Models
{
    public class ProductModel
    {
        public int p_id { get; set; }
        public string p_name { get; set; }
        public double p_price { get; set; }
        public string p_description { get; set; }
        public int cat_id { get; set; }
        public string p_picture { get; set; }

        public int p_type { get; set; }
    }
}