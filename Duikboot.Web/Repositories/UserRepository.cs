using Duikboot.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Duikboot.Web.Repositories
{
    public class UserRepository
    {
        private DuikbootContext DbContext;

        public UserRepository()
        {
            this.DbContext = new DuikbootContext(); 
        }

        public void Add(User user)
        {
            DbContext.Users.Add(user);

            DbContext.SaveChanges();
        }
    }
}