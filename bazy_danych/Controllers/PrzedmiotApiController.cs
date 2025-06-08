using Microsoft.AspNetCore.Mvc;
using bazy_danych.Models;
using bazy_danych.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrzedmiotApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PrzedmiotApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrzedmioty()
        {
            var przedmioty = await _context.Przedmioty.ToListAsync();
            return Ok(przedmioty);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrzedmiot(int id)
        {
            var przedmiot = await _context.Przedmioty.FindAsync(id);
            if (przedmiot == null)
                return NotFound();
            return Ok(przedmiot);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DodajPrzedmiot([FromBody] Przedmiot przedmiot)
        {
            _context.Przedmioty.Add(przedmiot);
            await _context.SaveChangesAsync();
            return Ok("Przedmiot dodany.");
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> UsunPrzedmiot(int id)
        {
            var przedmiot = await _context.Przedmioty.FindAsync(id);
            if (przedmiot == null)
                return NotFound();

            _context.Przedmioty.Remove(przedmiot);
            await _context.SaveChangesAsync();
            return Ok("Przedmiot usunięty.");
        }
    }
}
