using System;
using Nancy;
using Auth0.Nancy.SelfHost;

namespace Auth0.Nancy.Owin.SelfHost.Modules
{
    public class Home : NancyModule
    {
        public Home()
        {
            Get["/"] = _ =>
            {
                return View["Index"];
            };

            Get["/callback"] = o =>
            {
                const string defaultView = "Dashboard";
                var code = Request.Query.code;
                string state = Request.Query.state.Value ?? defaultView;

                // We want to ensure the URL is local to your site and not some third party site.
                // If you are using this field to also store a nonce you'd want to parse that out
                // prior to extracting the URI.
                //
                // Basically, we don't want a user tricked into authenticating and being forwarded
                // to a 3rd party site.
                Uri destination;
                var isRelativeUrl = Uri.TryCreate(state, UriKind.Relative, out destination);
                state = isRelativeUrl ? state : defaultView;

                var x = this.AuthenticateThisSession();
                return x.ThenRedirectTo(state);
            };

            Get["/Logout"] = o =>
            {
                if (Context.CurrentUser == null) return Response.AsRedirect("/");
                var x = this.RemoveAuthenticationFromThisSession();
                return x.ThenRedirectTo("/");
            };
        }
    }
}
