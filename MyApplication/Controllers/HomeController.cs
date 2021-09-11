using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyApplication.Models;

namespace MyApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult ThirdPartyLogin()
        {
            return Redirect("http://localhost:5001/Auth");
        }

        public IActionResult ThirdPartyAfterLogin(string token)
        {
            using var httpClient = new HttpClient();
            var response = httpClient.PostAsync("http://localhost:5001/Auth/ValidateAuthorizationGrant",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"AccessToken", token}
                }));
            response.Wait();
            var resultContentTask = response.Result.Content.ReadAsStringAsync();
            resultContentTask.Wait();

            using var apiHttpClient = new HttpClient();
            apiHttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", JsonSerializer.Deserialize<string>(resultContentTask.Result));
            var getSecretTask = apiHttpClient.PostAsync("http://localhost:5001/Secret/GetSecret", null);
            getSecretTask.Wait();
            var readAsStringAsync = getSecretTask.Result.Content.ReadAsStringAsync();
            readAsStringAsync.Wait();
            Response.Cookies.Append("AccessToken", JsonSerializer.Deserialize<string>(resultContentTask.Result));
            return RedirectToAction("Index", "AfterLogin");
        }
    }
}