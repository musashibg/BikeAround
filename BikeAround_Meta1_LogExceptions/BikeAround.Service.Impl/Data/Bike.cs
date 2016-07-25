using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BikeAround.Service.Impl.Data
{
    public class Bike
    {
        [Key]
        public int ID { get; set; }

        [Index(IsUnique = true)]
        public Guid SecretIdentifier { get; set; }

        public int OwnerUserID { get; set; }

        public decimal HourlyRate { get; set; }

        public BikeKind Kind { get; set; }

        [Required, MaxLength(50)]
        public string Make { get; set; }

        [Required, MaxLength(50)]
        public string Model { get; set; }

        [Required, MaxLength(50)]
        public string Color { get; set; }

        public int? Gears { get; set; }

        public float? Weight { get; set; }

        public BrakeKind? FrontBrake { get; set; }

        public BrakeKind? BackBrake { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public int LocationPostcode { get; set; }

        [Required, MaxLength(250)]
        public string LocationAddress { get; set; }

        // Navigation properties

        [ForeignKey(nameof(OwnerUserID))]
        public virtual User OwnerUser { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }

        // Helper properties

        public bool IsAvailable
        {
            get
            {
                return Trips.All(t => t.TripEnd.HasValue);
            }
        }
    }
}