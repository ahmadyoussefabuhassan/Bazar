using Bazar.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Domain.Interfaces
{
    public interface IRepositoryCategory : IRepository<Category>
    {
      
        Task<Category> CreateCategoryAsync(string categoryName);
        Task<bool> CategoryExistsAsync(string categoryName);
        Task<bool> DeleteCategoryAsync(int categoryId);
        Task<Category?> GetCategoryByNameAsync(string name);
    }
}
