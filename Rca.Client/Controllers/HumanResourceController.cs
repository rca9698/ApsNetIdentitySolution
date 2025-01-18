using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rca.Client.Models;
using System.Net.Http.Headers;

namespace Rca.Client.Controllers
{
    [Authorize(Policy = "MustBelongToHRDepartment")]
    public class HumanResourceController : Controller
    {
        private readonly HttpClient _httpClient;
        JWTToken token = new JWTToken();
        public HumanResourceController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("OurWebAPI");
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "HRManagerOnly")]
        public async Task<IActionResult> ManagerIndex()
        {

            var strTokenObj = HttpContext.Session.GetString("access_token");
            if (string.IsNullOrEmpty(strTokenObj))
            {
                token = await Authenticate();
            }
            else
            {
                token = JsonConvert.DeserializeObject<JWTToken>(strTokenObj);
            }

            if (token == null
                || string.IsNullOrEmpty(token.AccessToken)
                || token.ExpiresAt <= DateTime.UtcNow)
            {
                token = await Authenticate();
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);

            var data = await _httpClient.GetAsync("WeatherForecast");
            string resp = await data.Content.ReadAsStringAsync();
            return View((object)resp);
        }


        private async Task<JWTToken> Authenticate()
        {
            var res = await _httpClient.PostAsJsonAsync("api/Auth", new CredentialsModal() { UserName = "admin", Password = "password" });
            res.EnsureSuccessStatusCode();
            string jwtStr = await res.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("access_token", jwtStr);

            return JsonConvert.DeserializeObject<JWTToken>(jwtStr);
        }
    }
}
