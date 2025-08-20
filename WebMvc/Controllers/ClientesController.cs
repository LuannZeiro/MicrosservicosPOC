using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;

namespace WebMvc.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Lista clientes
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5289/"); // ApiOracle

            var response = await client.GetAsync("clientes");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var clientes = JsonSerializer.Deserialize<List<Cliente>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(clientes);
        }

        // Formulário GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Formulário POST
        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5289/"); // ApiOracle

            var response = await client.PostAsJsonAsync("clientes", cliente);
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Index));
        }
    }
}
