using System.Runtime.Serialization;

namespace BikeAround.Service
{
    [DataContract]
    public enum BrakeKind
    {
        [EnumMember]
        None,
        [EnumMember]
        Rim,
        [EnumMember]
        Disc,
        [EnumMember]
        Drum,
        [EnumMember]
        BackPedal,
    }
}