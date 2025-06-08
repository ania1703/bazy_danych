using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bazy_danych.Models
{
    public class Ocena
    {
        [Key]
        [Column("OCENA_ID")]
        public int OcenaId { get; set; }

        [Column("STUDENT_ID")]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [Column("PRZEDMIOT_ID")]
        public int PrzedmiotId { get; set; }

        [ForeignKey("PrzedmiotId")]
        public Przedmiot Przedmiot { get; set; }

        [Column("NAUCZYCIEL_ID")]
        public int NauczycielId { get; set; }

        [ForeignKey("NauczycielId")]
        public Nauczyciel Nauczyciel { get; set; }

        [Column("OCENA")]
        public decimal? OcenaWartosc { get; set; }

        [Column("DATA_OCENY")]
        public DateTime DataOceny { get; set; }
    }
}
