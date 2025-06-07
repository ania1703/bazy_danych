using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bazy_danych.Models
{
    public class Ranking
    {
        [Key]
        [Column("STUDENT_ID")]
        public int StudentId { get; set; }

        [Column("SREDNIA_OCEN")]
        public decimal SredniaOcen { get; set; }
    }
}
