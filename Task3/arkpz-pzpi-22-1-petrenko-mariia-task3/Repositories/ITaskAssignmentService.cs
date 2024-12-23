using FarmKeeper.Models;

namespace FarmKeeper.Repositories
{
    public interface ITaskAssignmentService
    {
        List<UserTask> AssignTasks(List<User> workers, List<Assignment> assignments);
    }
}
