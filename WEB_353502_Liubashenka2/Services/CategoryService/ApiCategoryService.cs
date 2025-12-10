using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WEB_353502_Liubashenka2.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiCategoryService> _logger;

        public ApiCategoryService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        return await response.Content
                            .ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions)
                            ?? ResponseData<List<Category>>.Error("Не удалось десериализовать ответ");
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError($"-----> Ошибка: {ex.Message}");
                        return ResponseData<List<Category>>.Error($"Ошибка: {ex.Message}");
                    }
                }
                
                _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
                return ResponseData<List<Category>>.Error($"Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Ошибка при запросе к API: {ex.Message}");
                return ResponseData<List<Category>>.Error($"Ошибка при запросе к API: {ex.Message}");
            }
        }
    }
}

