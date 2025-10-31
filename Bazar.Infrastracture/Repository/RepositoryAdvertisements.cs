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
    public class RepositoryAdvertisements : Repository<Advertisements>, IRepositoryAdvertisements
    {
        private readonly ApplicationDbContext _context;
        public RepositoryAdvertisements(ApplicationDbContext context) : base(context)
            => _context = context;


        public async Task<bool> DeleteAdvertisementAsync(int advertisementId)
        {
            var advertisement = _context.Advertisements.Include(a => a.Products)
                .ThenInclude(p => p.Images)
                .FirstOrDefault(a => a.Id == advertisementId);
            if (advertisement != null)
            {
                _context.Advertisements.Remove(advertisement);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> GetAdvertisementsCountAsync()
            => await _context.Advertisements.CountAsync();

        public async Task<IEnumerable<Advertisements>> GetAllWithDetailsAsync()
            => await _context.Advertisements
                .Include(a => a.Products)
                .ThenInclude(p => p.Images)
                .Include(a => a.User)
                .Include(a => a.Category)
                .ToListAsync();


        public async Task<IEnumerable<Advertisements>> GetByCategoryAsync(int categoryId)
            => await _context.Advertisements.Where(a => a.CategoryId == categoryId)
                .Include(a => a.Products)
                .ThenInclude(p => p.Images)
                .Include(a => a.User)
                .ToListAsync();

        public async Task<IEnumerable<Advertisements>> GetByUserAsync(int userId)
            => await _context.Advertisements.Where(a => a.UserId == userId)
                .Include(a => a.Products)
                .ThenInclude(p => p.Images)
                .Include(a => a.Category)
                .ToListAsync();

        public async Task<int> GetCountByUserIdAsync(int id)
            => await _context.Advertisements
                .CountAsync(a => a.UserId == id);


        public async Task<IEnumerable<Advertisements>> GetRecentAdvertisementsAsync(int days)
        {
            var startDate = DateTime.UtcNow.AddDays(-days);

            return await _context.Advertisements
                .Where(a => a.CreatedAt >= startDate)
                .Include(a => a.Products)
                .Include(a => a.User)
                .Include(a => a.Category)
                .ToListAsync();
        }
    }
}
