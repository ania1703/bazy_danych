using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using bazy_danych.Models;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OcenaApiController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OcenaApiController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult GetOceny()
        {
            string connStr = _config.GetConnectionString("OracleDb");
            List<Ocena> oceny = new();

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ocena_id, student_id, przedmiot_id, nauczyciel_id, ocena, data_oceny FROM ocena";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ocena = new Ocena
                    {
                        OcenaId = reader.GetInt32(0),
                        StudentId = reader.GetInt32(1),
                        PrzedmiotId = reader.GetInt32(2),
                        NauczycielId = reader.GetInt32(3),
                        OcenaWartosc = reader.GetDecimal(4),
                        DataOceny = reader.GetDateTime(5)
                    };
                    oceny.Add(ocena);
                }

                return Ok(oceny);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd bazy danych: " + ex.Message);
            }
        }
        [HttpPost]
        public IActionResult DodajOcene([FromBody] Ocena nowaOcena)
        {
            string connStr = _config.GetConnectionString("OracleDb");

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
            INSERT INTO ocena (ocena_id, student_id, przedmiot_id, nauczyciel_id, ocena, data_oceny)
            VALUES (:id, :student, :przedmiot, :nauczyciel, :wartosc, :data)";

                cmd.Parameters.Add(new OracleParameter("id", nowaOcena.OcenaId));
                cmd.Parameters.Add(new OracleParameter("student", nowaOcena.StudentId));
                cmd.Parameters.Add(new OracleParameter("przedmiot", nowaOcena.PrzedmiotId));
                cmd.Parameters.Add(new OracleParameter("nauczyciel", nowaOcena.NauczycielId));
                cmd.Parameters.Add(new OracleParameter("wartosc", nowaOcena.OcenaWartosc));
                cmd.Parameters.Add(new OracleParameter("data", nowaOcena.DataOceny));

                cmd.ExecuteNonQuery();

                return Ok("Ocena dodana.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd bazy danych: " + ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult UsunOcene(int id)
        {
            string connStr = _config.GetConnectionString("OracleDb");

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM ocena WHERE ocena_id = :id";
                cmd.Parameters.Add(new OracleParameter("id", id));

                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                {
                    return NotFound("Nie znaleziono oceny o podanym ID.");
                }

                return Ok("Ocena usunięta.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd bazy danych: " + ex.Message);
            }
        }

    }

}
