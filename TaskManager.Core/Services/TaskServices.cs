using AutoMapper;
using Microsoft.Extensions.Configuration;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Utilities;
using TaskManager.Domain.Models;

namespace TaskManager.Core.Services
{
    public class TaskServices : ITaskServices
    {
        private readonly IHttpCommandHandlers _httpClientHandler;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config; 
        public string baseUrl { get; set; }
        public TaskServices(IHttpCommandHandlers httpClientHandler, IMapper mapper,
            IConfiguration config)
        {
            _httpClientHandler = httpClientHandler;
            _mapper = mapper;
            _config = config;
            baseUrl =  _config["ConnectionStrings:baseUrl"];
        }    
        /// <summary>
        /// This method gets the number of specific task you 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskListDTO>> GetTasks(int pageNumber, int pageSize)
        {

            var tasks = await _httpClientHandler.GetRequest<List<Tasks>>(baseUrl);
            var result = new List<TaskListDTO>();
            foreach (var task in tasks)
            {
                result.Add(_mapper.Map<TaskListDTO>(task));
            }
            var paginatedResult = Pagination.PaginationAsync(result, pageSize, pageNumber);
            return paginatedResult.PageItems;
        }
        /// <summary>
        /// This is an async method that adds data to the database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Tasks> AddTaskAsync(CreateTaskDTO data)
        {
            var model = _mapper.Map<Tasks>(data);
            model.StartDate = DateTime.Now;
            var res = await _httpClientHandler.PostRequest<Tasks, Tasks>(model, baseUrl);
            return res;
        }
        /// <summary>
        /// This is an async method that gets a single task using it's ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<TaskListDTO> GetTaskByIdAsync(string Id)
        {
            string url = $"{baseUrl}/{Id}";
            var task = await _httpClientHandler.GetRequest<Tasks>(url);
             var result = _mapper.Map<TaskListDTO>(task);          
            return result;
        }
        /// <summary>
        /// This is an async method that delete's a task
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string Id)
        {
            string url = $"{baseUrl}/{Id}";
            var res = await _httpClientHandler.DeleteRequest<Tasks>(url);
            if(res is not null)
            {
                return true;
            }
            return false;
        }

    }
}
