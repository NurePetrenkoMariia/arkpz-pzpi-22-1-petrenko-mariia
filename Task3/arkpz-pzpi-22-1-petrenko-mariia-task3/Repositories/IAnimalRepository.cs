using Models;

namespace Repositories
{
    public interface IAnimalRepository
    {
        Task<List<Animal>> GetAllAsync();
        Task<Animal?> GetByIdAsync(Guid id);
        Task<Animal> CreateAsync(Animal animal);
        Task<Animal?> UpdateAsync(Guid id, Animal animal);
        Task<Animal?> DeleteAsync(Guid id);
    }
}
