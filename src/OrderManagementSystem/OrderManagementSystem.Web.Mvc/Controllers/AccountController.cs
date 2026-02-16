using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Core.Dtos;

namespace OrderManagementSystem.Web.Mvc.Controllers
{
    public class AccountController: Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory= httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var resp = await client.PostAsJsonAsync("api/auth/login", model);

            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Error = "Login Failed";
                return View(model);
            }

            var login = await resp.Content.ReadFromJsonAsync<LoginResponse>();

            // Token'ı HttpOnly cookie'de sakla (güvenli)
            Response.Cookies.Append("access_token", login!.Token, new CookieOptions
            {
                HttpOnly=true,
                Secure=true,
                SameSite=SameSiteMode.Lax,
                Expires=login.ExpiresAtUtc
            });

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
