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
            var newEntity = table.AddAsync(entity).AsTask();
            _context.SaveChanges();
            return newEntity;

        }

        public void DeleteAsync(T entity)
        {
            table.Remove(entity);
            _context.SaveChanges();

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
            _context.SaveChanges();

        }
    }
}
