using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Subject
    {
        public int id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Flashcard>  Flashcards { get; set; }
        public ICollection<Chapter> Chapters { get; set; }
    }
}
