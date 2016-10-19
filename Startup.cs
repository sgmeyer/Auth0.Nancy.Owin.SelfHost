using Owin;
using Microsoft.Owin.Security.Jwt;
using System.Text;
using System.IdentityModel.Tokens;

namespace Auth0.Nancy.Owin.SelfHost
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // In pipeline-1 we use the id_token for the JWT to call the API.  In pipeline-2 we will
            // use the new access_token for authorization to the API.  To cover forward and backward
            // compatibility we will allow the API to accept JWTs from both audiences.

            // The Client_ID is used as the audience for the id_token.
            var clientAudience = "Client Id used to authenticate the users";

            // With the pipeline-2 flow we'll use the identifier of the API as the audience.
            var apiAudience = "https://sgmeyer.nancy.owin.selfhost.com/";

            // This demo uses HS256 symmetric signing.  It is always better to use RS256 assymmetric
            // signing with a certificate.  Also, for this example I am not Base64 encoding the scret.
            // If you are using jwt.io's debugger make sure you uncheck that box or alter this line of
            // code.
            var keyAsBytes = Encoding.ASCII.GetBytes("signing secret");
            var options = new JwtBearerAuthenticationOptions()
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new InMemorySymmetricSecurityKey(keyAsBytes),
                    ValidIssuer = "https://sgmeyer.auth0.com/",

                    // This is critical.  You always want to require a signed token.  Never
                    // accept a token that is not signed.
                    RequireSignedTokens = true,

                    // These are other validation options you could explicitly set as true.  Most default
                    // to true.
                    //RequireExpirationTime = true,
                    //ValidateAudience = true
                    //ValidateIssuer = true
                    //ValidateIssuerSigningKey = true,
                    //ValidateActor = true
                    //ValidateLifetime = true

                    // Include all of the client_ids in allowed audiences for each client that can be issued a JWT.
                    ValidAudiences = new[] { apiAudience, clientAudience }
                },
                Provider = new MultiLocationBearerTokenProvider("some-cookie")
            };

            app.UseJwtBearerAuthentication(options);
            app.UseNancy();
        }
    }
}