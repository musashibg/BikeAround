using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

namespace BikeAround.Service.Impl
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.Add(new ServiceRoute(string.Empty, new WebServiceHostFactory(), typeof(BikeAroundServiceImpl)));
        }
    }
}