using System.Text;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Model;
using Movies.Client.Models;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MovieApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));

        }
        public async Task<Movie> CreateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var movieJson = new StringContent(
                                JsonConvert.SerializeObject(movie),
                                Encoding.UTF8,
                                Application.Json);

            //var response = await httpClient.PostAsync($"/api/movies/", movieJson).ConfigureAwait(false); this is required without api gatway
            var response = await httpClient.PostAsync($"/movies/", movieJson).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var insertedMovie = JsonConvert.DeserializeObject<Movie>(content);
            return insertedMovie!;
        }

        public async Task DeleteMovie(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            //var response = await httpClient.DeleteAsync($"/api/movies/{id}").ConfigureAwait(false);this is required without api gatway
            var response = await httpClient.DeleteAsync($"/movies/{id}").ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

        }

        public async Task<Movie> GetMovie(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            //var request = new HttpRequestMessage(
            //    HttpMethod.Get,
            //    $"/api/movies/{id}"); this is required without api gatway

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"/movies/{id}");

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(content);
            return movie!;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            ////////////////////////
            // WAY 1 :

            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            //var response = await httpClient.GetAsync("/api/movies").ConfigureAwait(false); this required without api gateway

            var response = await httpClient.GetAsync("/movies").ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            return movieList!;

            ////////////////////////// //////////////////////// ////////////////////////
            //// WAY 2 :

            //// 1. "retrieve" our api credentials. This must be registered on Identity Server!
            //var apiClientCredentials = new ClientCredentialsTokenRequest
            //{
            //    Address = "https://localhost:5005/connect/token",

            //    ClientId = "movieClient",
            //    ClientSecret = "secret",

            //    // This is the scope our Protected API requires. 
            //    Scope = "movieAPI"
            //};

            //// creates a new HttpClient to talk to our IdentityServer (localhost:5005)
            //var client = new HttpClient();

            //// just checks if we can reach the Discovery document. Not 100% needed but..
            //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5005");
            //if (disco.IsError)
            //{
            //    return null; // throw 500 error
            //}

            //// 2. Authenticates and get an access token from Identity Server
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);            
            //if (tokenResponse.IsError)
            //{
            //    return null;
            //}

            //// Another HttpClient for talking now with our Protected API
            //var apiClient = new HttpClient();

            //// 3. Set the access_token in the request Authorization: Bearer <token>
            //client.SetBearerToken(tokenResponse.AccessToken);

            //// 4. Send a request to our Protected API
            //var response = await client.GetAsync("https://localhost:5001/api/movies");
            //response.EnsureSuccessStatusCode();

            //var content = await response.Content.ReadAsStringAsync();

            //var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            //return movieList!;

        }

        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var idpClient = _httpClientFactory.CreateClient("IDPClient");
            var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();

            if (metaDataResponse.IsError)
            {
                throw new HttpRequestException($"Something went wrong while requesting the access token, exception details {metaDataResponse.HttpErrorReason}");
            }
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).ConfigureAwait(false);
            var userInfoResponse = await idpClient.GetUserInfoAsync(
                new UserInfoRequest
                {
                    Address = metaDataResponse.UserInfoEndpoint,
                    Token = accessToken
                });

            if (userInfoResponse.IsError)
            {

                throw new HttpRequestException($"Something went wrong while getting user infor, exception details {userInfoResponse.HttpErrorReason}");
            }

            var userInfoDict = userInfoResponse.Claims.ToDictionary(key => key.Type, value => value.Value);
            
            return new UserInfoViewModel(userInfoDict);
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var movieJson = new StringContent(
                                JsonConvert.SerializeObject(movie),
                                Encoding.UTF8,
                                Application.Json);

            //var response = await httpClient.PutAsync($"/api/movies/{movie.Id}", movieJson).ConfigureAwait(false); this is required without api gatway
            var response = await httpClient.PutAsync($"/movies/{movie.Id}", movieJson).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var updatedMovie = JsonConvert.DeserializeObject<Movie>(content);
            return updatedMovie!;
        }
    }
}
