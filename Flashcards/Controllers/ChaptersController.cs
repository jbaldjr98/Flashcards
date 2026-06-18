using DomainInterface;
using Flashcards.Model;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChaptersController : ControllerBase
    {
        private readonly IChapterService _chapterService;

        public ChaptersController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        [HttpGet]
        public IActionResult GetBySubject([FromQuery] int subjectId) =>
            Ok(_chapterService.GetChaptersBySubjectId(subjectId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Chapter chapter)
        {
            var created = await _chapterService.CreateNewChapter(chapter);
            if (created is null)
                return Conflict(new { message = "A chapter with that name already exists." });
            return CreatedAtAction(nameof(GetBySubject), created);
        }
    }
}
