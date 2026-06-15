using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Model
{
    public class ApplicationDbContext : DbContext

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Chapter>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Chapter>()
                .HasOne(c => c.Subject)
                .WithMany(s => s.Chapters)
                .HasForeignKey(c => c.SubjectId);

            modelBuilder.Entity<Flashcard>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Flashcard>()
                .HasOne(f => f.Chapter)
                .WithMany(c => c.Flashcards)
                .HasForeignKey(f => f.ChapterId);

            modelBuilder.Entity<Flashcard>()
                .HasOne(f => f.Subject)
                .WithMany(s => s.Flashcards)
                .HasForeignKey(f => f.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
