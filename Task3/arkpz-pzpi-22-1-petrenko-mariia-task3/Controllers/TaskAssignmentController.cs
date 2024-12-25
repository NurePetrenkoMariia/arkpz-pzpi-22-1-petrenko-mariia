using FarmKeeper.Models;
using FarmKeeper.Repositories;
using FarmKeeper.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmKeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAssignmentController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IAssignmentRepository assignmentRepository;
        private readonly ITaskAssignmentService taskAssignmentService;
        private readonly IUserTaskRepository userTaskRepository;
        public TaskAssignmentController(
            IUserRepository userRepository, 
            IAssignmentRepository assignmentRepository, 
            ITaskAssignmentService taskAssignmentService,
            IUserTaskRepository userTaskRepository)
        {
            this.userRepository = userRepository;
            this.assignmentRepository = assignmentRepository;
            this.taskAssignmentService = taskAssignmentService;
            this.userTaskRepository = userTaskRepository;
        }

        [HttpPost("assign-tasks")]
        [Authorize(Roles = "Owner,DatabaseAdmin")]
        public async Task<IActionResult> AssignTasks()
        {
            var workers = await userRepository.GetWorkersForAssignmentsAsync();
            var tasks = await assignmentRepository.GetNotStartedAssignmentsAsync();

            var assignedTasks = await taskAssignmentService.AssignTasks(workers, tasks);

            await userTaskRepository.AddUserTasksAsync(assignedTasks);

            return Ok(assignedTasks);
        }
    }
}
