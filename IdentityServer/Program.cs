using IdentityServer;
using IdentityServerHost.Quickstart.UI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddIdentityServer().AddInMemoryClients(Config.Clients)
               .AddInMemoryIdentityResources(Config.IdentityResources)
               //.AddInMemoryApiResources(Config.ApiResources)
               .AddInMemoryApiScopes(Config.ApiScopes)
                //.AddTestUsers(Config.TestUsers)
               .AddTestUsers(TestUsers.Users)
               .AddDeveloperSigningCredential();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.UseEndpoints(endpoints=>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();

//below command required for user interface
//iex((New - Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/main/getmain.ps1'))
