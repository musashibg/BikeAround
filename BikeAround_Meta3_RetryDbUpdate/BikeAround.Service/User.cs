using System;
using System.Runtime.Serialization;

namespace BikeAround.Service
{
    [DataContract]
    public class User
    {
        // Shown to everyone

        [DataMember(IsRequired = true)]
        public int UserID { get; set; }

        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        [DataMember(IsRequired = true)]
        public int RentalOffers { get; set; }

        // Shown to self

        [DataMember]
        public int TripCount { get; set; }

        [DataMember]
        public decimal Balance { get; set; }

        [DataMember]
        public Guid SecretIdentifier { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Email { get; set; }

        public override string ToString()
        {
            return $"BikeAround.Service.User [UserID = {UserID}]";
        }
    }
}