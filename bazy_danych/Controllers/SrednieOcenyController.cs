using bazy_danych.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SrednieOcenyController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SrednieOcenyController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SredniaOcena>> Get()
        {
            string connStr = _config.GetConnectionString("OracleDb");
            List<SredniaOcena> oceny = new();

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT student_id, srednia_ocen FROM V_Srednie_Oceny";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    oceny.Add(new SredniaOcena
                    {
                        Student_Id = reader.GetInt32(0),
                        Srednia_Ocen = reader.GetDecimal(1)
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Błąd: {ex.Message}");
            }

            return Ok(oceny);
        }
    }
}
