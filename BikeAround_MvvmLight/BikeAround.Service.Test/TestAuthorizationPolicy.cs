/* Original code by David Tchepak, taken from http://www.davesquared.net/2008/04/faking-wcf-authentication-for.html */

using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Security.Principal;

namespace BikeAround.Service.Test
{

    internal sealed class TestAuthorizationPolicy : IAuthorizationPolicy
    {
        private readonly string _userName;

        public string Id
        {
            get { return "TestAuthPolicy"; }
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public TestAuthorizationPolicy(string userName)
        {
            _userName = userName;
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            var identity = new GenericIdentity(_userName);
            evaluationContext.Properties["Principal"] = new GenericPrincipal(identity, null);
            IList<IIdentity> identities = new List<IIdentity> { identity };
            evaluationContext.Properties.Add("Identities", identity);
            return true;
        }
    }
}