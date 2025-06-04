using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using bazy_danych.Models;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NauczycielApiController : ControllerBase
    {
        private readonly IConfiguration _config;

        public NauczycielApiController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult GetNauczyciele()
        {
            string connStr = _config.GetConnectionString("OracleDb");
            List<Nauczyciel> nauczyciele = new();

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT nauczyciel_id, imie, nazwisko, email FROM nauczyciel";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    nauczyciele.Add(new Nauczyciel
                    {
                        NauczycielId = reader.GetInt32(0),
                        Imie = reader.GetString(1),
                        Nazwisko = reader.GetString(2),
                        Email = reader.GetString(3)
                    });
                }

                return Ok(nauczyciele);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd bazy danych: " + ex.Message);
            }
        }
    }
}
