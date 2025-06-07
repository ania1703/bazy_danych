using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bazy_danych.Models
{
    public class Przedmiot
    {
        [Key]
        [Column("PRZEDMIOT_ID")]
        public int PrzedmiotId { get; set; }

        [Column("NAZWA")]
        public string Nazwa { get; set; }

        [Column("SEMESTR")]
        public string Semestr { get; set; }

        // Nawigacja
        public ICollection<Ocena> Oceny { get; set; }
    }
}
