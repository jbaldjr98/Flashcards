using DomainInterface;
using Flashcards.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class CreateSubjectModel : PageModel
    {
        private readonly ISubjectService _subjectService;
        private readonly ApplicationDbContext _context;
        public CreateSubjectModel(ISubjectService subjectService, ApplicationDbContext context)
        {
            _subjectService = subjectService;
            _context = context;
        }
        [BindProperty]
        public Subject NewSubject { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _subjectService.CreateNewSubject(NewSubject);
            
            return Page();


        }



    }
}
