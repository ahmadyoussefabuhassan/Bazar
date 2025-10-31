using Bazar.Domain.Entites;
using Bazar.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Infrastracture.Repository
{
    public class RepositoryCategory : Repository<Category>, IRepositoryCategory
    {
        private readonly ApplicationDbContext _context;
        public RepositoryCategory(ApplicationDbContext context) : base(context)
            => _context = context;


        public async Task<bool> CategoryExistsAsync(string categoryName)
            => await _context.Categories
        .AnyAsync(c => c.Name.ToLower() == categoryName.ToLower());


        public async Task<Category> CreateCategoryAsync(string categoryName)
        {
           var category = new Category { Name = categoryName };
           await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                 _context.Categories.Remove(category);
                 await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        => await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }
}
