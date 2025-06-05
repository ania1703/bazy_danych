using bazy_danych.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZwiekszOcenyController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ZwiekszOcenyController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult ZwiekszOceny([FromBody] ZwiekszOcenyRequest request)
        {
            string connStr = _config.GetConnectionString("OracleDb");

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "ZwiekszOcenyStudentom";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add("p_przedmiot_id", OracleDbType.Int32).Value = request.PrzedmiotId;
                cmd.Parameters.Add("p_bonus", OracleDbType.Decimal).Value = request.Bonus;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd: {ex.Message}");
            }

            return Ok("Oceny zostały zaktualizowane.");
        }
    }
}
