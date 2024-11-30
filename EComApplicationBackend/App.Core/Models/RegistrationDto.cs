using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class RegistrationDto
    {
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? email { get; set; }
        public int roleid { get; set; }
        public DateOnly dob { get; set; }
        public string? mobilenumber { get; set; }
        public string? profileimage { get; set; }
        public string? address { get; set; }
        public string? zipcode { get; set; }
        public int countryid { get; set; }
        public int stateid { get; set; }

    }
}
