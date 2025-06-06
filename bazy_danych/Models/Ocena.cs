namespace bazy_danych.Models
{
    public class Ocena
    {
        public int OcenaId { get; set; }
        public int StudentId { get; set; }
        public int PrzedmiotId { get; set; }
        public int NauczycielId { get; set; }
        public decimal OcenaWartosc { get; set; }
        public DateTime DataOceny { get; set; }

        public Student Student { get; set; }
        public Przedmiot Przedmiot { get; set; }
        public Nauczyciel Nauczyciel { get; set; }
    }
}
