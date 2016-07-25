using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeAround.Service.Impl.Data
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Index(IsUnique = true)]
        public Guid SecretIdentifier { get; set; }

        [Required, MaxLength(50), Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required, MaxLength(250)]
        public string FullName { get; set; }

        [Required, MaxLength(1000)]
        public string Address { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string PasswordHash { get; set; }

        [Required, MaxLength(100)]
        public string PasswordSalt { get; set; }

        public decimal Balance { get; set; }

        // Navigation properties

        public virtual ICollection<Bike> OwnedBikes { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }
    }
}