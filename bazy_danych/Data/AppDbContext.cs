using Microsoft.EntityFrameworkCore;
using bazy_danych.Models;

namespace bazy_danych.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Studenci { get; set; }
        public DbSet<Nauczyciel> Nauczyciele { get; set; }
        public DbSet<Przedmiot> Przedmioty { get; set; }
        public DbSet<Ocena> Oceny { get; set; }
        public DbSet<Ranking> Rankingi { get; set; }
        public DbSet<HistoriaOcen> HistoriaOcen { get; set; }

        public DbSet<SredniaOcena> SrednieOceny { get; set; } // opcjonalnie


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {// Wymuszenie nazw tabel dokładnie jak w Oracle
            modelBuilder.Entity<Student>().ToTable("STUDENT");
            modelBuilder.Entity<Nauczyciel>().ToTable("NAUCZYCIEL");
            modelBuilder.Entity<Przedmiot>().ToTable("PRZEDMIOT");
            modelBuilder.Entity<Ocena>().ToTable("OCENA");
            modelBuilder.Entity<Ranking>().ToTable("RANKING");
            modelBuilder.Entity<HistoriaOcen>().ToTable("HISTORIA_OCEN");
            modelBuilder.Entity<SredniaOcena>().HasNoKey().ToView("WIDOK_SREDNIA_OCENA");

            // 🔗 Relacja: Ocena → Student (wiele ocen do jednego studenta)
            modelBuilder.Entity<Ocena>()
                .HasOne(o => o.Student)
                .WithMany(s => s.Oceny)
                .HasForeignKey(o => o.StudentId);

            // 🔗 Relacja: Ocena → Przedmiot
            modelBuilder.Entity<Ocena>()
                .HasOne(o => o.Przedmiot)
                .WithMany(p => p.Oceny)
                .HasForeignKey(o => o.PrzedmiotId);


            // 🔗 Relacja: Ocena → Nauczyciel
            modelBuilder.Entity<Ocena>()
                .HasOne(o => o.Nauczyciel)
                .WithMany(n => n.Oceny)
                .HasForeignKey(o => o.NauczycielId);


            // 🔗 Relacja: HistoriaOcen → Ocena
            modelBuilder.Entity<HistoriaOcen>()
                .HasOne<Ocena>() // jeśli nie masz właściwości nawigacyjnej
                .WithMany()
                .HasForeignKey(h => h.OcenaId);

            // 🔗 Ranking → Student (jeden do jednego)
            modelBuilder.Entity<Ranking>()
                .HasOne<Student>()
                .WithOne()
                .HasForeignKey<Ranking>(r => r.StudentId);

            // 🔷 SredniaOcena — widok bez klucza
            modelBuilder.Entity<SredniaOcena>().HasNoKey().ToView("WIDOK_SREDNIA_OCENA");

            // ✏️ Dodatkowo — jeśli masz liczby zmiennoprzecinkowe (decimal) → ustaw precyzję:
            modelBuilder.Entity<Ocena>().Property(o => o.OcenaWartosc).HasPrecision(3, 1);
            modelBuilder.Entity<HistoriaOcen>().Property(h => h.OcenaStara).HasPrecision(3, 1);
            modelBuilder.Entity<HistoriaOcen>().Property(h => h.OcenaNowa).HasPrecision(3, 1);
            modelBuilder.Entity<Ranking>().Property(r => r.SredniaOcen).HasPrecision(5, 2);
            modelBuilder.Entity<SredniaOcena>().Property(s => s.Srednia_Ocen).HasPrecision(5, 2);
        }
    }
}

