using DomainInterface;
using Flashcards.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashcardsController : ControllerBase
    {
        private readonly IFlashcardService _flashcardService;
        private readonly ApplicationDbContext _context;

        public FlashcardsController(IFlashcardService flashcardService, ApplicationDbContext context)
        {
            _flashcardService = flashcardService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBySubject([FromQuery] int subjectId)
        {
            var cards = await _context.Flashcards
                .Where(f => f.SubjectId == subjectId)
                .ToListAsync();
            return Ok(cards);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> CreateBatch([FromBody] List<Flashcard> flashcards)
        {
            await _flashcardService.CreateFlashcards(flashcards);
            return Ok(flashcards);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Flashcard updated)
        {
            var existing = await _context.Flashcards.FindAsync(id);
            if (existing is null) return NotFound();
            existing.Front = updated.Front;
            existing.Back = updated.Back;
            existing.ChapterId = updated.ChapterId;
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var card = await _context.Flashcards.FindAsync(id);
            if (card is null) return NotFound();
            _context.Flashcards.Remove(card);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/mark")]
        public async Task<IActionResult> Mark(int id, [FromBody] MarkDto dto)
        {
            var card = await _context.Flashcards.FindAsync(id);
            if (card is null) return NotFound();
            if (dto.Result == "success") card.numSuccess++;
            else if (dto.Result == "failure") card.numFailure++;
            card.IsRevisit = dto.IsRevisit;
            await _context.SaveChangesAsync();
            return Ok(card);
        }
    }

    public record MarkDto(string? Result, bool IsRevisit);
}
