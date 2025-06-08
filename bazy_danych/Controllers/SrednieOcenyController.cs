using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bazy_danych.Models;
using bazy_danych.Data;
using Microsoft.AspNetCore.Authorization;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SrednieOcenyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SrednieOcenyController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "student,nauczyciel,admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var oceny = await _context.Set<SredniaOcena>()
                .FromSqlRaw("SELECT student_id AS Student_Id, srednia_ocen AS Srednia_Ocen FROM V_Srednie_Oceny")
                .ToListAsync();

            return Ok(oceny);
        }
    }
}
