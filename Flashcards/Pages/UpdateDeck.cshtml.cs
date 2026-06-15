using DomainInterface;
using Flashcards.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Web.Pages
{
    public class UpdateDeckModel : PageModel
    {
        private readonly IFlashcardService _flashcardService;
        private readonly IChapterService _chapterService;
        private readonly ApplicationDbContext _context;

        public UpdateDeckModel(IFlashcardService flashcardService, IChapterService chapterService, ApplicationDbContext context)
        {
            _flashcardService = flashcardService;
            _chapterService = chapterService;
            _context = context;
        }

        public Subject Subject { get; set; }
        public List<Chapter> Chapters { get; set; } = new();

        [BindProperty]
        public List<Flashcard> Flashcards { get; set; } = new();

        [BindProperty]
        public List<Flashcard> NewFlashcards { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int SubjectId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Subject = await _context.Subjects.FindAsync(SubjectId);
            if (Subject is null) return RedirectToPage("/Index");

            Chapters = _chapterService.GetChaptersBySubjectId(SubjectId);
            Flashcards = await _context.Flashcards
                .Where(f => f.SubjectId == SubjectId)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteFlashcardAsync(int flashcardId)
        {
            var flashcard = await _context.Flashcards.FindAsync(flashcardId);
            if (flashcard is not null)
            {
                _context.Flashcards.Remove(flashcard);
                await _context.SaveChangesAsync();
            }
            return new OkResult();
        }

        public async Task<IActionResult> OnPostAddChapterAsync(int subjectId, string chapterName, string? chapterDescription)
        {
            var chapter = new Chapter
            {
                Name = chapterName,
                Description = chapterDescription,
                SubjectId = subjectId
            };
            await _chapterService.CreateNewChapter(chapter);
            return RedirectToPage(new { subjectId });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Subject = await _context.Subjects.FindAsync(SubjectId);
            Chapters = _chapterService.GetChaptersBySubjectId(SubjectId);

            foreach (var submitted in Flashcards)
            {
                var existing = await _context.Flashcards.FindAsync(submitted.Id);
                if (existing is null) continue;
                existing.Front = submitted.Front;
                existing.Back = submitted.Back;
                existing.ChapterId = submitted.ChapterId;
            }

            var toAdd = NewFlashcards
                .Where(f => !string.IsNullOrWhiteSpace(f.Front) || !string.IsNullOrWhiteSpace(f.Back))
                .ToList();

            foreach (var flashcard in toAdd)
                flashcard.SubjectId = SubjectId;

            if (toAdd.Any())
                await _context.Flashcards.AddRangeAsync(toAdd);

            await _context.SaveChangesAsync();

            Subject = await _context.Subjects.FindAsync(SubjectId);
            Chapters = _chapterService.GetChaptersBySubjectId(SubjectId);
            Flashcards = await _context.Flashcards
                .Where(f => f.SubjectId == SubjectId)
                .ToListAsync();
            NewFlashcards = new List<Flashcard>();

            return Page();
        }
    }
}
