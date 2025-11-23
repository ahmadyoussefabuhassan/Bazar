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

        public async Task<IEnumerable<Category>> GetAllWithProductsAsync()
            => await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
    }
}
