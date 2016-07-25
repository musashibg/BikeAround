using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BikeAround.Service
{
    public class BikeAroundServiceClient : IBikeAroundService
    {
        private readonly string _serviceEndpoint;
        private readonly string _userName;
        private readonly string _password;

        public BikeAroundServiceClient()
        {
            _serviceEndpoint = "http://localhost/BikeAround/";
        }

        public BikeAroundServiceClient(string userName, string password)
            : this()
        {
            _userName = userName;
            _password = password;
        }

        #region IBikeAroundService Members

        public User GetCurrentUser()
        {
            return RunSynchronously(GetCurrentUserAsync);
        }

        public User GetUser(int userID)
        {
            return RunSynchronously(() => GetUserAsync(userID));
        }

        public Bike GetBike(int bikeID)
        {
            return RunSynchronously(() => GetBikeAsync(bikeID));
        }

        public Bike[] FindAvailableBikes(int postcode)
        {
            return RunSynchronously(() => FindAvailableBikesAsync(postcode));
        }

        public Bike[] GetCurrentUserBikes()
        {
            return RunSynchronously(GetCurrentUserBikesAsync);
        }

        public Trip[] GetCurrentUserTrips()
        {
            return RunSynchronously(GetCurrentUserTripsAsync);
        }

        public Trip[] GetBikeTrips(int bikeID)
        {
            return RunSynchronously(() => GetBikeTripsAsync(bikeID));
        }

        public User RegisterUser(User userDetails, string password)
        {
            return RunSynchronously(() => RegisterUserAsync(userDetails, password));
        }

        public Bike RegisterBike(Bike bikeDetails, Guid bikeSecretIdentifier)
        {
            return RunSynchronously(() => RegisterBikeAsync(bikeDetails, bikeSecretIdentifier));
        }

        public void InitiateTrip(Guid userSecretIdentifier, Guid bikeSecretIdentifier)
        {
            RunSynchronously(() => InitiateTripAsync(userSecretIdentifier, bikeSecretIdentifier));
        }

        public void TerminateTrip(Guid bikeSecretIdentifier, int locationPostcode, string locationAddress)
        {
            RunSynchronously(() => TerminateTripAsync(bikeSecretIdentifier, locationPostcode, locationAddress));
        }

        #endregion

        #region Asynchronous service methods

        public async Task<User> GetCurrentUserAsync()
        {
            return await RestCall<User>(HttpMethod.Get, "users/current");
        }

        public async Task<User> GetUserAsync(int userID)
        {
            return await RestCall<User>(HttpMethod.Get, $"users?id={userID}");
        }

        public async Task<Bike> GetBikeAsync(int bikeID)
        {
            return await RestCall<Bike>(HttpMethod.Get, $"bikes?id={bikeID}");
        }

        public async Task<Bike[]> FindAvailableBikesAsync(int postcode)
        {
            return await RestCall<Bike[]>(HttpMethod.Get, $"bikes/available?postcode={postcode}");
        }

        public async Task<Bike[]> GetCurrentUserBikesAsync()
        {
            return await RestCall<Bike[]>(HttpMethod.Get, "bikes/currentuser");
        }

        public async Task<Trip[]> GetCurrentUserTripsAsync()
        {
            return await RestCall<Trip[]>(HttpMethod.Get, "trips/currentuser");
        }

        public async Task<Trip[]> GetBikeTripsAsync(int bikeID)
        {
            return await RestCall<Trip[]>(HttpMethod.Get, $"trips/bike?id={bikeID}");
        }

        public async Task<User> RegisterUserAsync(User userDetails, string password)
        {
            var bodyParam = new Dictionary<string, object>
            {
                { "userDetails", userDetails },
                { "password", password },
            };
            return await RestCall<User>(HttpMethod.Post, "users/create", bodyParam);
        }

        public async Task<Bike> RegisterBikeAsync(Bike bikeDetails, Guid bikeSecretIdentifier)
        {
            var bodyParam = new Dictionary<string, object>
            {
                { "bikeDetails", bikeDetails },
                { "bikeSecretIdentifier", bikeSecretIdentifier },
            };
            return await RestCall<Bike>(HttpMethod.Post, "bikes/create", bodyParam);
        }

        public async Task InitiateTripAsync(Guid userSecretIdentifier, Guid bikeSecretIdentifier)
        {
            var bodyParam = new Dictionary<string, object>
            {
                { "userSecretIdentifier", userSecretIdentifier },
                { "bikeSecretIdentifier", bikeSecretIdentifier },
            };
            await RestCall(HttpMethod.Post, "trips/initiate", bodyParam);
        }

        public async Task TerminateTripAsync(Guid bikeSecretIdentifier, int locationPostcode, string locationAddress)
        {
            var bodyParam = new Dictionary<string, object>
            {
                { "bikeSecretIdentifier", bikeSecretIdentifier },
                { "locationPostcode", locationPostcode },
                { "locationAddress", locationAddress },
            };
            await RestCall(HttpMethod.Post, "trips/terminate", bodyParam);
        }

        #endregion

        private static void RunSynchronously(Func<Task> asyncMethod)
        {
            Task task = Task.Run(asyncMethod);
            task.WaitAndUnwrapException();
        }

        private static T RunSynchronously<T>(Func<Task<T>> asyncMethod)
        {
            Task<T> task = Task.Run(asyncMethod);
            return task.WaitAndUnwrapException();
        }

        private static string SerializeBodyParam(IDictionary<string, object> bodyParam)
        {
            var settings = new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true,
                KnownTypes = new[] { typeof(User), typeof(Bike) },
            };
            var serializer = new DataContractJsonSerializer(typeof(IDictionary<string, object>), settings);
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, bodyParam);
                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private async Task RestCall(HttpMethod httpMethod, string methodName, IDictionary<string, object> bodyParam = null)
        {
            string serviceUri = _serviceEndpoint + methodName;
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, serviceUri);
            AddAuthorizationHeader(request);
            if (bodyParam != null)
            {
                string bodyString = SerializeBodyParam(bodyParam);
                request.Content = new StringContent(bodyString, Encoding.UTF8, "application/json");
            }

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        private async Task<T> RestCall<T>(HttpMethod httpMethod, string methodName, IDictionary<string, object> bodyParam = null)
        {
            string serviceUri = _serviceEndpoint + methodName;
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, serviceUri);
            AddAuthorizationHeader(request);
            if (bodyParam != null)
            {
                string bodyString = SerializeBodyParam(bodyParam);
                request.Content = new StringContent(bodyString, Encoding.UTF8, "application/json");
            }

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using (var resultStream = await response.Content.ReadAsStreamAsync())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(resultStream);
            }
        }

        private void AddAuthorizationHeader(HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(_userName))
            {
                byte[] authorizationBytes = Encoding.UTF8.GetBytes($"{_userName}:{_password}");
                string authorizationString = $"Basic {Convert.ToBase64String(authorizationBytes)}";
                request.Headers.Add("Authorization", authorizationString);
            }
        }
    }
}