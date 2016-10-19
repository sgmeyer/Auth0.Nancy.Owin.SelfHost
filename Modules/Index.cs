using Nancy;
using Nancy.Security;

namespace Auth0.Nancy.Owin.SelfHost.Modules
{
    public class Index : NancyModule
    {
        public Index()
        {
            Get["/"] = _ => "Hello!";
        }
    }

    public class Secure : NancyModule
    {
        public Secure()
        {
#if !DEBUG
            this.RequiresHttps();
#endif
            this.RequiresMSOwinAuthentication();

            Get["/private"] = _ => {
                var user = this.Context.GetMSOwinUser();
                return "Hello, from secured.";
            };
        }
    }
}