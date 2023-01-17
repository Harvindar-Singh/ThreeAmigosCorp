using System;
using System.Net.Http.Headers;

namespace Customer.Web.Services.Values
{
    public class ValuesService : IValuesService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public ValuesService(IHttpClientFactory clientFactory,
                             IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        record TokenDto(string access_token, string token_type, int expires_in);

        public async Task<ValuesGetDto> Get()
        {
            var tokenClient = _clientFactory.CreateClient();

            var authBaseAddress = _configuration["Auth:Authority"];
            tokenClient.BaseAddress = new Uri(authBaseAddress);

            var tokenParams = new Dictionary<string, string> {
            { "grant_type", "client_credentials" },
            { "client_id", _configuration["Auth:ClientId"] },
            { "client_secret", _configuration["Auth:ClientSecret"] },
            { "audience", _configuration["Services:Values:AuthAudience"] },
        };
            var tokenForm = new FormUrlEncodedContent(tokenParams);
            var tokenResponse = await tokenClient.PostAsync("oauth/token", tokenForm);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenInfo = await tokenResponse.Content.ReadFromJsonAsync<TokenDto>();

            // FIXME: token should be cached rather than obtained each call


            var client = _clientFactory.CreateClient();

            var serviceBaseAddress = _configuration["Services:Values:BaseAddress"];
            client.BaseAddress = new Uri(serviceBaseAddress);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenInfo?.access_token);

            var response = await client.GetAsync("api/values");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<ValuesGetDto>();
            return result;
        }
    }
}