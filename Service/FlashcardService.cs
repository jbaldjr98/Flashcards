using DataInterface;
using DomainInterface;
using Flashcards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class FlashcardService : GenericService<Flashcard>, IFlashcardService
    {
        private readonly IFlashcardRepository _flashcardRepository;

        public FlashcardService(IFlashcardRepository flashcardRepository) : base(flashcardRepository)
        {
            _flashcardRepository = flashcardRepository;
        }

        public async Task CreateFlashcards(List<Flashcard> newFlashcards)
        {
            await _flashcardRepository.AddRangeAsync(newFlashcards);
        }
    }
}
