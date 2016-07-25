using System.Runtime.Serialization;

namespace BikeAround.Service
{
    [DataContract]
    public enum BikeKind
    {
        [EnumMember]
        CityBicycle,
        [EnumMember]
        RoadBicycle,
        [EnumMember]
        MountainBike,
        [EnumMember]
        ElectricBicycle,
        [EnumMember]
        Scooter,
        [EnumMember]
        Other,
    }
}