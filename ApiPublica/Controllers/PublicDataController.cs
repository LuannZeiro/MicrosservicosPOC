using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ApiPublica.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicDataController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PublicDataController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = _httpClientFactory.CreateClient("PublicApi");

            try
            {
                var response = await client.GetAsync("/");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao chamar API pública: {ex.Message}");
            }
        }
    }
}
