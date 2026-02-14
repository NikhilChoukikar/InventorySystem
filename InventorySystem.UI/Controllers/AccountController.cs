using InventorySystem.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace InventorySystem.UI.Controllers
{
    public class AccountController : Controller
    {
            private readonly IHttpClientFactory _httpFactory;
            private const string TokenCookie = "AccessToken";

            public AccountController(IHttpClientFactory httpFactory)
            {
                _httpFactory = httpFactory;
            }

            [HttpGet]
            public IActionResult Login() => View(new LoginViewModel());

            [HttpPost]
            public async Task<IActionResult> Login(LoginViewModel model)
            {
                if (!ModelState.IsValid) return View(model);

                var client = _httpFactory.CreateClient("Api");
                var payload = JsonSerializer.Serialize(new { email = model.Email, password = model.Password });
                var resp = await client.PostAsync("api/auth/login", new StringContent(payload, Encoding.UTF8, "application/json"));

                if (!resp.IsSuccessStatusCode)
                {
                    model.Error = "Login failed";
                    return View(model);
                }

                using var s = await resp.Content.ReadAsStreamAsync();
                var doc = await JsonSerializer.DeserializeAsync<JsonElement>(s);
                if (doc.TryGetProperty("accessToken", out var tokenProp))
                {
                    var token = tokenProp.GetString();
                    if (!string.IsNullOrEmpty(token))
                    {
                        Response.Cookies.Append(TokenCookie, token, new CookieOptions { HttpOnly = true, Secure = Request.IsHttps });
                        return RedirectToAction("Index", "Products");
                    }
                }

                model.Error = "Invalid login response";
                return View(model);
            }

            [HttpPost]
            public IActionResult Logout()
            {
                Response.Cookies.Delete(TokenCookie);
                return RedirectToAction("Login");
            }
        }
    }


