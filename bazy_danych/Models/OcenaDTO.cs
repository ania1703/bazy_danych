using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bazy_danych.Models
{
    public class OcenaDTO
    {
        public int OcenaId { get; set; }
        public string StudentImie { get; set; }
        public string StudentNazwisko { get; set; }
        public string PrzedmiotNazwa { get; set; }
        public string NauczycielImie { get; set; }
        public decimal? Ocena { get; set; }
        public DateTime Data { get; set; }
    }
}

