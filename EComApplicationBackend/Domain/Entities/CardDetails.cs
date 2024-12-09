using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CardDetails
    {
        public int id { get; set; }
        public string? cardnumber { get; set; }
        public DateOnly expirydate { get; set; }
        public int cvv { get; set; }
    }
}
