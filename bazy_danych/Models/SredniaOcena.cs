using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bazy_danych.Models
{
    public class SredniaOcena
    {
        [Column("STUDENT_ID")]
        public int Student_Id { get; set; }

        [Column("SREDNIA_OCEN")]
        public decimal Srednia_Ocen { get; set; }
    }
}
