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
                .HasKey(s => s.id);

            modelBuilder.Entity<Chapter>()
                .HasKey(c => c.id);

            modelBuilder.Entity<Chapter>()
                .HasOne(c => c.Subject)
                .WithMany(s => s.Chapters)
                .HasForeignKey(c => c.subjectId);

            modelBuilder.Entity<Flashcard>()
                .HasKey(f => f.id);

            modelBuilder.Entity<Flashcard>()
                .HasOne(f => f.chapter)
                .WithMany(c => c.Flashcards)
                .HasForeignKey(f => f.chapterId);

            modelBuilder.Entity<Flashcard>()
                .HasOne(f => f.subject)
                .WithMany(s => s.Flashcards)
                .HasForeignKey(f => f.subjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
