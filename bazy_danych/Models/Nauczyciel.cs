using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bazy_danych.Models
{
    public class Nauczyciel
    {
        [Key]
        [Column("NAUCZYCIEL_ID")]
        public int NauczycielId { get; set; }

        [Column("IMIE")]
        public string Imie { get; set; }

        [Column("NAZWISKO")]
        public string Nazwisko { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        // Nawigacja
        public ICollection<Ocena>? Oceny { get; set; }
    }
}
