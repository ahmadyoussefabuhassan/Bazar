using Bazar.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazar.Application.Interfaces
{
    public interface IAdvertisementsService
    {
        Task<IEnumerable<AdvertisementsDto>> GetAllAdvertisementsAsync();
        Task<AdvertisementsDto> GetAdvertisementByIdAsync(int id);
        Task<AdvertisementsDto> CreateAdvertisementAsync(AdvertisementsDto createDto);
        Task<bool> UpdateAdvertisementAsync(int id, AdvertisementsDto updateDto);
        Task<bool> DeleteAdvertisementAsync(int id);
        Task<IEnumerable<AdvertisementsDto>> GetAdvertisementsByUserAsync(string username);
        Task<IEnumerable<AdvertisementsDto>> GetAdvertisementsByCategoryAsync(string categoryname);
        Task<IEnumerable<AdvertisementsDto>> SearchAdvertisementsAsync(string searchTerm);
        
        // Admin specific 
        Task<int> GetAdvertisementsCountAsync();
    }
}
