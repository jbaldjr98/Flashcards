using DomainInterface;
using Flashcards.Model;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _subjectService.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Subject subject)
        {
            var created = await _subjectService.CreateNewSubject(subject);
            if (created is null)
                return Conflict(new { message = "A subject with that name already exists." });
            return CreatedAtAction(nameof(GetAll), created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject is null) return NotFound();
            _subjectService.DeleteAsync(subject);
            return NoContent();
        }
    }
}
