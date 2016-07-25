using System;
using System.ServiceModel;

namespace BikeAround.Service
{
    [ServiceContract]
    public interface IBikeAroundService
    {
        [OperationContract]
        User GetCurrentUser();

        [OperationContract]
        User GetUser(int userID);

        [OperationContract]
        Bike GetBike(int bikeID);

        [OperationContract]
        Bike[] FindAvailableBikes(int postcode);

        [OperationContract]
        Bike[] GetCurrentUserBikes();

        [OperationContract]
        Trip[] GetCurrentUserTrips();

        [OperationContract]
        Trip[] GetBikeTrips(int bikeID);

        [OperationContract]
        User RegisterUser(User userDetails, string password);

        [OperationContract]
        Bike RegisterBike(Bike bikeDetails, Guid bikeSecretIdentifier);

        [OperationContract]
        void InitiateTrip(Guid userSecretIdentifier, Guid bikeSecretIdentifier);

        [OperationContract]
        void TerminateTrip(Guid bikeSecretIdentifier, int locationPostcode, string locationAddress);
    }
}