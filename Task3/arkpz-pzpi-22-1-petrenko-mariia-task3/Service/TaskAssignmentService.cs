using FarmKeeper.Enums;
using FarmKeeper.Models;
using FarmKeeper.Repositories;

namespace FarmKeeper.Service
{
    public class TaskAssignmentService : ITaskAssignmentService
    {
        public List<UserTask> AssignTasks(List<User> workers, List<Assignment> assignments)
        {
            if (!assignments.Any())
            {
                throw new Exception("There are no tasks.");
            }

            if (!workers.Any())
            {
                throw new Exception("There are no workers.");
            }
            var sortedAssignments = assignments
                .Where(a => a.Status == Status.NotStarted) 
                .OrderByDescending(a => a.Priority) 
                .ToList();

            var assignedTasks = new List<UserTask>();
            foreach (var assignment in sortedAssignments) 
            {
                var chosenWorker = workers
                    .OrderBy(w => w.Assignments.Count)
                    .ThenBy(w => w.Assignments.Count(a => a.Priority == Priority.High))
                    .FirstOrDefault();
                if (chosenWorker != null)
                {
                    assignedTasks.Add(new UserTask
                    {
                        Id = Guid.NewGuid(),
                        UserId = chosenWorker.Id,
                        User = chosenWorker,
                        AssignmentId = assignment.Id,
                        Assignment = assignment
                    });

                    chosenWorker.Assignments.Add(assignment);
                }
            }
            return assignedTasks;
        }
    }
}
