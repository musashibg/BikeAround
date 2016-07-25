namespace BikeAround.App.WPF.Test
{
    public struct Location
    {
        public readonly int Postcode;
        public readonly string Address;

        public Location(int postcode, string address)
        {
            Postcode = postcode;
            Address = address;
        }
    }
}