using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Model
{
    public class Subject
    {
        public int id {  get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Flashcard>  Flashcards { get; set; }
        public ICollection<Chapter> Chapters { get; set; }
    }
}
