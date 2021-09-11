using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SecretController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetSecret()
        {
            return Json("You got it");
        }

        public IActionResult GetSecretData()
        {
            return Json(new
            {
                Name = "Jay",
                Age = 20
            });
        }
    }
}