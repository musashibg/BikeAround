using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeAround.Service.Impl.Data
{
    public class Trip
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int BikeID { get; set; }

        public DateTime TripStart { get; set; }

        public DateTime? TripEnd { get; set; }

        public decimal? TripCost { get; set; }

        public decimal? ServiceFee { get; set; }

        // Navigation properties

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(BikeID))]
        public virtual Bike Bike { get; set; }
    }
}