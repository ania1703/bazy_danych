﻿using Microsoft.AspNetCore.Mvc;
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

        [Authorize(Roles = "admin, nauczyciel")]
        [HttpGet]
        public async Task<IActionResult> GetStudenci(int page = 1, int pageSize = 10)
        {
            var query = _context.Studenci.AsQueryable();

            var totalItems = await query.CountAsync();
            var studenci = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                Items = studenci
            };

            return Ok(result);
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
            var student = await _context.Studenci
        .Include(s => s.Oceny) // załaduj oceny studenta
        .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
                return NotFound("Nie znaleziono studenta.");

            if (student.Oceny.Any())
                return BadRequest("Nie można usunąć studenta, który ma przypisane oceny.");

            _context.Studenci.Remove(student);
            await _context.SaveChangesAsync();
            return Ok("Student usunięty.");
        }
    }
}
