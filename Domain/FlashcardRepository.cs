using DataInterface;
using Flashcards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class FlashcardRepository : GenericRepository<Flashcard>, IFlashcardRepository
    {
        public FlashcardRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
