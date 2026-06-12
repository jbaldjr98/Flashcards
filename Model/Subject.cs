using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Model
{
    [Table("Subjects")]
    public class Subject
    {
        public int id {  get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
