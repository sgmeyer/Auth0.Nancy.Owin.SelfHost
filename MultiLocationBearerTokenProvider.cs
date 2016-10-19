using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

namespace Auth0.Nancy.Owin.SelfHost
{
    public class MultiLocationBearerTokenProvider : OAuthBearerAuthenticationProvider
    {
        readonly string _cookieName;

        public MultiLocationBearerTokenProvider(string cookieName)
        {
            _cookieName = cookieName;
        }

        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            // This checks a cookie for JWT.  If it doesn't find it in the cookie
            // it will check the Authorization header, which is what the base class
            // is doing.
            var cookie = context.Request.Cookies[_cookieName];
            if (!string.IsNullOrEmpty(cookie))
            {
                context.Token = cookie;
            }
            else
            {
                base.RequestToken(context);
            }
            return Task.FromResult<object>(null);
        }
    }
}