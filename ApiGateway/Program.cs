using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", false, true);
builder.Services.AddOcelot();
var authenticationProviderKey = "IdentityApiKey";
builder.Services.AddAuthentication()
            .AddJwtBearer(authenticationProviderKey, x =>
            {
                x.Authority = "https://localhost:5005"; // IDENTITY SERVER URL
                                                        //x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.MapControllers();
await app.UseOcelot().ConfigureAwait(false);
app.Run();
