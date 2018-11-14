namespace Duikboot.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Users")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        [NotMapped] 
        public Dictionary<string,int> Days { get; set; }
    }


}
