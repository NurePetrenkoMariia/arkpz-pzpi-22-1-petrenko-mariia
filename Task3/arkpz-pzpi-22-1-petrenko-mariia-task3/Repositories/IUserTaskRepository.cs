using FarmKeeper.Models;

namespace FarmKeeper.Repositories
{
    public interface IUserTaskRepository
    {
        Task AddUserTasksAsync(List<UserTask> userTasks);
        Task<UserTask?> GetByIdAsync(Guid id);
        Task<List<UserTask>> GetAllAsync();
        Task<UserTask> CreateAsync(UserTask userTask);

    }
}
