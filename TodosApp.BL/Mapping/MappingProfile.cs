using AutoMapper;
using TodosApp.DAL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TodoReadDto,TodoItem>().ReverseMap();
        CreateMap<TodoAddDto, TodoItem>().ReverseMap();
        CreateMap<TodoUpdateDto, TodoItem>().ReverseMap();



    }
}
