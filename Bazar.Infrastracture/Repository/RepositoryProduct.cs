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
        public async Task AddProductImageAsync(int productId, Images image)
        {
            image.ProductId = productId;
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
          var product = _context.Products
                .FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                 _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
            => await _context.Products.Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Advertisements)
                .ToListAsync();


        public async Task<IEnumerable<Product>> GetByAdvertisementAsync(int advertisementId)
            => await _context.Products
                .Where(p => p.AdvertisementsId == advertisementId)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Advertisements)
                .ToListAsync();


        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
            => await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Advertisements)
                .ToListAsync();


        public async Task<Images?> GetMainProductImageAsync(int productId)
            => await _context.Images
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.IsMain);


        public async Task<IEnumerable<Images>> GetProductImagesAsync(int productId)
            => await _context.Images
                .Where(i => i.ProductId == productId)
                .ToListAsync();


        public async Task<int> GetProductsCountAsync()
            => await _context.Products.CountAsync();

        public async Task<IEnumerable<Product>> GetRecentProductsAsync(int days)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Products
                .Include(p => p.Advertisements) 
                .Where(p => p.Advertisements.CreatedAt >= cutoffDate)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
            => await _context.Products
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Advertisements)
                .ToListAsync();

    }
}
