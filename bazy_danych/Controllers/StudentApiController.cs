using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using bazy_danych.Models;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentApiController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StudentApiController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult GetStudenci()
        {
            string connStr = _config.GetConnectionString("OracleDb");
            List<Student> studenci = new();

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT student_id, imie, nazwisko, nr_indeksu, email FROM student";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var student = new Student
                    {
                        StudentId = reader.GetInt32(0),
                        Imie = reader.GetString(1),
                        Nazwisko = reader.GetString(2),
                        NrIndeksu = reader.GetString(3),
                        Email = reader.GetString(4)
                    };
                    studenci.Add(student);
                }

                return Ok(studenci);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd bazy danych: " + ex.Message);
            }
        }
    }
}
