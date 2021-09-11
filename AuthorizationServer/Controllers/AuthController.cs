using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using AuthorizationServer.Models;
using AuthorizationServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identity;
        private string _token = "c35ca694-5554-4d2a-a783-42a06cfdd7d8";

        public AuthController(IIdentityService identity)
        {
            _identity = identity;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(LoginInfo loginInfo)
        {
            if (_identity.Login(new Identity
            {
                UserId = loginInfo.UserId,
                Password = loginInfo.Password
            }))
            {
                return Redirect($"http://localhost:5002/Home/ThirdPartyAfterLogin?token={_token}");
            }

            return Forbid();
        }

        public IActionResult ValidateAuthorizationGrant(AuthorizationModel model)
        {
            if (model.AccessToken == _token)
            {
                //generate JWT 
                var jwtSecurityToken = new JwtSecurityToken(
                    claims: new List<Claim>
                    {
                        new Claim("UserID", "test"),
                        new Claim("UserName", "Jay")
                    },
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("f97e47be-f8c2-41b2-956f-677b28a66852")),
                        SecurityAlgorithms.HmacSha256)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return Json(token);
            }

            return Json(true);
        }
    }
}