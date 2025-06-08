using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using bazy_danych.Models;
using bazy_danych.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatystykaApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatystykaApiController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("policz-studentow")]
        public IActionResult PoliczStudentow([FromBody] PoliczStudentowRequest request)
        {
            try
            {
                var conn = _context.Database.GetDbConnection();
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "PoliczStudentowZMinOcena";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var paramMin = new OracleParameter("min_liczba_ocen", OracleDbType.Int32)
                {
                    Direction = System.Data.ParameterDirection.Input,
                    Value = request.MinLiczbaOcen
                };

                var paramOut = new OracleParameter("liczba_studentow", OracleDbType.Int32)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                cmd.Parameters.Add(paramMin);
                cmd.Parameters.Add(paramOut);

                cmd.ExecuteNonQuery();

                int wynik = Convert.ToInt32(paramOut.Value.ToString());

                return Ok(new PoliczStudentowResponse { LiczbaStudentow = wynik });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd: " + ex.Message);
            }
        }
    }
}
