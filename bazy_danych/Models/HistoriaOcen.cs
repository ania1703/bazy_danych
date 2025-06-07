using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bazy_danych.Models
{
    public class HistoriaOcen
    {
        [Key]
        [Column("HISTORIA_ID")]
        public int HistoriaId { get; set; }

        [Column("OCENA_ID")]
        public int OcenaId { get; set; }

        [Column("OCENA_STARA")]
        public decimal OcenaStara { get; set; }

        [Column("OCENA_NOWA")]
        public decimal OcenaNowa { get; set; }

        [Column("DATA_ZMIANY")]
        public DateTime DataZmiany { get; set; }
    }
}
