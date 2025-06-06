using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bazy_danych.Models;
using bazy_danych.Data;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoriaOcenApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoriaOcenApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistoriaOcen()
        {
            var historia = await _context.HistoriaOcen.ToListAsync();
            return Ok(historia);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHistoria(int id)
        {
            var wpis = await _context.HistoriaOcen.FindAsync(id);
            if (wpis == null)
                return NotFound("Nie znaleziono wpisu w historii.");

            return Ok(wpis);
        }

        [HttpPost]
        public async Task<IActionResult> DodajHistorie([FromBody] HistoriaOcen wpis)
        {
            _context.HistoriaOcen.Add(wpis);
            await _context.SaveChangesAsync();
            return Ok("Wpis dodany do historii ocen.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> UsunHistorie(int id)
        {
            var wpis = await _context.HistoriaOcen.FindAsync(id);
            if (wpis == null)
                return NotFound();

            _context.HistoriaOcen.Remove(wpis);
            await _context.SaveChangesAsync();
            return Ok("Wpis usunięty z historii ocen.");
        }
    }
}
