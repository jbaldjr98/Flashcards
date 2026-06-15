using DomainInterface;
using Flashcards.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Flashcards.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ISubjectService _subjectService;

        public IndexModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public List<Subject> Subjects { get; set; } = new();

        public async Task OnGetAsync()
        {
            Subjects = await _subjectService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDeleteSubjectAsync(int subjectId)
        {
            var subject = await _subjectService.GetByIdAsync(subjectId);
            if (subject is not null)
                _subjectService.DeleteAsync(subject);
            return RedirectToPage();
        }
    }
}
