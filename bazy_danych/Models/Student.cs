using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bazy_danych.Models
{
    public class Student
    {
        [Key]
        [Column("STUDENT_ID")]
        public int StudentId { get; set; }

        [Column("IMIE")]
        public string Imie { get; set; }

        [Column("NAZWISKO")]
        public string Nazwisko { get; set; }

        [Column("NR_INDEKSU")]
        public string NrIndeksu { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        // Nawigacja
        public ICollection<Ocena>? Oceny { get; set; }
    }
}
