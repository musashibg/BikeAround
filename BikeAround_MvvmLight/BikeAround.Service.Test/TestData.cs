using System;

namespace BikeAround.Service.Test
{
    public static class TestData
    {
        public const string Password = "12345";

        public static readonly string[] UserNames = new[]
        {
            "john",
            "daniel",
            "solesen",
            "peter",
            "clarac",
            "mary",
            "sven",
            "jespersen",
            "alice",
            "gregory",
            "stephen",
            "karl",
            "anna",
            "kaare",
            "james",
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

        public static readonly Guid[] BikeSecretIdentifiers = new[]
        {
            new Guid("040655CA-7838-4446-A365-073E0D0B10E9"),
            new Guid("F4208875-9C23-402E-A971-DFB38E76C9B3"),
            new Guid("31A9DFD3-A293-4D9B-9521-8B1AC66CD113"),
            new Guid("0099ADFF-2251-42A8-BB64-F39959136175"),
            new Guid("B19FCC19-5DE9-4342-ADC6-6A827AC92390"),
            new Guid("7C82C032-D06C-42DF-9C80-1470C0FB274B"),
            new Guid("F0280C25-B8D1-4C6C-8EA3-203B01ED51F9"),
            new Guid("9E50AE9B-9DB1-4F6C-90BA-DDF53E5B7713"),
            new Guid("A8DE3B01-D2F3-4FB3-9FC0-F40722623FCC"),
            new Guid("406798E4-E51F-44D8-8E72-C27E1E4940AC"),
            new Guid("682285C6-F14D-41FE-8B39-B928301E4D78"),
            new Guid("1279B87F-5B3E-4313-9549-539377A559B7"),
            new Guid("AA100F14-6A29-4A36-ADD0-FCA766B90954"),
            new Guid("40DE28DC-EA5A-437C-88B0-2FE19545B7FA"),
            new Guid("E331A538-7E41-44D8-8613-8DC0A29A59DB"),
        };

        public static int GetRandomID(Random random)
        {
            return random.Next(1, 21);
        }

        public static string GetRandomUserName(Random random)
        {
            return UserNames[random.Next(UserNames.Length)];
        }

        public static Location GetRandomLocation(Random random)
        {
            return Locations[random.Next(Locations.Length)];
        }

        public static Guid GetRandomBikeSecretIdentifier(Random random)
        {
            return BikeSecretIdentifiers[random.Next(BikeSecretIdentifiers.Length)];
        }
    }
}