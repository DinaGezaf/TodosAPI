
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shipping.Services.Dtos;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

public class ExternalTodoService:IExternalTodoService
{
    private readonly HttpClient _httpClient;

    public ExternalTodoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/todos");
    }

    public async Task<IEnumerable<TodoReadDto>> GetExternalTodosAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<TodoReadDto>>("todos");
        return response;
    }

    [HttpGet("paginated")]
    public async Task<PaginationResponse<TodoReadDto>> GetTodosPaginatedAsync([FromQuery] PaginationParameters paginationParameters)
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<TodoReadDto>>("todos");

        int totalRecords = response.Count();

        List<TodoReadDto> paginatedTodos = response
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToList();

        PaginationResponse<TodoReadDto> result = new PaginationResponse<TodoReadDto>
        {
            Data = paginatedTodos,
            PageNo = paginationParameters.PageNumber,
            PageSize = paginationParameters.PageSize,
            TotalRecords = totalRecords
        };

        return result;
    }

}
