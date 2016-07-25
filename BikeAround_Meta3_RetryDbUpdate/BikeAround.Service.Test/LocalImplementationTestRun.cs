using BikeAround.Service.Impl;
using System;
using System.Diagnostics;

namespace BikeAround.Service.Test
{
    internal sealed class LocalImplementationTestRun : IAuthenticationContext
    {
        private readonly Random _random;
        private readonly BikeAroundServiceImpl _serviceImplementation;

        private string _currentUserName;

        public LocalImplementationTestRun(int seed)
        {
            _random = new Random(seed);
            _serviceImplementation = new BikeAroundServiceImpl(this);
        }

        #region IAuthenticationContext Members

        public string UserName
        {
            get
            {
                return _currentUserName;
            }
        }

        #endregion

        public TimeSpan Perform(int count)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < count; i++)
            {
                try
                {
                    switch (_random.Next(11))
                    {
                        case 0:
                            TestGetCurrentUser();
                            break;
                        case 1:
                            TestGetUser();
                            break;
                        case 2:
                            TestGetBike();
                            break;
                        case 3:
                            TestFindAvailableBikes();
                            break;
                        case 4:
                            TestGetCurrentUserBikes();
                            break;
                        case 5:
                            TestGetCurrentUserTrips();
                            break;
                        case 6:
                            TestGetBikeTrips();
                            break;
                        case 7:
                            TestRegisterUser();
                            break;
                        case 8:
                            TestRegisterBike();
                            break;
                        case 9:
                            TestInitiateTrip();
                            break;
                        case 10:
                            TestTerminateTrip();
                            break;
                    }
                }
                catch (Exception)
                {
                    // It is expected that some of the tests will get an exception as a response. We silence them
                }
            }
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }

        private void TestGetCurrentUser()
        {
            string userName = TestData.GetRandomUserName(_random);
            SetCurrentUser(userName);
            _serviceImplementation.GetCurrentUser();
        }

        private void TestGetUser()
        {
            string userName = TestData.GetRandomUserName(_random);
            SetCurrentUser(userName);
            _serviceImplementation.GetUser(TestData.GetRandomID(_random));
        }

        private void TestGetBike()
        {
            string userName = TestData.GetRandomUserName(_random);
            SetCurrentUser(userName);
            _serviceImplementation.GetBike(TestData.GetRandomID(_random));
        }

        private void TestFindAvailableBikes()
        {
            string userName = TestData.GetRandomUserName(_random);
            SetCurrentUser(userName);
            Location location = TestData.GetRandomLocation(_random);
            _serviceImplementation.FindAvailableBikes(location.Postcode);
        }

        private void TestGetCurrentUserBikes()
        {
            string userName = TestData.GetRandomUserName(_random);
            SetCurrentUser(userName);
            _serviceImplementation.GetCurrentUserBikes();
        }

        private void TestGetCurrentUserTrips()
        {
            string userName = TestData.GetRandomUserName(_random);
            SetCurrentUser(userName);
            _serviceImplementation.GetCurrentUserTrips();
        }

        private void TestGetBikeTrips()
        {
            string userName = TestData.GetRandomUserName(_random);
            SetCurrentUser(userName);
            Bike[] bikes = _serviceImplementation.GetCurrentUserBikes();
            _serviceImplementation.GetBikeTrips(TestData.GetRandomID(_random));
        }

        private void TestRegisterUser()
        {
            string userName = TestData.GetRandomUserName(_random);
            Location location = TestData.GetRandomLocation(_random);
            var user = new User
            {
                UserName = userName,
                FullName = userName,
                Address = location.Address,
                Email = $"{userName}@example.com",
            };
            SetCurrentUser(string.Empty);
            _serviceImplementation.RegisterUser(user, TestData.Password);
        }

        private void TestRegisterBike()
        {
            string userName = TestData.GetRandomUserName(_random);
            Guid bikeSecretIdentifier = TestData.GetRandomBikeSecretIdentifier(_random);
            Location location = TestData.GetRandomLocation(_random);
            var bike = new Bike
            {
                HourlyRate = _random.Next(20, 31),
                Make = "Kildemoes",
                Model = "City",
                Color = "Black",
                Gears = 7,
                Weight = 12.0f,
                FrontBrake = BrakeKind.Rim,
                BackBrake = BrakeKind.BackPedal,
                LocationPostcode = location.Postcode,
                LocationAddress = location.Address,
            };
            SetCurrentUser(userName);
            _serviceImplementation.RegisterBike(bike, bikeSecretIdentifier);
        }

        private void TestInitiateTrip()
        {
            string userName = TestData.GetRandomUserName(_random);
            SetCurrentUser(userName);
            User user = _serviceImplementation.GetCurrentUser();
            Guid userSecretIdentifier = user.SecretIdentifier;
            Guid bikeSecretIdentifier = TestData.GetRandomBikeSecretIdentifier(_random);
            SetCurrentUser(string.Empty);
            _serviceImplementation.InitiateTrip(userSecretIdentifier, bikeSecretIdentifier);
        }

        private void TestTerminateTrip()
        {
            Guid bikeSecretIdentifier = TestData.GetRandomBikeSecretIdentifier(_random);
            Location location = TestData.GetRandomLocation(_random);
            SetCurrentUser(string.Empty);
            _serviceImplementation.TerminateTrip(bikeSecretIdentifier, location.Postcode, location.Address);
        }

        private void SetCurrentUser(string userName)
        {
            _currentUserName = userName;
        }
    }
}