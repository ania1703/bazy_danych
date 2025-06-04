using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using bazy_danych.Models;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoriaOcenApiController : ControllerBase
    {
        private readonly IConfiguration _config;

        public HistoriaOcenApiController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult GetHistoriaOcen()
        {
            string connStr = _config.GetConnectionString("OracleDb");
            List<HistoriaOcen> historia = new();

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT historia_id, ocena_id, ocena_stara, ocena_nowa, data_zmiany FROM historia_ocen";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    historia.Add(new HistoriaOcen
                    {
                        HistoriaId = reader.GetInt32(0),
                        OcenaId = reader.GetInt32(1),
                        OcenaStara = reader.GetDecimal(2),
                        OcenaNowa = reader.GetDecimal(3),
                        DataZmiany = reader.GetDateTime(4)
                    });
                }

                return Ok(historia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd bazy danych: " + ex.Message);
            }
        }
    }
}
