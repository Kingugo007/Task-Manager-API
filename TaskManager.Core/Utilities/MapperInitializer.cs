using AutoMapper;
using TaskManager.Core.DTOs;
using TaskManager.Domain.Models;

namespace TaskManager.Core.Utilities
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<TaskListDTO, Tasks>().ReverseMap()
                 .ForMember(
                    dest => dest.DueDate,
                    opt => opt.MapFrom(src => src.StartDate.AddDays(src.AllottedTimeInDays))
                ).ForMember(
                    dest => dest.EndDate,
                    opt => opt.MapFrom(src => src.StartDate.AddDays(src.ElapsedTimeInDays))
                ).ForMember(
                    dest => dest.DaysOverDue,
                    opt => opt.MapFrom(src => !src.TaskStatus ? src.ElapsedTimeInDays - src.AllottedTimeInDays : 0)
                ).ForMember(
                    dest => dest.DaysLate,
                    opt => opt.MapFrom(src => src.TaskStatus ? src.AllottedTimeInDays - src.ElapsedTimeInDays : 0)
                );
            CreateMap<Tasks, CreateTaskDTO>().ReverseMap();
        }
    }
}
