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
        {
            // Ranking
            modelBuilder.Entity<Ranking>().HasKey(r => r.StudentId);

            // Ocena → Student
            modelBuilder.Entity<Ocena>()
                .HasOne(o => o.Student)
                .WithMany(s => s.Oceny)
                .HasForeignKey(o => o.StudentId);

            // Ocena → Przedmiot
            modelBuilder.Entity<Ocena>()
                .HasOne(o => o.Przedmiot)
                .WithMany(p => p.Oceny)
                .HasForeignKey(o => o.PrzedmiotId);

            // Ocena → Nauczyciel
            modelBuilder.Entity<Ocena>()
                .HasOne(o => o.Nauczyciel)
                .WithMany(n => n.Oceny)
                .HasForeignKey(o => o.NauczycielId);
        }
    }
}

