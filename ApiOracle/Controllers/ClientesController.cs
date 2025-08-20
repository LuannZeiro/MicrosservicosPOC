using Infrastructure;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiOracle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly OracleDbContext _context;

        public ClientesController(OracleDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Cliente>> Get()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Post(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
        }
    }
}
