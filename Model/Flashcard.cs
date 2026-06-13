using System.ComponentModel.DataAnnotations.Schema;

namespace Flashcards.Model
{
    [Table("Flashcards")]
    public class Flashcard
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int ChapterId { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public double SuccessRate {  get; set; }
        public bool IsRevisit {  get; set; }

        public Subject Subject {  get; set; }
        public Chapter Chapter { get; set; }

    }
}
