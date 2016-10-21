using Nancy;
using Nancy.Responses;

namespace Auth0.Nancy.Owin.SelfHost
{
    public static class ModuleExtensions
    {
        public static void RequiresAuthentication2(this NancyModule module)
        {
            module.Before.AddItemToEndOfPipeline(ModuleExtensions.AuthenticateSession);
        }

        // This was based off of Auth0.NancyFx.SelfHost.  This should really
        // be forked and have a return URL account for.  For now I am 
        // Overriding the RequiresAuthentication() static method with my own impl.
        // This allows me to levearge the nuget package while adding the redirection
        // code.
        public static Response AuthenticateSession(NancyContext context)
        {
            var isAuthenticated = context.CurrentUser == null;
            var requestedPath = context.Request.Path;
            var loginUrl = string.Format("/?return={0}", requestedPath);
            return isAuthenticated ? new RedirectResponse(loginUrl) : null;
        }
    }
}
