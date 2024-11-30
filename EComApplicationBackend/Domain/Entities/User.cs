using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int id { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? email { get; set; }

        [ForeignKey("Role")]
        public int roleid {  get; set; }
        public Role? role { get; set; }
        public DateOnly dob { get; set; }   
        public string? username {  get; set; }
        public string? password { get; set; }
        public string? mobilenumber { get; set; }
        public string? profileimage { get; set; }
        public string? address { get; set; }
        public string? zipcode { get; set; }

        [ForeignKey("Country")]
        public int countryid { get; set; }
        public Country? country { get; set; }

        [ForeignKey("State")]
        public int stateid { get; set; }
        public State? state { get; set; }

    }
}
