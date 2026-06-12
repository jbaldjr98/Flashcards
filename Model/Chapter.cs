using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Model
{
    public class Chapter
    {
        public int id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int subjectId {  get; set; }
        public ICollection<Flashcard> Flashcards = new List<Flashcard>();
        public Subject Subject { get; set; }
    }
}
