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

    }

}
