using DataInterface;
using DomainInterface;
using Flashcards.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class CreateFlashcardsModel : PageModel
    {
        private readonly IFlashcardService _flashcardService;
        private readonly ISubjectService _subjectService;
        private readonly IChapterService _chapterService;
        private readonly ApplicationDbContext _context;
        public CreateFlashcardsModel(IFlashcardService flashcardService, ISubjectService subjectService, IChapterService chapterService, ApplicationDbContext context)
        {
            _flashcardService = flashcardService;
            _subjectService = subjectService;
            _chapterService = chapterService;
            _context = context;
        }
        [BindProperty]
        public List<Flashcard> NewFlashcards { get; set; } = new List<Flashcard>();
        [BindProperty]
        public int SubjectId { get; set; }
        [BindProperty]
        public int ChapterId { get; set; }
        public List<Subject> Subjects { get; set; } = new List<Subject>();

        public async Task OnGetAsync()
        {
            Subjects = await GetSubjectsForDropdown();
        }

        public JsonResult OnGetChaptersBySubject(int subjectId)
        {
            var chapters = GetChaptersBySubject(subjectId);
            return new JsonResult(chapters.Select(c => new { c.Id, c.Name }));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            foreach (var flashcard in NewFlashcards)
            {
                flashcard.SubjectId = SubjectId;
                flashcard.ChapterId = ChapterId;
            }

            await _flashcardService.CreateFlashcards(NewFlashcards);
            return RedirectToPage("/createFlashcards");
        }

        private async Task<List<Subject>> GetSubjectsForDropdown()
        {
            return await _subjectService.GetAllAsync();
        }

        private List<Chapter> GetChaptersBySubject(int subjectId)
        {
            return _chapterService.GetChaptersBySubjectId(subjectId);
        }
    }
}
