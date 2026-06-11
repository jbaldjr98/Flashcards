using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInterface
{
    public interface IGenericRepository<T> where T : class
    {
            public Task<T> GetByIdAsync(int id);
            public Task<IEnumerable<T>> GetAllAsync();
            public Task AddAsync(T entity);
            public void UpdateAsync(T entity);
            public void DeleteAsync(T entity);
    }
}
