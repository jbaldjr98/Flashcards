using DataInterface;
using Flashcards.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ChapterRepository : GenericRepository<Chapter>, IChapterRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ChapterRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Chapter> GetChapterByNameAsync(string name)
        {
            var chapter = await _dbContext.Chapters.FirstOrDefaultAsync(c => c.Name == name);
            return chapter;
        }
    }
}
