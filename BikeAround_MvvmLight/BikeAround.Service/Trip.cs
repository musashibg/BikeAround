using System;
using System.Runtime.Serialization;

namespace BikeAround.Service
{
    [DataContract]
    public class Trip
    {
        [DataMember(IsRequired = true)]
        public int TripID { get; set; }

        [DataMember(IsRequired = true)]
        public int UserID { get; set; }

        [DataMember(IsRequired = true)]
        public int BikeID { get; set; }

        [DataMember(IsRequired = true)]
        public DateTime TripStart { get; set; }

        [DataMember(IsRequired = true)]
        public DateTime TripEnd { get; set; }

        [DataMember(IsRequired = true)]
        public decimal TripCost { get; set; }

        [DataMember(IsRequired = true)]
        public decimal ServiceFee { get; set; }

        public override string ToString()
        {
            return $"BikeAround.Service.Trip [TripID = {TripID}]";
        }
    }
}