namespace Model
{
    public class Flashcard
    {
        public int id { get; set; }
        public int subjectId { get; set; }
        public int chapterId { get; set; }
        public string front { get; set; }
        public string back { get; set; }
        public double successRate {  get; set; }
        public bool isRevisit {  get; set; }

        public Subject subject {  get; set; }
        public Chapter chapter { get; set; }

    }
}
