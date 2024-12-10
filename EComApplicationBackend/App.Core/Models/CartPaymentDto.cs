using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class CartPaymentDto
    {
        public int UserId { get; set; }
        public string CardNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int Cvv { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string ZipCode { get; set; }
    }
}
