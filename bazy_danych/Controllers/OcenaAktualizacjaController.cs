using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using bazy_danych.Data;
using Microsoft.EntityFrameworkCore;

namespace bazy_danych.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OcenaAktualizacjaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OcenaAktualizacjaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AktualizujOcene([FromQuery] int ocenaId, [FromQuery] decimal nowaOcena)
        {
            try
            {
                var conn = _context.Database.GetDbConnection();
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "PKG_Oceny.Zaktualizuj_Ocene";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new OracleParameter("p_ocena_id", ocenaId));
                cmd.Parameters.Add(new OracleParameter("p_nowa_ocena", nowaOcena));

                cmd.ExecuteNonQuery();

                return Ok("Ocena została zaktualizowana i zapisana do historii.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Błąd podczas aktualizacji oceny: " + ex.Message);
            }
        }
    }
}
