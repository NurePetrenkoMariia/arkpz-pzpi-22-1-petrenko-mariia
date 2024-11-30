using FarmKeeper.Data;
using FarmKeeper.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Repositories
{
    public class SQLAnimalRepository : IAnimalRepository
    {
        private readonly FarmKeeperDbContext dbContext;
        public SQLAnimalRepository(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Animal>> GetAllAsync()
        {
            return await dbContext.Animals.ToListAsync();
        }
    }
}
