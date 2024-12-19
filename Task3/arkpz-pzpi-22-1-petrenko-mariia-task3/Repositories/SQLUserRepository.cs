using Data;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly FarmKeeperDbContext dbContext;
        public SQLUserRepository(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteAsync(Guid id)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (existingUser == null)
            {
                return null;
            }

            dbContext.Users.Remove(existingUser);
            await dbContext.SaveChangesAsync();
            return existingUser;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users.Include(f => f.Farms).ThenInclude(f => f.Stables).ThenInclude(s => s.Animals).Include(a => a.Assignments).Include(n => n.Notifications).ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await dbContext.Users.Include(f => f.Farms).ThenInclude(f => f.Stables).ThenInclude(s => s.Animals).Include(a => a.Assignments).Include(n => n.Notifications).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> UpdateAsync(Guid id, User user)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (existingUser == null)
            {
                return null;
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.Role = user.Role;
            existingUser.FarmId = user.FarmId;

            await dbContext.SaveChangesAsync();
            return existingUser;
        }
    }
}
