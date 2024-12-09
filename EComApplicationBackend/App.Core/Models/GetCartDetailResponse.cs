using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class GetCartDetailResponse
    {
        public int productid { get; set; }
        public string productimg {  get; set; }
        public string productname { get; set; }
        public int quantity { get; set; }
        public float sellingprice { get; set; }
        public int cartid { get; set; }
    }
}
