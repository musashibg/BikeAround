using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace BikeAround.Service.Test
{
    public static class Program
    {
        private const int RandomSeed = 75493731;
        private const int TestCount = 10000;

        public static void Main(string[] args)
        {
            PerformWebServiceTestRuns(10, TestCount);
            PerformLocalImplementationTestRuns(10, TestCount);

            Console.Write($"All test runs finished. Press enter to close...");
            Console.ReadLine();
        }

        private static void PerformWebServiceTestRuns(int runCount, int operationCount)
        {
            var elapsedDurations = new List<TimeSpan>();
            for (int i = 0; i < runCount; i++)
            {
                ResetDBAndLog();

                Console.WriteLine($"Initiating web service test run #{i + 1} with {operationCount} operations...");

                var run = new WebServiceTestRun(RandomSeed);
                TimeSpan elapsedDuration = run.Perform(operationCount);
                elapsedDurations.Add(elapsedDuration);
                TerminateActiveTrips();

                Console.WriteLine($"Test run finished in {elapsedDuration.TotalSeconds:F3} seconds.");
                Console.WriteLine();
            }

            Console.WriteLine($"Mean of elapsed durations over {runCount} runs: {DurationMean(elapsedDurations):F3} seconds.");
            Console.WriteLine($"Standard deviation of elapsed durations over {runCount} runs: {DurationStDev(elapsedDurations):F3} seconds.");
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void PerformLocalImplementationTestRuns(int runCount, int operationCount)
        {
            var elapsedDurations = new List<TimeSpan>();
            for (int i = 0; i < runCount; i++)
            {
                ResetDBAndLog();

                Console.WriteLine($"Initiating local implementation test run #{i + 1} with {operationCount} operations...");

                var run = new LocalImplementationTestRun(RandomSeed);
                TimeSpan elapsedDuration = run.Perform(operationCount);
                elapsedDurations.Add(elapsedDuration);
                TerminateActiveTrips();

                Console.WriteLine($"Test run finished in {elapsedDuration.TotalSeconds:F3} seconds.");
                Console.WriteLine();
            }

            Console.WriteLine($"Mean of elapsed durations over {runCount} runs: {DurationMean(elapsedDurations):F3} seconds.");
            Console.WriteLine($"Standard deviation of elapsed durations over {runCount} runs: {DurationStDev(elapsedDurations):F3} seconds.");
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void ResetDBAndLog()
        {
            string dbFilePath = ConfigurationManager.AppSettings["dbFilePath"];
            string initialDBFilePath = ConfigurationManager.AppSettings["initialDBFilePath"];
            string logFilePath = ConfigurationManager.AppSettings["logFilePath"];

            // Copy the initial database file over the current one
            if (!File.Exists(initialDBFilePath))
            {
                Console.WriteLine("Initial database file does not exist!");
                return;
            }
            File.Copy(initialDBFilePath, dbFilePath, true);

            // Delete the entire log file
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
        }

        private static void TerminateActiveTrips()
        {
            var random = new Random();
            foreach (Guid bikeSecretIdentifier in TestData.BikeSecretIdentifiers)
            {
                Location location = TestData.Locations[random.Next(TestData.Locations.Length)];
                var client = new BikeAroundServiceClient();
                try
                {
                    client.TerminateTrip(bikeSecretIdentifier, location.Postcode, location.Address);
                }
                catch
                {
                    // Silence exceptions for bikes not currently in a trip
                }
            }
        }

        private static double DurationMean(List<TimeSpan> durations)
        {
            return durations.Average(d => d.TotalSeconds);
        }

        private static double DurationStDev(List<TimeSpan> durations)
        {
            if (durations.Count < 2)
            {
                return 0.0;
            }

            double secondsMean = durations.Average(d => d.TotalSeconds);
            double variance = durations.Sum(d => (d.TotalSeconds - secondsMean) * (d.TotalSeconds - secondsMean)) / (durations.Count - 1);
            return Math.Sqrt(variance);
        }
    }
}