﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Duikboot.Web.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public User()
        {

        }
    }
}