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
        Task<IEnumerable<Category>> GetAllWithProductsAsync();
    }
}
