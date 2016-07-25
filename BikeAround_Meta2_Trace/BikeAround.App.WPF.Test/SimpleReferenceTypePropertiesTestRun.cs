using BikeAround.App.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace BikeAround.App.WPF.Test
{
    internal sealed class SimpleReferenceTypePropertiesTestRun
    {
        private readonly Random _random;
        private readonly BikeViewModel _bike;

        public SimpleReferenceTypePropertiesTestRun(int seed)
        {
            _random = new Random(seed);
            _bike = new BikeViewModel();
            _bike.PropertyChanged += Bike_PropertyChanged;
        }

        public TimeSpan Perform(int count)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < count; i++)
            {
                switch (_random.Next(3))
                {
                    case 0:
                        TestSetMake();
                        break;

                    case 1:
                        TestSetModel();
                        break;

                    case 2:
                        TestSetLocationAddress();
                        break;
                }
            }
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }

        private void TestSetMake()
        {
            string newMake;
            if (_random.Next(2) == 0)
            {
                // 50% chance to retain the old value
                newMake = _bike.Make;
            }
            else
            {
                newMake = TestData.GetRandomBikeMake(_random);
            }
            _bike.Make = newMake;
        }

        private void TestSetModel()
        {
            string newModel;
            if (_random.Next(2) == 0)
            {
                // 50% chance to retain the old value
                newModel = _bike.Model;
            }
            else
            {
                newModel = TestData.GetRandomBikeMake(_random);
            }
            _bike.Model = newModel;
        }

        private void TestSetLocationAddress()
        {
            string newLocationAddress;
            if (_random.Next(2) == 0)
            {
                // 50% chance to retain the old value
                newLocationAddress = _bike.LocationAddress;
            }
            else
            {
                newLocationAddress = TestData.GetRandomLocation(_random).Address;
            }
            _bike.LocationAddress = newLocationAddress;
        }

        private void Bike_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}