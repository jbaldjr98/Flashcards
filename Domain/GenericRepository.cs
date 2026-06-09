using DomainInterface;
using Flashcards.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DbSet<T> table = null;
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }
        public Task AddAsync(T entity)
        {
            return table.AddAsync(entity).AsTask();
        }

        public void DeleteAsync(T entity)
        {
            table.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public void UpdateAsync(T entity)
        {
            table.Update(entity);
        }
    }
}
