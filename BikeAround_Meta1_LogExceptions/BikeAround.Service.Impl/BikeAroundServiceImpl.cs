using BikeAround.Service.Impl.Data;
using BikeAround.Service.Impl.Meta;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace BikeAround.Service.Impl
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    %LogExceptions(Logger.Instance)
    public sealed class BikeAroundServiceImpl : IBikeAroundService
    {
        private BikeAroundContext _context;
        private IAuthenticationContext _authenticationContext;

        public BikeAroundServiceImpl()
            : this(new ServiceAuthenticationContext())
        {
        }

        public BikeAroundServiceImpl(IAuthenticationContext authenticationContext)
        {
            _authenticationContext = authenticationContext;
        }

        #region IBikeAroundService Members

        [WebGet(UriTemplate = "/users/current", ResponseFormat = WebMessageFormat.Json)]
        public User GetCurrentUser()
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(GetCurrentUser)}()";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            string userName = EnsureAuthenticatedUser();
            Data.User user = Context.Users.Single(u => u.UserName == userName);
            User result = GetUserFull(user);

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription} with result {result}");
            return result;
        }

        [WebGet(UriTemplate = "/users?id={userID}", ResponseFormat = WebMessageFormat.Json)]
        public User GetUser(int userID)
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(GetUser)}({userID})";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            string userName = EnsureAuthenticatedUser();
            Data.User user = Context.Users.Single(u => u.ID == userID);
            User result = user.UserName == userName
                            ? GetUserFull(user)
                            : GetUserPublic(user);

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription} with result {result}");
            return result;
        }

        [WebGet(UriTemplate = "/bikes?id={bikeID}", ResponseFormat = WebMessageFormat.Json)]
        public Bike GetBike(int bikeID)
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(GetBike)}({bikeID})";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            EnsureAuthenticatedUser();
            Data.Bike bike = Context.Bikes.Single(v => v.ID == bikeID);
            Bike result = GetBike(bike);

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription} with result {result}");
            return result;
        }

        [WebGet(UriTemplate = "/bikes/available?postcode={postcode}", ResponseFormat = WebMessageFormat.Json)]
        public Bike[] FindAvailableBikes(int postcode)
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(FindAvailableBikes)}({postcode})";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            EnsureAuthenticatedUser();
            Bike[] result = Context.Bikes
                .Where(v => v.LocationPostcode == postcode && v.Trips.All(t => t.TripEnd.HasValue))
                .OrderBy(v => v.LocationAddress)
                .ToList()
                .Select(v => GetBike(v))
                .ToArray();

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription} with result {result}");
            return result;
        }

        [WebGet(UriTemplate = "/bikes/currentuser", ResponseFormat = WebMessageFormat.Json)]
        public Bike[] GetCurrentUserBikes()
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(GetCurrentUserBikes)}()";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            string userName = EnsureAuthenticatedUser();
            Data.User user = Context.Users.Single(u => u.UserName == userName);
            Bike[] result = user.OwnedBikes.Select(v => GetBike(v)).ToArray();

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription} with result {result}");
            return result;
        }

        [WebGet(UriTemplate = "/trips/currentuser", ResponseFormat = WebMessageFormat.Json)]
        public Trip[] GetCurrentUserTrips()
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(GetCurrentUserTrips)}()";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            string userName = EnsureAuthenticatedUser();
            Data.User user = Context.Users.Single(u => u.UserName == userName);
            Trip[] result = user.Trips
                .Where(t => t.TripEnd.HasValue)
                .OrderByDescending(t => t.TripStart)
                .ToList()
                .Select(t => GetTrip(t))
                .ToArray();

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription} with result {result}");
            return result;
        }

        [WebGet(UriTemplate = "/trips/bike?id={bikeID}", ResponseFormat = WebMessageFormat.Json)]
        public Trip[] GetBikeTrips(int bikeID)
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(GetBikeTrips)}({bikeID})";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            string userName = EnsureAuthenticatedUser();
            Data.Bike bike = Context.Bikes.Single(v => v.ID == bikeID);
            if (bike.OwnerUser.UserName != userName)
            {
                throw new FaultException("This bike is not owned by you.");
            }
            Trip[] result = bike.Trips
                .Where(t => t.TripEnd.HasValue)
                .OrderByDescending(t => t.TripStart)
                .ToList()
                .Select(t => GetTrip(t))
                .ToArray();

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription} with result {result}");
            return result;
        }

        [WebInvoke(
            Method = "POST",
            UriTemplate = "/users/create",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        public User RegisterUser(User userDetails, string password)
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(RegisterUser)}({userDetails}, <password>)";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            if (userDetails == null)
            {
                throw new ArgumentNullException(nameof(userDetails));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            else if (password.Length < 5)
            {
                throw new ArgumentException("Password must be at least 5 characters long.", nameof(password));
            }

            Data.User user = null;
            bool savedSuccessfully;
            do
            {
                // Repeatedly execute the business logic until the changes can be safely saved to the database
                try
                {
                    if (Context.Users.Any(u => u.UserName == userDetails.UserName))
                    {
                        throw new FaultException("The chosen username is already in use.");
                    }

                    string passwordSalt;
                    string passwordHash;
                    AuthenticationHelper.HashPassword(password, out passwordSalt, out passwordHash);

                    user = Context.Users.Create();
                    user.SecretIdentifier = Guid.NewGuid();
                    user.UserName = userDetails.UserName;
                    user.FullName = userDetails.FullName;
                    user.Address = userDetails.Address;
                    user.Email = userDetails.Email;
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Balance = 0m;
                    user = Context.Users.Add(user);

                    try
                    {
                        Context.SaveChanges();
                    }
                    catch (DbUpdateException ex) when (!(ex is DbUpdateConcurrencyException))
                    {
                        throw new FaultException("Some of the provided user data is not well-formed.");
                    }
                    savedSuccessfully = true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    InvalidateContext();
                    savedSuccessfully = false;
                }
            }
            while (!savedSuccessfully);

            User result = GetUserFull(user);

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription} with result {result}");
            return result;
        }

        [WebInvoke(
            Method = "POST",
            UriTemplate = "/bikes/create",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        public Bike RegisterBike(Bike bikeDetails, Guid bikeSecretIdentifier)
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(RegisterBike)}({bikeDetails}, {bikeSecretIdentifier})";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            if (bikeDetails == null)
            {
                throw new ArgumentNullException(nameof(bikeDetails));
            }

            string userName = EnsureAuthenticatedUser();

            Data.Bike bike = null;
            bool savedSuccessfully;
            do
            {
                // Repeatedly execute the business logic until the changes can be safely saved to the database
                try
                {
                    Data.User user = Context.Users.Single(u => u.UserName == userName);
                    if (bikeDetails.OwnerUserID != 0 && bikeDetails.OwnerUserID != user.ID)
                    {
                        throw new FaultException("Cannot register bikes owned by someone else than the current user.");
                    }

                    if (Context.Bikes.Any(v => v.SecretIdentifier == bikeSecretIdentifier))
                    {
                        throw new FaultException("This bike secret identifier is already in use.");
                    }

                    bike = Context.Bikes.Create();
                    bike.SecretIdentifier = bikeSecretIdentifier;
                    bike.OwnerUserID = user.ID;
                    bike.HourlyRate = decimal.Round(bikeDetails.HourlyRate, 2);
                    bike.Kind = bikeDetails.Kind;
                    bike.Make = bikeDetails.Make;
                    bike.Model = bikeDetails.Model;
                    bike.Color = bikeDetails.Color;
                    bike.Gears = bikeDetails.Gears;
                    bike.Weight = bikeDetails.Weight;
                    bike.FrontBrake = bikeDetails.FrontBrake;
                    bike.BackBrake = bikeDetails.BackBrake;
                    bike.Description = bikeDetails.Description;
                    bike.LocationPostcode = bikeDetails.LocationPostcode.Value;
                    bike.LocationAddress = bikeDetails.LocationAddress;
                    bike = Context.Bikes.Add(bike);

                    try
                    {
                        Context.SaveChanges();
                    }
                    catch (DbUpdateException ex) when (!(ex is DbUpdateConcurrencyException))
                    {
                        throw new FaultException("Some of the provided bike data is not well-formed.");
                    }
                    savedSuccessfully = true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    InvalidateContext();
                    savedSuccessfully = false;
                }
            }
            while (!savedSuccessfully);

            Bike result = GetBike(bike);

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription} with result {result}");
            return result;
        }

        [WebInvoke(Method = "POST", UriTemplate = "/trips/initiate", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json)]
        public void InitiateTrip(Guid userSecretIdentifier, Guid bikeSecretIdentifier)
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(InitiateTrip)}({userSecretIdentifier}, {bikeSecretIdentifier})";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            bool savedSuccessfully;
            do
            {
                // Repeatedly execute the business logic until the changes can be safely saved to the database
                try
                {
                    Data.User user = Context.Users.Single(u => u.SecretIdentifier == userSecretIdentifier);
                    Data.Bike bike = Context.Bikes.Single(v => v.SecretIdentifier == bikeSecretIdentifier);
                    if (!bike.IsAvailable)
                    {
                        throw new FaultException("Another trip with this bike is currently in progress.");
                    }

                    Data.Trip trip = Context.Trips.Create();
                    trip.User = user;
                    trip.Bike = bike;
                    trip.TripStart = DateTime.UtcNow;
                    trip = Context.Trips.Add(trip);

                    try
                    {
                        Context.SaveChanges();
                    }
                    catch (DbUpdateException ex) when (!(ex is DbUpdateConcurrencyException))
                    {
                        throw new FaultException("Saving trip failed.");
                    }
                    savedSuccessfully = true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    InvalidateContext();
                    savedSuccessfully = false;
                }
            }
            while (!savedSuccessfully);

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription}");
        }

        [WebInvoke(Method = "POST", UriTemplate = "/trips/terminate", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json)]
        public void TerminateTrip(Guid bikeSecretIdentifier, int locationPostcode, string locationAddress)
        {
            string methodDescription = $"{typeof(BikeAroundServiceImpl)}.{nameof(TerminateTrip)}({bikeSecretIdentifier}, {locationPostcode}, {locationAddress})";
            Logger.Instance.LogTraceEntry($"Entering {methodDescription}");

            bool savedSuccessfully;
            do
            {
                // Repeatedly execute the business logic until the changes can be safely saved to the database
                try
                {
                    Data.Bike bike = Context.Bikes.Single(v => v.SecretIdentifier == bikeSecretIdentifier);

                    Data.Trip trip = bike.Trips.SingleOrDefault(t => !t.TripEnd.HasValue);
                    if (trip == null)
                    {
                        throw new FaultException("No trip with this bike is currently in progress.");
                    }

                    DateTime tripEnd = DateTime.UtcNow;
                    decimal tripCost = decimal.Round((decimal)(tripEnd - trip.TripStart).TotalHours * bike.HourlyRate, 2);
                    decimal serviceFee = decimal.Round(tripCost * 0.1m, 2);

                    trip.TripEnd = tripEnd;
                    trip.TripCost = tripCost;
                    trip.ServiceFee = serviceFee;

                    bike.LocationPostcode = locationPostcode;
                    bike.LocationAddress = locationAddress;

                    trip.User.Balance -= tripCost;

                    bike.OwnerUser.Balance += tripCost - serviceFee;

                    try
                    {
                        Context.SaveChanges();
                    }
                    catch (DbUpdateException ex) when (!(ex is DbUpdateConcurrencyException))
                    {
                        throw new FaultException("Saving trip failed.");
                    }
                    savedSuccessfully = true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    InvalidateContext();
                    savedSuccessfully = false;
                }
            }
            while (!savedSuccessfully);

            Logger.Instance.LogTraceEntry($"Exiting {methodDescription}");
        }

        #endregion

        private BikeAroundContext Context
        {
            get
            {
                return _context ?? (_context = new BikeAroundContext());
            }
        }

        private static User GetUserFull(Data.User user)
        {
            return new User
            {
                UserID = user.ID,
                UserName = user.UserName,
                RentalOffers = user.OwnedBikes.Count,
                TripCount = user.Trips.Count(t => t.TripEnd.HasValue),
                Balance = user.Balance,
                SecretIdentifier = user.SecretIdentifier,
                FullName = user.FullName,
                Address = user.Address,
                Email = user.Email,
            };
        }

        private static User GetUserPublic(Data.User user)
        {
            return new User
            {
                UserID = user.ID,
                UserName = user.UserName,
                RentalOffers = user.OwnedBikes.Count,
            };
        }

        private static Bike GetBike(Data.Bike bike)
        {
            return new Bike
            {
                BikeID = bike.ID,
                OwnerUserID = bike.OwnerUserID,
                IsAvailable = bike.IsAvailable,
                HourlyRate = bike.HourlyRate,
                Kind = bike.Kind,
                Make = bike.Make,
                Model = bike.Model,
                Color = bike.Color,
                Gears = bike.Gears,
                Weight = bike.Weight,
                FrontBrake = bike.FrontBrake,
                BackBrake = bike.BackBrake,
                Description = bike.Description,
                LocationPostcode = bike.IsAvailable ? bike.LocationPostcode : (int?)null,
                LocationAddress = bike.IsAvailable ? bike.LocationAddress : null,
            };
        }

        private static Trip GetTrip(Data.Trip trip)
        {
            return new Trip
            {
                TripID = trip.ID,
                UserID = trip.UserID,
                BikeID = trip.BikeID,
                TripStart = trip.TripStart,
                TripEnd = trip.TripEnd.Value,
                TripCost = trip.TripCost.Value,
                ServiceFee = trip.ServiceFee.Value,
            };
        }

        private string EnsureAuthenticatedUser()
        {
            string userName = _authenticationContext.UserName;
            if (string.IsNullOrEmpty(userName))
            {
                throw new FaultException("This method requires authentication.");
            }
            return userName;
        }

        private void InvalidateContext()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }
    }
}