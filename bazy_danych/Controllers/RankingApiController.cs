using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using bazy_danych.Models;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RankingApiController : ControllerBase
    {
        private readonly IConfiguration _config;

        public RankingApiController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult GetRanking()
        {
            string connStr = _config.GetConnectionString("OracleDb");
            List<Ranking> ranking = new();

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT student_id, srednia_ocen, miejsce_w_rankingu FROM ranking";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ranking.Add(new Ranking
                    {
                        StudentId = reader.GetInt32(0),
                        SredniaOcen = reader.GetDecimal(1),
                        MiejsceWRankingu = reader.GetInt32(2)
                    });
                }

                return Ok(ranking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd bazy danych: " + ex.Message);
            }
        }
    }
}
