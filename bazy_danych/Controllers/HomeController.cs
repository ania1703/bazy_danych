using System.Diagnostics;
using bazy_danych.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace bazy_danych.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ListaTabel()
        {
            string connStr = _config.GetConnectionString("OracleDb");
            List<string> tables = new List<string>();

            try
            {
                using var conn = new OracleConnection(connStr);
                conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT TABLE_NAME FROM USER_TABLES";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "B³¹d: " + ex.Message;
            }

            return View(tables);
        }
        public IActionResult SrednieOceny()
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
                ViewBag.Error = "B³¹d: " + ex.Message;
            }

            return View(oceny);
        }
        [HttpGet]
        public IActionResult Zaliczenie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Zaliczenie(int studentId, int przedmiotId)
        {
            try
            {
                var conn = _config.GetConnectionString("OracleDb");
                using var connection = new OracleConnection(conn);
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = "CzyStudentZaliczylPrzedmiot";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new OracleParameter("p_student_id", studentId));
                cmd.Parameters.Add(new OracleParameter("p_przedmiot_id", przedmiotId));

                var output = new OracleParameter("p_zaliczyl", OracleDbType.Varchar2, 20)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                cmd.Parameters.Add(output);

                cmd.ExecuteNonQuery();

                ViewBag.Wynik = output.Value?.ToString() ?? "BRAK WYNIKU";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "B³¹d: " + ex.Message;
            }

            return View();
        }

    }

}
