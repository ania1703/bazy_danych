using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using bazy_danych.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZaliczeniaApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ZaliczeniaApiController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "student,nauczyciel,admin")]
        [HttpGet("czy-zaliczyl")]
        public IActionResult CzyZaliczyl([FromQuery] int studentId, [FromQuery] int przedmiotId)
        {
            try
            {
                var conn = _context.Database.GetDbConnection();
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "CzyStudentZaliczylPrzedmiot";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new OracleParameter("p_student_id", studentId));
                cmd.Parameters.Add(new OracleParameter("p_przedmiot_id", przedmiotId));

                var output = new OracleParameter("p_zaliczyl", OracleDbType.Varchar2, 20)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                cmd.Parameters.Add(output);

                cmd.ExecuteNonQuery();

                var wynik = output.Value?.ToString() ?? "BRAK WYNIKU";
                return Ok(new { studentId, przedmiotId, zaliczyl = wynik });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd: " + ex.Message);
            }
        }
    }
}
