using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Otp
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("User")]
        public int userid {  get; set; }
        public string? otp {  get; set; }

    }
}
