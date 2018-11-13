using Duikboot.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Duikboot.Web.Entity
{
    public class DuikbootContext:DbContext
    { 
        DbSet<User> Users { get; set; }
    }
}