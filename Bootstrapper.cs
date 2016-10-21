using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Auth0.Nancy.SelfHost;

namespace Auth0.Nancy.Owin.SelfHost
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            Auth0Authentication.Enable(pipelines, new AuthenticationConfig
            {
                RedirectOnLoginFailed = "login",
                CookieName = "_auth0_userid",
                UserIdentifier = "userid"
            });
        }
    }
}
