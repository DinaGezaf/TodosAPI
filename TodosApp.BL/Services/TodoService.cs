using AutoMapper;
using System.ComponentModel.DataAnnotations;
using TodosApp.DAL;

public class TodoService: ITodoService
{
    private readonly ITodoRepository _todoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TodoService(ITodoRepository todoRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _todoRepository = todoRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TodoReadDto>> GetAllTodosAsync()
    {
        var todos = await _todoRepository.GetAllTodosAsync();

        List<TodoReadDto> TodosResponse = new List<TodoReadDto>();
        foreach (TodoItem todoItem in todos)
        {
            TodosResponse.Add(_mapper.Map<TodoReadDto>(todoItem));
        }
        return TodosResponse;
    }

    public async Task<TodoReadDto> GetTodoByIdAsync(int id, string userId)
    {
        var todo = await _todoRepository.GetTodoByIdAsync(id);
        if (todo == null || todo.User.Id != userId)
        {
            return null;
        }
        return _mapper.Map<TodoReadDto>(todo);
    }

    public async Task<List<ValidationResult>?> CreateTodoAsync(TodoAddDto todoAddDto, string userId)
    {
        var user = await _todoRepository.GetUserByIdAsync(userId);
        var todo = _mapper.Map<TodoItem>(todoAddDto);
        todo.User = user;

        List<ValidationResult>? validationResults = ValidateModel.ModelValidation(todo);
        if (validationResults?.Count == 0)
        {
            await _todoRepository.AddTodoAsync(todo);
            _unitOfWork.BeginTransaction();

            try
            {
                await _unitOfWork.SaveAsync();
                _unitOfWork.Commit(); 
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        return validationResults;
    }

    public async Task<List<ValidationResult>?> UpdateTodoAsync(int id, TodoUpdateDto todoUpdateDto, string userId)
    {
        var user = await _todoRepository.GetUserByIdAsync(userId);
        var existingTodo = await _todoRepository.GetTodoByIdAsync(id);

        
        if (existingTodo == null || user == null || existingTodo.User.Id != userId)
            return null;

        _mapper.Map(todoUpdateDto, existingTodo);
        List<ValidationResult>? validationResults = ValidateModel.ModelValidation(existingTodo);

        if (validationResults?.Count == 0)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                await _unitOfWork.SaveAsync(); 
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        return validationResults;
    }

    public async Task<bool> DeleteTodoAsync(int id, string userId)
    {
        var user = await _todoRepository.GetUserByIdAsync(userId);
        var todo = await _todoRepository.GetTodoByIdAsync(id);
        if (todo != null && user == null && todo.User.Id == userId)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                await _todoRepository.DeleteTodoAsync(id);
                await _unitOfWork.SaveAsync();
                _unitOfWork.Commit(); 
                return true;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback(); 
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        return false;
    }

    public async Task<IEnumerable<TodoReadDto>> GetTodosByUserIdAsync(string userId)
    {
        var todos = await _todoRepository.GetTodosByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<TodoReadDto>>(todos);
    }

   

}
