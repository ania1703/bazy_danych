namespace bazy_danych.Models
{
    public class Przedmiot
    {
        public int PrzedmiotId { get; set; }
        public string Nazwa { get; set; }
        public string Semestr { get; set; }

        // Nawigacja
        public ICollection<Ocena> Oceny { get; set; }
    }
}
