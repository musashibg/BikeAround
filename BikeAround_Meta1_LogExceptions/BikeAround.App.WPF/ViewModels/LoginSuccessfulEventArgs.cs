using BikeAround.Service;
using System;

namespace BikeAround.App.ViewModels
{
    public sealed class LoginSuccessfulEventArgs : EventArgs
    {
        public BikeAroundServiceClient AuthenticatedClient { get; set; }

        public LoginSuccessfulEventArgs(BikeAroundServiceClient authenticatedClient)
        {
            AuthenticatedClient = authenticatedClient;
        }
    }
}