using System.ServiceModel;

namespace BikeAround.Service.Impl
{
    public sealed class ServiceAuthenticationContext : IAuthenticationContext
    {
        #region IAuthenticationContext Members

        public string UserName
        {
            get
            {
                return ServiceSecurityContext.Current.PrimaryIdentity.Name;
            }
        }

        #endregion
    }
}