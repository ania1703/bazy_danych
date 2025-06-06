using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bazy_danych.Models;
using bazy_danych.Data;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OcenaApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OcenaApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Ocena
        [HttpGet]
        public async Task<IActionResult> GetOceny()
        {
            var oceny = await _context.Oceny
                .Include(o => o.Student)
                .Include(o => o.Przedmiot)
                .Include(o => o.Nauczyciel)
                .ToListAsync();

            return Ok(oceny);
        }

        // GET: api/Ocena/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOcena(int id)
        {
            var ocena = await _context.Oceny
                .Include(o => o.Student)
                .Include(o => o.Przedmiot)
                .Include(o => o.Nauczyciel)
                .FirstOrDefaultAsync(o => o.OcenaId == id);

            if (ocena == null)
                return NotFound("Nie znaleziono oceny.");

            return Ok(ocena);
        }

        // POST: api/Ocena
        [HttpPost]
        public async Task<IActionResult> DodajOcene([FromBody] Ocena nowaOcena)
        {
            _context.Oceny.Add(nowaOcena);
            await _context.SaveChangesAsync();
            return Ok("Ocena dodana.");
        }

        // DELETE: api/Ocena/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> UsunOcene(int id)
        {
            var ocena = await _context.Oceny.FindAsync(id);
            if (ocena == null)
                return NotFound("Nie znaleziono oceny o podanym ID.");

            _context.Oceny.Remove(ocena);
            await _context.SaveChangesAsync();
            return Ok("Ocena usunięta.");
        }
    }
}
