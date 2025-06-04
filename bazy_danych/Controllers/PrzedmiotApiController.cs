using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using bazy_danych.Models;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrzedmiotApiController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PrzedmiotApiController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult GetPrzedmioty()
        {
            string connStr = _config.GetConnectionString("OracleDb");
            List<Przedmiot> przedmioty = new();

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT przedmiot_id, nazwa, semestr FROM przedmiot";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    przedmioty.Add(new Przedmiot
                    {
                        PrzedmiotId = reader.GetInt32(0),
                        Nazwa = reader.GetString(1),
                        Semestr = reader.GetString(2)
                    });
                }

                return Ok(przedmioty);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd bazy danych: " + ex.Message);
            }
        }
    }
}
