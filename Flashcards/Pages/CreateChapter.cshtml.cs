using DomainInterface;
using Flashcards.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class CreateChapterModel : PageModel
    {

        private readonly IChapterService _chapterService;
        private readonly ISubjectService _subjectService;
        private readonly ApplicationDbContext _context;
        public CreateChapterModel(IChapterService chapterService, ISubjectService subjectService, ApplicationDbContext context)
        {
            _chapterService = chapterService;
            _subjectService = subjectService;
            _context = context;
        }
        [BindProperty]
        public Chapter NewChapter { get; set; } = new Chapter();

        public void OnGet(Subject subject)
        {
            NewChapter.SubjectId = subject.Id;
        }

        public async Task<IActionResult> OnPostAsync(Subject subject)
        {
            NewChapter.SubjectId = subject.Id;
            NewChapter.Subject = await _subjectService.GetByIdAsync(subject.Id);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var createdChapter =await _chapterService.CreateNewChapter(NewChapter);
            if(createdChapter is null)
            {
                ModelState.AddModelError("Name", "This name is already taken");
                return Page();
            }

            return RedirectToPage("/createFlashcards");


        }

    }
}
