using Flashcards.Model;

namespace DataInterface
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        public Task<Subject> GetSubjectByName(string name);

    }
}
