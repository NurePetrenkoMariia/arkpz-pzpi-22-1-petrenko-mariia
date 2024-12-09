using Models;

namespace Repositories
{
    public interface IFarmRepository
    {
        Task<List<Farm>> GetAllAsync();
        Task<Farm?> GetByIdAsync(Guid id);
        Task<Farm> CreateAsync(Farm farm);
        Task<Farm?> UpdateAsync(Guid id, Farm farm);
        Task<Farm?> DeleteAsync(Guid id);
    }
}
