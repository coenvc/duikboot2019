using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Duikboot.Web.Models
{
    public class Event 
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SpotsAvailable { get; set; }
        public List<User> Users { get; set; }


    }
}