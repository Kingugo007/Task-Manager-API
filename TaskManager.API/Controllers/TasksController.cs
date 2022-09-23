using Microsoft.AspNetCore.Mvc;
using Serilog;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskServices _taskServices;
        public TasksController(ITaskServices taskServices)
        {
            _taskServices = taskServices;
        }
       /// <summary>
       /// This get request all tasks depending on the page number and page size
       /// </summary>
       /// <param name="pageNumber"></param>
       /// <param name="pageSize"></param>
       /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> GetAllTasks(int pageNumber = 1, int pageSize = 10)
        {
            Log.Information($"getting all task");
            //  var result = await _taskServices.GetAllTasksAsync();
            var result = await _taskServices.GetTasks(pageNumber, pageSize);
            return Ok(result);
        }
        /// <summary>
        /// this http request post to data to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTasks([FromBody] CreateTaskDTO model)
        {
            if (!ModelState.IsValid)
            {
                Log.Error($"Error occuried posting to database");
                return BadRequest(ModelState);
            }

            var res = await _taskServices.AddTaskAsync(model);
            Log.Information($"task created");
            return Ok(res);
        }
        /// <summary>
        /// this request get a single task by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> GetTaskById(string id)
        {
            var results = await _taskServices.GetTaskByIdAsync(id);
            return Ok(results);
        }
        /// <summary>
        /// this request deletes a task using it ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var results = await _taskServices.DeleteAsync(id);
            if (results)
            {
                Log.Information($"Task with {id} is deleted");
                return NoContent();
            }
            return BadRequest();           
        }
        
    }
}
