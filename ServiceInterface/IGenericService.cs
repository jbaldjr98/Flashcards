namespace DomainInterface
{
    public interface IGenericService<T> where T : class 
    {
        public Task<T> GetByIdAsync(int id);
        public Task<List<T>> GetAllAsync();
        public Task AddAsync(T entity);
        public Task AddRangeAsync(List<T> entities);
        public void UpdateAsync(T entity);
        public void DeleteAsync(T entity);
    }
}
