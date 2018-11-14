namespace Duikboot.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Duikboot.Web.DuikbootContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Duikboot.Web.DuikbootContext context)
        {
            //context.Capacity.AddOrUpdate(x => x.ID,
            //    new Models.Capacity()
            //    {
            //        Spots = 45,
            //        Name ="Zaterdag"
            //    },
            //    new Models.Capacity()
            //    {
            //        Spots = 45,
            //        Name = "Zondag"
            //    }, 
            //    new Models.Capacity()
            //    {
            //        Spots = 45,
            //        Name ="Maandag"
            //    }, 
            //    new Models.Capacity()
            //    {
            //        Spots = 45,
            //        Name = "Dinsdag"
            //    }
            //    );
        }
    }
}
