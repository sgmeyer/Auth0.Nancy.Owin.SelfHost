using Nancy;
using Nancy.Security;

namespace Auth0.Nancy.Owin.SelfHost.Modules
{
    /**
     * I highly recommend keeping the API and Website as two different applications (and clients).
     * You will find this is more beneficial as we make our new Auth Pipeline changes in the roadmap.
     **/
    public class Api : NancyModule
    {
        public Api()
        {
#if !DEBUG
            this.RequiresHttps();
#endif
            Get["/api/"] = _ => "Hello!";
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

            Get["/api/private"] = _ => {
                var user = this.Context.GetMSOwinUser();
                return "Hello, from secured.";
            };
        }
    }
}