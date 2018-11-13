using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Duikboot.Web.Models;

namespace Duikboot.Web.ExtensionMethods
{
    public class Extension
    {
        public static User SetDays(User user)
        {
            user.Zaterdag = user.Zaterdag == null ? true : false;
            user.Zondag = user.Zondag == null ? true : false;
            user.Maandag = user.Maandag == null ? true : false;
            user.Dinsdag = user.Dinsdag == null ? true : false;
            return user;
        }
    }
}