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
    public class RepositoryProduct : Repository<Product>, IRepositoryProduct
    {
        private readonly ApplicationDbContext _context;
        public RepositoryProduct(ApplicationDbContext context) : base(context)
            => _context = context;

        public async Task<IEnumerable<Product>> GetProductsWithFilterAsync(string? search, string? category, int? minPrice, int? maxPrice)
        {
            var query = _context.Products
                .Include(p => p.Images) 
                .Include(p => p.Category)
                .Include(p => p.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            // فلترة حسب الفئة
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category != null && p.Category.Name == category);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            query = query.OrderByDescending(p => p.Id);

            return await query.ToListAsync();
        }

        public async Task<Product?> GetProductWithDetailsAsync(int id)
             => await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

    }
}
