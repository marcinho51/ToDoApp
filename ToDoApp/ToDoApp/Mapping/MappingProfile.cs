using AutoMapper;
using ToDoApp.Dtos;
using ToDoApp.Models;

namespace ToDoApp.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ToDo, ToDoDto>();
        CreateMap<ToDoCreateDto, ToDo>();
        CreateMap<ToDoUpdateDto, ToDo>();
    }
}
