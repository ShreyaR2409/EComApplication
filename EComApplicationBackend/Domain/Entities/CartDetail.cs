using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CartDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CartMaster")]
        public int cartMasterId {  get; set; }
        public CartMaster cartMaster { get; set; }

        [ForeignKey("Product")]
        public int productId { get; set; }
        public Product Product { get; set; }

        public int quantity { get; set; }
    }
}
