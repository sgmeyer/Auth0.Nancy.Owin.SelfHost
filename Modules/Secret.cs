using Nancy;
using Nancy.Security;

namespace Auth0.Nancy.Owin.SelfHost.Modules
{
    public class Secret : NancyModule
    {
        public Secret()
        {
            this.RequiresAuthentication2();

            Get["/Dashboard"] = _ => View["Index"];
            Get["/Profile"] = _ => View["Profile"];
        }
    }
}
