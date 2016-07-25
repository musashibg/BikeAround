using BikeAround.Service;
using System;
using System.Text;

namespace BikeAround.Bike.Emulator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BikeAround on-bike locking and tracking device emulator.");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Please choose an action:");
                Console.WriteLine("  [1] Initiate bike trip");
                Console.WriteLine("  [2] Terminate bike trip");
                Console.WriteLine("  [3] Exit");
                string input = Console.ReadLine();

                int choice;
                if (int.TryParse(input, out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            InitiateTrip();
                            break;

                        case 2:
                            TerminateTrip();
                            break;

                        case 3:
                            Console.WriteLine("Terminating the program.");
                            return;
                    }
                }

                Console.WriteLine();
            }
        }

        private static void InitiateTrip()
        {
            Console.Write("Username: ");
            string userName = Console.ReadLine();

            Console.Write("Password: ");
            string password = InputPassword();

            Guid userSecretIdentifier;
            try
            {
                var authenticatedClient = new BikeAroundServiceClient(userName, password);
                User user = authenticatedClient.GetCurrentUser();
                userSecretIdentifier = user.SecretIdentifier;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Obtaining user secret identifier failed: {0}", ex);
                return;
            }

            Console.Write("Bike secret identifier: ");
            string input = Console.ReadLine();
            Guid bikeSecretIdentifier;
            if (!Guid.TryParse(input, out bikeSecretIdentifier))
            {
                Console.WriteLine("Invalid bike secret identifier format.");
                return;
            }

            try
            {
                var client = new BikeAroundServiceClient();
                client.InitiateTrip(userSecretIdentifier, bikeSecretIdentifier);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to initiate trip: {0}", ex);
                return;
            }

            Console.WriteLine("Trip initiated successfully.");
        }

        private static void TerminateTrip()
        {
            Console.Write("Bike secret identifier: ");
            string input = Console.ReadLine();
            Guid bikeSecretIdentifier;
            if (!Guid.TryParse(input, out bikeSecretIdentifier))
            {
                Console.WriteLine("Invalid bike secret identifier format.");
                return;
            }

            Console.Write("Current bike location - postcode: ");
            input = Console.ReadLine();
            int locationPostcode;
            if (!int.TryParse(input, out locationPostcode))
            {
                Console.WriteLine("Invalid postcode.");
                return;
            }

            Console.Write("Current bike location - address: ");
            string locationAddress = Console.ReadLine();

            try
            {
                var client = new BikeAroundServiceClient();
                client.TerminateTrip(bikeSecretIdentifier, locationPostcode, locationAddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to terminate trip: {0}", ex);
                return;
            }

            Console.WriteLine("Trip terminated successfully.");
        }

        private static string InputPassword()
        {
            var sb = new StringBuilder();

            ConsoleKeyInfo key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key != ConsoleKey.Backspace)
                {
                    sb.Append(key.KeyChar);
                    Console.Write("*");
                }
                else
                {
                    sb.Remove(sb.Length - 1, 1);
                    Console.Write("\b \b");
                }
                key = Console.ReadKey(true);
            }
            Console.WriteLine();
            return sb.ToString();
        }
    }
}