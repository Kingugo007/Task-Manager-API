using TaskManager.Core.DTOs;
using TaskManager.Domain.Models;

namespace TaskManager.Core.Interfaces
{
    public interface ITaskServices
    {
        Task<Tasks> AddTaskAsync(CreateTaskDTO data);
        Task<bool> DeleteAsync(string Id);
        Task<TaskListDTO> GetTaskByIdAsync(string Id);
        Task<IEnumerable<TaskListDTO>> GetTasks(int pageNumber, int pageSize);
    }
}