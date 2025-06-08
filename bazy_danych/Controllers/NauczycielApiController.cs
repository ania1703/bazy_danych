using Microsoft.AspNetCore.Mvc;
using bazy_danych.Models;
using bazy_danych.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NauczycielApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NauczycielApiController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetNauczyciele(int page = 1, int pageSize = 10)
        {
            var query = _context.Nauczyciele.AsQueryable();

            var totalItems = await query.CountAsync();
            var nauczyciele = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                Items = nauczyciele
            };

            return Ok(result);
        }

        [Authorize(Roles = "admin, nauczyciel")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNauczyciel(int id)
        {
            var nauczyciel = await _context.Nauczyciele.FindAsync(id);
            if (nauczyciel == null)
                return NotFound();
            return Ok(nauczyciel);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DodajNauczyciela([FromBody] Nauczyciel nauczyciel)
        {
            _context.Nauczyciele.Add(nauczyciel);
            await _context.SaveChangesAsync();
            return Ok("Nauczyciel dodany.");
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> UsunNauczyciela(int id)
        {
            var nauczyciel = await _context.Nauczyciele.FindAsync(id);
            if (nauczyciel == null)
                return NotFound();

            _context.Nauczyciele.Remove(nauczyciel);
            await _context.SaveChangesAsync();
            return Ok("Nauczyciel usunięty.");
        }
    }
}
