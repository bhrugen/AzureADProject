using AzureADB2CWeb.Helper;
using AzureADB2CWeb.Models;
using AzureADB2CWeb.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AzureADB2CWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger,
             IHttpClientFactory httpClientFactory, IUserService userService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _userService = userService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var b2cObjectId = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _userService.GetById(b2cObjectId);
                if(user==null || string.IsNullOrWhiteSpace(user.B2CObjectId))
                {
                    var role = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst("extension_UserRole").Value;

                    user = new()
                    {
                        B2CObjectId = b2cObjectId,
                        Email = ((ClaimsIdentity)this.HttpContext.User.Identity).FindFirst("emails").Value,
                        UserRole = role
                    };

                    _userService.Create(user);
                }
            }
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Permission("homeowner")]
        //[Authorize(Roles = "homeowner")]
        public IActionResult Homeowner()
        {
            return View();
        }

        [Permission("contractor")]
        //[Authorize(Roles = "contractor")]
        public IActionResult Contractor()
        {
            return View();
        }


        public IActionResult SignIn()
        {
            var scheme = OpenIdConnectDefaults.AuthenticationScheme;
            var redirectUrl = Url.ActionContext.HttpContext.Request.Scheme
               + "://" + Url.ActionContext.HttpContext.Request.Host;
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            }, scheme);
        }

        public IActionResult EditProfile()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "B2C_1_Edit");
        }


        public IActionResult SignOut()
        {
            var scheme = OpenIdConnectDefaults.AuthenticationScheme;
            return SignOut(new AuthenticationProperties(), CookieAuthenticationDefaults.AuthenticationScheme, scheme);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> APICall()
        {
            var acccessToken = await HttpContext.GetTokenAsync("access_token");

            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://localhost:44381/WeatherForecast");
            request.Headers.Authorization =
                new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, acccessToken);

            var response = await client.SendAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //issue
            }
            return Content(await response.Content.ReadAsStringAsync());

        }
    }
}
