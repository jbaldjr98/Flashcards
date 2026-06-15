using Flashcards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainInterface
{
    public interface IChapterService : IGenericService<Chapter> 
    {
        public Task<Chapter> CreateNewChapter(Chapter NewChapter);

        public List<Chapter> GetChaptersBySubjectId(int SubjectId);
    }
}
