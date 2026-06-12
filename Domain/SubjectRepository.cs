using DataInterface;
using Flashcards.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SubjectRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<Subject> GetSubjectByName(string name)
        {
            return await _dbContext.Subjects.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
