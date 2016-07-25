using System;
using System.Collections.Generic;
using System.Linq;

namespace BikeAround.App.WPF.Test
{
    public static class Program
    {
        private const int RandomSeed = 826458263;
        private const int TestCount = 100000000;

        public static void Main(string[] args)
        {
            PerformSimpleValueTypePropertiesTestRuns(10, TestCount);
            PerformSimpleReferenceTypePropertiesTestRuns(10, TestCount);
            PerformComplexPropertiesTestRuns(10, TestCount);

            Console.Write($"All test runs finished. Press enter to close...");
            Console.ReadLine();
        }

        private static void PerformSimpleValueTypePropertiesTestRuns(int runCount, int operationCount)
        {
            var elapsedDurations = new List<TimeSpan>();
            for (int i = 0; i < runCount; i++)
            {
                Console.WriteLine($"Initiating simple value-typed properties test run #{i + 1} with {operationCount} operations...");

                var run = new SimpleValueTypePropertiesTestRun(RandomSeed);
                TimeSpan elapsedDuration = run.Perform(operationCount);
                elapsedDurations.Add(elapsedDuration);

                Console.WriteLine($"Test run finished in {elapsedDuration.TotalSeconds:F3} seconds.");
                Console.WriteLine();
            }

            Console.WriteLine($"Mean of elapsed durations over {runCount} runs: {DurationMean(elapsedDurations):F3} seconds.");
            Console.WriteLine($"Standard deviation of elapsed durations over {runCount} runs: {DurationStDev(elapsedDurations):F3} seconds.");
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void PerformSimpleReferenceTypePropertiesTestRuns(int runCount, int operationCount)
        {
            var elapsedDurations = new List<TimeSpan>();
            for (int i = 0; i < runCount; i++)
            {
                Console.WriteLine($"Initiating simple reference-typed properties test run #{i + 1} with {operationCount} operations...");

                var run = new SimpleReferenceTypePropertiesTestRun(RandomSeed);
                TimeSpan elapsedDuration = run.Perform(operationCount);
                elapsedDurations.Add(elapsedDuration);

                Console.WriteLine($"Test run finished in {elapsedDuration.TotalSeconds:F3} seconds.");
                Console.WriteLine();
            }

            Console.WriteLine($"Mean of elapsed durations over {runCount} runs: {DurationMean(elapsedDurations):F3} seconds.");
            Console.WriteLine($"Standard deviation of elapsed durations over {runCount} runs: {DurationStDev(elapsedDurations):F3} seconds.");
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void PerformComplexPropertiesTestRuns(int runCount, int operationCount)
        {
            var elapsedDurations = new List<TimeSpan>();
            for (int i = 0; i < runCount; i++)
            {
                Console.WriteLine($"Initiating complex reference-typed properties test run #{i + 1} with {operationCount} operations...");

                var run = new ComplexPropertiesTestRun(RandomSeed);
                TimeSpan elapsedDuration = run.Perform(operationCount);
                elapsedDurations.Add(elapsedDuration);

                Console.WriteLine($"Test run finished in {elapsedDuration.TotalSeconds:F3} seconds.");
                Console.WriteLine();
            }

            Console.WriteLine($"Mean of elapsed durations over {runCount} runs: {DurationMean(elapsedDurations):F3} seconds.");
            Console.WriteLine($"Standard deviation of elapsed durations over {runCount} runs: {DurationStDev(elapsedDurations):F3} seconds.");
            Console.WriteLine();
            Console.WriteLine();
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