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
    public class ChapterService : GenericService<Chapter>, IChapterService
    {

        private readonly IChapterRepository _ChapterRepository;

        public ChapterService(IChapterRepository ChapterRepository) : base(ChapterRepository)
        {
            _ChapterRepository = ChapterRepository;
        }
    }
}
