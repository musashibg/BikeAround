using BikeAround.Service.Impl.Data;
using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace BikeAround.Service.Impl
{
    public sealed class BikeAroundAuthenticationModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Context_AuthenticateRequest;
        }

        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext.Current.User = ProcessAuthentication();
        }

        #endregion

        private IPrincipal ProcessAuthentication()
        {
            //Extract the Authorization header, and parse out the credentials converting the Base64 string:
            string authHeader = HttpContext.Current.Request.Headers["Authorization"];
            if ((authHeader != null) && (authHeader != string.Empty))
            {
                string[] serviceCredentials = Encoding.UTF8
                    .GetString(Convert.FromBase64String(authHeader.Substring(6)))
                    .Split(':');
                string userName = serviceCredentials[0];
                string password = serviceCredentials[1];

                var context = new BikeAroundContext();
                Data.User user = context.Users.SingleOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    byte[] saltBytes = Convert.FromBase64String(user.PasswordSalt);
                    string passwordHash = AuthenticationHelper.ComputePasswordHash(password, saltBytes);

                    if (passwordHash == user.PasswordHash)
                    {
                        return new GenericPrincipal(new GenericIdentity(userName), null);
                    }
                }

                throw new HttpException(400, "Invalid logon credentials.");
            }
            return null;
        }
    }
}