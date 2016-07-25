using BikeAround.App.ViewModels;
using BikeAround.Service;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace BikeAround.App.WPF.Test
{
    internal sealed class SimpleValueTypePropertiesTestRun
    {
        private readonly Random _random;
        private readonly BikeViewModel _bike;

        public SimpleValueTypePropertiesTestRun(int seed)
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
                        TestSetHourlyRate();
                        break;

                    case 1:
                        TestSetKind();
                        break;

                    case 2:
                        TestSetLocationPostcode();
                        break;
                }
            }
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }

        private void TestSetHourlyRate()
        {
            decimal newHourlyRate;
            if (_random.Next(2) == 0)
            {
                // 50% chance to retain the old value
                newHourlyRate = _bike.HourlyRate;
            }
            else
            {
                newHourlyRate = _random.Next(20, 31);
            }
            _bike.HourlyRate = newHourlyRate;
        }

        private void TestSetKind()
        {
            BikeKind newKind;
            if (_random.Next(2) == 0)
            {
                // 50% chance to retain the old value
                newKind = _bike.Kind;
            }
            else
            {
                newKind = TestData.GetRandomBikeKind(_random);
            }
            _bike.Kind = newKind;
        }

        private void TestSetLocationPostcode()
        {
            int? newLocationPostcode;
            if (_random.Next(2) == 0)
            {
                // 50% chance to retain the old value
                newLocationPostcode = _bike.LocationPostcode;
            }
            else
            {
                newLocationPostcode = TestData.GetRandomLocation(_random).Postcode;
            }
            _bike.LocationPostcode = newLocationPostcode;
        }

        private void Bike_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}