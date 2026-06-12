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
            modelBuilder.Entity<Subject>().ToTable("Subject","dbo");
            modelBuilder.Entity<Flashcard>().ToTable("Flashcard","dbo");
            modelBuilder.Entity<Chapter>().ToTable("Chapter","dbo");

            modelBuilder.Entity<Subject>()
                .HasKey(s => s.id);

            modelBuilder.Entity<Chapter>()
                .HasKey(c => c.id);


            modelBuilder.Entity<Chapter>()
                .HasKey(c => c.subjectId)
                .HasName("FK_Chapter_ToSubject");
            
            modelBuilder.Entity<Flashcard>()
                .HasKey(f => f.id);

            modelBuilder.Entity<Flashcard>()
                .HasKey(f => f.chapterId)
                .HasName("FK_Flashcard_ToChapter");

            modelBuilder.Entity<Flashcard>()
                .HasKey(f => f.subjectId)
                .HasName("FK_Flashcard_ToSubject");
                
        }   
    }
}
