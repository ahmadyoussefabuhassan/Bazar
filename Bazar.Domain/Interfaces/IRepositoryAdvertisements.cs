using Bazar.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Domain.Interfaces
{
    public interface IRepositoryAdvertisements : IRepository<Advertisements>
    {
        Task<IEnumerable<Advertisements>> GetByUserAsync(int userId);
        Task<IEnumerable<Advertisements>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Advertisements>> GetAllWithDetailsAsync();
        Task<bool> DeleteAdvertisementAsync(int advertisementId);
        Task<int> GetAdvertisementsCountAsync();
        Task<IEnumerable<Advertisements>> GetRecentAdvertisementsAsync(int days);
        Task<int> GetCountByUserIdAsync(int id);
    }
}
