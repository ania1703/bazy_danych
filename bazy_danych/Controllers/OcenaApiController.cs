using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bazy_danych.Models;
using bazy_danych.Data;
using Microsoft.AspNetCore.Authorization;

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


        /*[HttpGet]
        public async Task<IActionResult> GetOceny()
        {
            var oceny = await _context.Oceny
                .Include(o => o.Student)
                .Include(o => o.Przedmiot)
                .Include(o => o.Nauczyciel)
                .ToListAsync();

            return Ok(oceny);
        }*/

        // GET: api/Ocena/5
        [Authorize(Roles = "student, nauczyciel, admin")]
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

        [Authorize(Roles = "nauczyciel, admin")]
        [HttpGet("lite")]
        public async Task<IActionResult> GetOcenyLite(int page = 1, int pageSize = 10)
        {
            var query = _context.Oceny
                .Include(o => o.Student)
                .Include(o => o.Przedmiot)
                .Include(o => o.Nauczyciel)
                .Select(o => new OcenaDTO
                {
                    OcenaId = o.OcenaId,
                    StudentImie = o.Student.Imie,
                    StudentNazwisko = o.Student.Nazwisko,
                    PrzedmiotNazwa = o.Przedmiot.Nazwa,
                    NauczycielImie = o.Nauczyciel.Imie,
                    Ocena = o.OcenaWartosc,
                    Data = o.DataOceny
                });

            var totalItems = await query.CountAsync();

            var pagedItems = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                Items = pagedItems
            };

            return Ok(result);
        }



        // POST: api/Ocena
        [Authorize(Roles = "nauczyciel, admin")]
        [HttpPost]
        public async Task<IActionResult> DodajOcene([FromBody] DodajOceneRequest req)
        {
            var nowaOcena = new Ocena
            {
                OcenaId = req.OcenaId,
                StudentId = req.StudentId,
                PrzedmiotId = req.PrzedmiotId,
                NauczycielId = req.NauczycielId,
                OcenaWartosc = req.OcenaWartosc,
                DataOceny = req.DataOceny
            };

            _context.Oceny.Add(nowaOcena);
            await _context.SaveChangesAsync();

            return Ok("Ocena dodana.");
        }


        // DELETE: api/Ocena/5
        [Authorize(Roles = "nauczyciel, admin")]
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
