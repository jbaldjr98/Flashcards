using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
    [Table("Chapters")]
    public class Chapter
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SubjectId {  get; set; }
        public ICollection<Flashcard> Flashcards = new List<Flashcard>();
        [ValidateNever]
        public Subject Subject { get; set; }
    }
}
