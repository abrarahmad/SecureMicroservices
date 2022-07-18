using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients => new Client[]
             {
                new Client
                {
                    ClientId = "movieClient",
                    AllowedGrantTypes = new List<string>{GrantType.ClientCredentials },
                    ClientSecrets =
                    {
                        new ("secret".Sha256())
                    },
                    AllowedScopes={"movieAPI"}
                },
                new Client
                {
                    ClientId="movies_mvc_client",
                    ClientName="Movies MVC Web App",
                    //AllowedGrantTypes=new List<string>{GrantType.AuthorizationCode},
                    AllowedGrantTypes=new List<string>{GrantType.Hybrid},
                    RequirePkce = false,//add for hybrid flow
                    AllowRememberConsent=false,

                    RedirectUris= new List<string>
                    {
                        "https://localhost:5002/signin-oidc"
                    },
                    PostLogoutRedirectUris= new List<string>
                    {
                        "https://localhost:5002/signout-callback-oidc"
                    },
                     ClientSecrets =
                    {
                        new ("secret".Sha256())
                    },
                      AllowedScopes= new List<string>
                      {
                          IdentityServerConstants.StandardScopes.OpenId,
                          IdentityServerConstants.StandardScopes.Profile,
                          IdentityServerConstants.StandardScopes.Address,
                          IdentityServerConstants.StandardScopes.Email,
                          "movieAPI",
                          "roles"

                      }
                }

             };
        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
          {
              new ("movieAPI","Movie API")
          };
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
           {

           };
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
         {
             new IdentityResources.OpenId(),
             new IdentityResources.Profile(),
             new IdentityResources.Address(),
             new IdentityResources.Email(),
             new IdentityResource("roles","your role(s)",new List<string>{ "role"})
         };
        public static List<TestUser> TestUsers => new List<TestUser>
        {
            new TestUser
            {
                SubjectId="4D71E892-3275-4E93-828A-08EA15CDC1FC",
                Username="abrar",
                Password="abrar",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName,"Abrar Ahmad Ansari"),
                    new Claim(JwtClaimTypes.FamilyName,"Abrar"),
                }
            }
        };
    }
}