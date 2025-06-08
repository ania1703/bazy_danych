using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bazy_danych.Models;
using bazy_danych.Data;
using Microsoft.AspNetCore.Authorization;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RankingApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RankingApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "student,nauczyciel,admin")]
        public async Task<IActionResult> GetRanking()
        {
            var ranking = await _context.Rankingi.ToListAsync();
            return Ok(ranking);
        }

        [Authorize(Roles = "student,nauczyciel,admin")]
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetRankingDlaStudenta(int studentId)
        {
            var wpis = await _context.Rankingi.FindAsync(studentId);
            if (wpis == null)
                return NotFound("Nie znaleziono rankingu dla tego studenta.");

            return Ok(wpis);
        }

        [Authorize(Roles = "nauczyciel,admin")]
        [HttpPost]
        public async Task<IActionResult> DodajRanking([FromBody] Ranking nowy)
        {
            _context.Rankingi.Add(nowy);
            await _context.SaveChangesAsync();
            return Ok("Ranking dodany.");
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{studentId}")]
        public async Task<IActionResult> UsunRanking(int studentId)
        {
            var wpis = await _context.Rankingi.FindAsync(studentId);
            if (wpis == null)
                return NotFound();

            _context.Rankingi.Remove(wpis);
            await _context.SaveChangesAsync();
            return Ok("Ranking usunięty.");
        }
    }
}
