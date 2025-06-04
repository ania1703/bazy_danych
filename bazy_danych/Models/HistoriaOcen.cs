namespace bazy_danych.Models
{
    public class HistoriaOcen
    {
        public int HistoriaId { get; set; }
        public int OcenaId { get; set; }
        public decimal OcenaStara { get; set; }
        public decimal OcenaNowa { get; set; }
        public DateTime DataZmiany { get; set; }
    }
}
