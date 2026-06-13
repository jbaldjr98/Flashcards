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

        private readonly IChapterRepository _chapterRepository;

        public ChapterService(IChapterRepository ChapterRepository) : base(ChapterRepository)
        {
            _chapterRepository = ChapterRepository;
        }

        public async Task<Chapter> CreateNewChapter(Chapter NewChapter)
        {
            var chapter = await _chapterRepository.GetChapterByNameAsync(NewChapter.Name);
            if (chapter == null)
            {
                chapter = await _chapterRepository.AddAsync(NewChapter);
            }
            return chapter;
        }
    }
}
