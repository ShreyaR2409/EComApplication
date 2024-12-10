using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class ProductDto
    {
        public string productname { get; set; }
        public int productcode { get; set; }
        public IFormFile productimg { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public float sellingprice { get; set; }
        public float purchaseprice { get; set; }
        public DateTime purchasedate { get; set; }
        public int Stock { get; set; }
    }
}
