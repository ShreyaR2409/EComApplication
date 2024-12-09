using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SalesDetail
    {
        public int id {  get; set; }
        [ForeignKey("SalesMaster")]
        public int invoiceId { get; set; }
        public SalesMaster salesMaster { get; set; }
        public int productCode { get; set; }
        public int quantity {  get; set; }
        public float sellingPrice { get; set; }
    }
}
