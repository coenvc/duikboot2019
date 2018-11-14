namespace Duikboot.Web
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Duikboot.Web.Models;

    public partial class DuikbootContext : DbContext
    {
        public DuikbootContext(): base("name=DuikbootContext"){ 

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Capacity> Capacity { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.SurName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);
        }
    }
}
