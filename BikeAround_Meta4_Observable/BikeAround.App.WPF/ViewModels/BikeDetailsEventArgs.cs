using System;

namespace BikeAround.App.ViewModels
{
    public sealed class BikeDetailsEventArgs : EventArgs
    {
        public int BikeID { get; }

        public BikeDetailsEventArgs(int bikeID)
        {
            BikeID = bikeID;
        }
    }
}