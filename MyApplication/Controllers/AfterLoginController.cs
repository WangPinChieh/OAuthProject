using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyApplication.Controllers
{
    public class AfterLoginController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult QueryApi()
        {
            var accessToken = Request.Cookies["AccessToken"];
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
            var postAsync = httpClient.PostAsync("http://localhost:5001/Secret/GetSecretData", null);
            postAsync.Wait();

            var readAsStringAsync = postAsync.Result.Content.ReadAsStringAsync();
            return Json(readAsStringAsync.Result);
        }
    }
}