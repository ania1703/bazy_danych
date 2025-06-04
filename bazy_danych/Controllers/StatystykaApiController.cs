using bazy_danych.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatystykaApiController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StatystykaApiController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("policz-studentow")]
        public IActionResult PoliczStudentow([FromBody] PoliczStudentowRequest request)
        {
            string connStr = _config.GetConnectionString("OracleDb");

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "PoliczStudentowZMinOcena";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add("min_liczba_ocen", OracleDbType.Int32).Value = request.MinLiczbaOcen;

                var outputParam = new OracleParameter("liczba_studentow", OracleDbType.Int32)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                cmd.ExecuteNonQuery();

                int wynik = Convert.ToInt32(outputParam.Value.ToString());

                return Ok(new PoliczStudentowResponse { LiczbaStudentow = wynik });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd: " + ex.Message);
            }
        }
    }
}
