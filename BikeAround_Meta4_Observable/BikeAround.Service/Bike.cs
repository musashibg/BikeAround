using System.Runtime.Serialization;

namespace BikeAround.Service
{
    [DataContract]
    public class Bike
    {
        [DataMember(IsRequired = true)]
        public int BikeID { get; set; }

        [DataMember(IsRequired = true)]
        public int OwnerUserID { get; set; }

        [DataMember(IsRequired = true)]
        public bool IsAvailable { get; set; }

        [DataMember(IsRequired = true)]
        public decimal HourlyRate { get; set; }

        [DataMember(IsRequired = true)]
        public BikeKind Kind { get; set; }

        [DataMember(IsRequired = true)]
        public string Make { get; set; }

        [DataMember(IsRequired = true)]
        public string Model { get; set; }

        [DataMember(IsRequired = true)]
        public string Color { get; set; }

        [DataMember]
        public int? Gears { get; set; }

        [DataMember]
        public float? Weight { get; set; }

        [DataMember]
        public BrakeKind? FrontBrake { get; set; }

        [DataMember]
        public BrakeKind? BackBrake { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int? LocationPostcode { get; set; }

        [DataMember]
        public string LocationAddress { get; set; }

        public override string ToString()
        {
            return $"BikeAround.Service.Bike [BikeID = {BikeID}]";
        }
    }
}