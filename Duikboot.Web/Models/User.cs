using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Duikboot.Web.Models
{
    public class User
    {

        public int ID { get; set; }
        
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Zaterdag { get; set; }
        public bool? Zondag { get; set; }
        public bool? Maandag { get; set; }
        public bool? Dinsdag { get; set; }
        public decimal? Amount { get; set; }

        public User()
        {

        }
    }
}