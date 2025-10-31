using Bazar.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Infrastracture.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context) => 
            (_context, _dbSet) = (context, context.Set<T>());

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

    }
}
