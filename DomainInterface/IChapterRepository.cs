using Flashcards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface
{
    public interface IChapterRepository : IGenericRepository<Chapter>
    {
        public Task<Chapter> GetChapterByNameAsync(string name);
        public IQueryable<Chapter> GetChaptersBySubject(int subjectId);

    }
}
