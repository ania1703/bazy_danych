using Microsoft.AspNetCore.Mvc;
using bazy_danych.Models;
using bazy_danych.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentApiController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetStudenci()
        {
            var studenci = await _context.Studenci.ToListAsync();
            return Ok(studenci);
        }

        [Authorize(Roles = "student, admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _context.Studenci.FindAsync(id);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DodajStudenta([FromBody] Student student)
        {
            _context.Studenci.Add(student);
            await _context.SaveChangesAsync();
            return Ok("Student dodany.");
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> UsunStudenta(int id)
        {
            var student = await _context.Studenci.FindAsync(id);
            if (student == null)
                return NotFound();

            _context.Studenci.Remove(student);
            await _context.SaveChangesAsync();
            return Ok("Student usunięty.");
        }
    }
}
