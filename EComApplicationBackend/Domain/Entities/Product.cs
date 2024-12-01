using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int id { get; set; }
        public string productname { get; set; }
        public string productcode { get; set; }
        public string productimg { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public float sellingprice { get; set; }
        public float purchaseprice { get; set; }
        public DateTime purchasedate { get; set; }
        public int Stock { get; set; }
        public bool isDeleted { get; set; }= false;

    }
}
