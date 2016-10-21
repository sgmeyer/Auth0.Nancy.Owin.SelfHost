using Owin;
using Microsoft.Owin.Security.Jwt;
using System.Configuration;
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
            var clientAudience = ConfigurationManager.AppSettings["auth0:ClientId"];

            // With the pipeline-2 flow we'll use the identifier of the API as the audience.
            var apiAudience = "https://sgmeyer.nancy.owin.selfhost.com/";

            // This demo uses HS256 symmetric signing.  It is always better to use RS256 assymmetric
            // signing with a certificat.
            var secret = ConfigurationManager.AppSettings["auth0:ClientSecret"];
            var keyAsBytes = Base64UrlDecode(secret);
            var options = new JwtBearerAuthenticationOptions()
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new InMemorySymmetricSecurityKey(keyAsBytes),
                    ValidIssuer = "https://sgmeyer.auth0.com/",

                    // This is critical.  You always want to require a signed token.  Never
                    // accept a token that is not signed.
                    RequireSignedTokens = true,
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


        // We are using id_token coming out of Auth0.  This is signed with the ClientSecret.
        // We need this helper method for verifying the JWT signature:
        //
        // https://auth0.com/docs/tutorials/generate-jwt-dotnet
        private byte[] Base64UrlDecode(string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default:
                    throw new System.Exception(
                "Illegal base64url string!");
            }
            return System.Convert.FromBase64String(s); // Standard base64 decoder
        }

    }
}