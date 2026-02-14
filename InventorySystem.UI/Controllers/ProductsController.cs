using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using InventorySystem.UI.Models;

namespace InventorySystem.UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;
        private const string TokenCookie = "AccessToken";

        public ProductsController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        private void AddAuth(HttpClient client)
        {
            if (Request.Cookies.TryGetValue(TokenCookie, out var token) && !string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20)
        {
            var client = _httpFactory.CreateClient("Api");
            AddAuth(client);

            var resp = await client.GetAsync($"api/Products?pageNumber={pageNumber}&pageSize={pageSize}");
            if (!resp.IsSuccessStatusCode)
            {
                if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) return RedirectToAction("Login", "Account");
                return Content("Failed to load products");
            }

            using var s = await resp.Content.ReadAsStreamAsync();
            var products = JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(s, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? Enumerable.Empty<ProductViewModel>();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create() => View(new CreateProductViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpFactory.CreateClient("Api");
            AddAuth(client);

            var payload = JsonSerializer.Serialize(new
            {
                name = model.Name,
                categoryId = model.CategoryId,
                subCategoryId = model.SubCategoryId,
                quantity = model.Quantity,
                price = model.Price
            });

            var resp = await client.PostAsync("api/Products", new StringContent(payload, Encoding.UTF8, "application/json"));
            if (resp.IsSuccessStatusCode)
                return RedirectToAction("Index");

            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) return RedirectToAction("Login", "Account");

            model.Error = $"Create failed: {resp.StatusCode}";
            return View(model);
        }
    }
}