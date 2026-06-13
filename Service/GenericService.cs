using DataInterface;
using DomainInterface;

namespace Domain
{
    public class GenericService<T>: IGenericService<T>  where T : class 
    {
        protected readonly IGenericRepository<T> _repository;

        public GenericService( IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public Task AddAsync(T entity) => _repository.AddAsync(entity);

        public Task AddRangeAsync(List<T> entities) => _repository.AddRangeAsync(entities);

        public void DeleteAsync(T entity) => _repository.DeleteAsync(entity);

        public async Task<List<T>> GetAllAsync() => (await _repository.GetAllAsync()).ToList();

        public async Task<T> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public void UpdateAsync(T entity) => _repository.UpdateAsync(entity); 
    }
}
