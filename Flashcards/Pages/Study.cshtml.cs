using DomainInterface;
using Flashcards.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class StudyModel : PageModel
    {
        private readonly IFlashcardService _flashcardService;
        private readonly ISubjectService _subjectService;
        private readonly IChapterService _chapterService;

        public StudyModel(IFlashcardService flashcardService, ISubjectService subjectService, IChapterService chapterService)
        {
            _flashcardService = flashcardService;
            _subjectService = subjectService;
            _chapterService = chapterService;
        }

        [BindProperty(SupportsGet = true)]
        public int SubjectId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ChapterId { get; set; }

        public List<Subject> Subjects { get; set; } = new();
        public List<Flashcard> Deck { get; set; } = new();

        public async Task OnGetAsync()
        {
            Subjects = await _subjectService.GetAllAsync();

            if (SubjectId > 0)
            {
                var all = await _flashcardService.GetAllAsync();
                Deck = all.Where(f => f.SubjectId == SubjectId)
                          .Where(f => ChapterId == 0 || f.ChapterId == ChapterId)
                          .ToList();
            }
        }

        public JsonResult OnGetChaptersBySubject(int subjectId)
        {
            var chapters = _chapterService.GetChaptersBySubjectId(subjectId);
            return new JsonResult(chapters.Select(c => new { c.Id, c.Name }));
        }

        public async Task<IActionResult> OnPostMarkAsync([FromBody] MarkRequest request)
        {
            var card = await _flashcardService.GetByIdAsync(request.Id);
            if (card == null) return new JsonResult(new { ok = false });

            if (request.Result == "success") card.numSuccess++;
            else if (request.Result == "failure") card.numFailure++;

            card.IsRevisit = request.IsRevisit;
            _flashcardService.UpdateAsync(card);

            return new JsonResult(new { ok = true });
        }
    }

    public class MarkRequest
    {
        public int Id { get; set; }
        public string? Result { get; set; }
        public bool IsRevisit { get; set; }
    }
}
