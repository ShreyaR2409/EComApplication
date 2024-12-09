using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SalesMaster
    {
        public int id { get; set; }
        public string invoiceId { get; set; }
        [ForeignKey("User")]
        public int userid { get; set; }
        public User user { get; set; }
        public DateTime invoiceDate { get; set; }
        public int total { get; set; }
        public string address { get; set; }
        public string zipcode { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }
}
