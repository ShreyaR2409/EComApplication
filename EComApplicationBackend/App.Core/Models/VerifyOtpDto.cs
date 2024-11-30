using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class VerifyOtpDto
    {
        public int UserId { get; set; }
        public string Otp { get; set; }
    }
}
