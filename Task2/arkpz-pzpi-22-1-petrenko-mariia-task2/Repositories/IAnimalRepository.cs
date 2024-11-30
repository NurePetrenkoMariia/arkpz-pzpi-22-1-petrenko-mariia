using FarmKeeper.Models;

namespace FarmKeeper.Repositories
{
    public interface IAnimalRepository
    {
        Task<List<Animal>> GetAllAsync();
    }
}
