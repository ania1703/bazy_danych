using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bazy_danych.Models;
using bazy_danych.Data;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZwiekszOcenyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ZwiekszOcenyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ZwiekszOceny([FromBody] ZwiekszOcenyRequest request)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "BEGIN ZwiekszOcenyStudentom(:przedmiotId, :bonus); END;",
                    new Oracle.ManagedDataAccess.Client.OracleParameter("przedmiotId", request.PrzedmiotId),
                    new Oracle.ManagedDataAccess.Client.OracleParameter("bonus", request.Bonus)
                );

                return Ok("Oceny zostały zaktualizowane.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd: {ex.Message}");
            }
        }
    }
}
