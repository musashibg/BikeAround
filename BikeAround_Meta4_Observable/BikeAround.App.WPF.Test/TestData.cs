using BikeAround.Service;
using System;

namespace BikeAround.App.WPF.Test
{
    public static class TestData
    {
        public static BikeKind[] BikeKinds = (BikeKind[])Enum.GetValues(typeof(BikeKind));

        public static string[] BikeMakes = new[]
        {
            "Kildemoes",
            "Raleigh",
            "Centurion",
            "Drag",
            "Mustang",
            "Everton",
            "Trek",
            "Scott",
            "Spectra",
            "Avenue",
        };

        public static string[] BikeModels = new[]
        {
            "Classic",
            "Tourist",
            "Classic Deluxe",
            "Whistler Pro",
            "Aspect",
            "Contessa Spark",
            "Evoke",
            "Scale",
            "Genius",
            "Gambler",
        };

        public static readonly Location[] Locations = new[]
        {
            new Location(2300, "Startfordvej 1"),
            new Location(2300, "Greisvej 18"),
            new Location(2770, "Soldugvej 31"),
            new Location(2300, "Vogtervej 27"),
            new Location(2300, "Reberbanegade 51"),
            new Location(1440, "Sydområdet 4"),
            new Location(1401, "Strandgade 1"),
            new Location(1208, "Kompagnistræde 29"),
            new Location(1165, "Nørregade 12"),
            new Location(1302, "Dronningens Tværgade 43"),
            new Location(2100, "Øster Farimagsgade 65"),
            new Location(2200, "Guldbergsgade 24"),
            new Location(2100, "Vibekegade 29"),
            new Location(2100, "Fanøgade 27"),
            new Location(2100, "Lersø Parkallé 111"),
            new Location(2700, "Skolevangen 4"),
            new Location(2700, "Brønshøjgårdvej 4"),
            new Location(2720, "Slotsherrensvej 41"),
            new Location(2500, "Sæbyholmsvej 53"),
            new Location(2500, "Højbovej 3B"),
        };

        public static readonly string[] UserNames = new[]
        {
            "",
            "john",
            "daniel",
            "solesen",
            "peter",
            "clarac",
            "mary",
            "sven",
            "jespersen",
            "alice",
        };

        public static readonly string[] Passwords = new[]
        {
            "",
            "12345",
            "1234567890",
            "verysecret",
            "LoveYou",
            "asdfgh",
            "coconut",
            "j#0b<Ldq^",
            "dogslife",
            "Courtney",
        };

        public static BikeKind GetRandomBikeKind(Random random)
        {
            return BikeKinds[random.Next(BikeKinds.Length)];
        }

        public static string GetRandomBikeMake(Random random)
        {
            return BikeMakes[random.Next(BikeMakes.Length)];
        }

        public static string GetRandomBikeModel(Random random)
        {
            return BikeModels[random.Next(BikeModels.Length)];
        }

        public static Location GetRandomLocation(Random random)
        {
            return Locations[random.Next(Locations.Length)];
        }

        public static string GetRandomUserName(Random random)
        {
            return UserNames[random.Next(UserNames.Length)];
        }

        public static string GetRandomPassword(Random random)
        {
            return Passwords[random.Next(Passwords.Length)];
        }
    }
}