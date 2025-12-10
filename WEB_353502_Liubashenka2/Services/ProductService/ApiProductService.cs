using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WEB_353502_Liubashenka2.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiProductService> _logger;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress?.AbsoluteUri}");
            
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}");
            }
            
            // добавить параметры в строку запроса
            var queryParams = new List<string>();
            if (pageNo > 1)
            {
                queryParams.Add($"pageNo={pageNo}");
            }
            
            // добавить размер страницы в строку запроса
            if (!string.IsNullOrEmpty(_pageSize) && !_pageSize.Equals("3"))
            {
                queryParams.Add($"pageSize={_pageSize}");
            }
            
            if (queryParams.Count > 0)
            {
                urlString.Append($"?{string.Join("&", queryParams)}");
            }

            // отправить запрос к API
            try
            {
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
                
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        return await response.Content
                            .ReadFromJsonAsync<ResponseData<ListModel<Dish>>>(_serializerOptions) 
                            ?? ResponseData<ListModel<Dish>>.Error("Не удалось десериализовать ответ");
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError($"-----> Ошибка: {ex.Message}");
                        return ResponseData<ListModel<Dish>>.Error($"Ошибка: {ex.Message}");
                    }
                }
                
                _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
                return ResponseData<ListModel<Dish>>.Error($"Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Ошибка при запросе к API: {ex.Message}");
                return ResponseData<ListModel<Dish>>.Error($"Ошибка при запросе к API: {ex.Message}");
            }
        }

        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content
                        .ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
                    return result ?? ResponseData<Dish>.Error("Не удалось десериализовать ответ");
                }
                
                _logger.LogError($"-----> Объект не найден. Error: {response.StatusCode.ToString()}");
                return ResponseData<Dish>.Error($"Объект не найден. Error: {response.StatusCode.ToString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Ошибка при запросе к API: {ex.Message}");
                return ResponseData<Dish>.Error($"Ошибка при запросе к API: {ex.Message}");
            }
        }

        public async Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"{_httpClient.BaseAddress}{id}")
                };
                
                request.Content = new StringContent(
                    JsonSerializer.Serialize(product, _serializerOptions),
                    Encoding.UTF8,
                    "application/json");
                
                var response = await _httpClient.SendAsync(request, CancellationToken.None);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content
                        .ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
                    return responseData ?? ResponseData<Dish>.Error("Не удалось десериализовать ответ");
                }
                
                _logger.LogError($"-----> Объект не обновлен. Error: {response.StatusCode.ToString()}");
                return ResponseData<Dish>.Error($"Объект не обновлен. Error: {response.StatusCode.ToString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Ошибка при запросе к API: {ex.Message}");
                return ResponseData<Dish>.Error($"Ошибка при запросе к API: {ex.Message}");
            }
        }

        public async Task<ResponseData<bool>> DeleteProductAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content
                        .ReadFromJsonAsync<ResponseData<bool>>(_serializerOptions);
                    return result ?? ResponseData<bool>.Error("Не удалось десериализовать ответ", false);
                }
                
                _logger.LogError($"-----> Объект не удален. Error: {response.StatusCode.ToString()}");
                return ResponseData<bool>.Error($"Объект не удален. Error: {response.StatusCode.ToString()}", false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Ошибка при запросе к API: {ex.Message}");
                return ResponseData<bool>.Error($"Ошибка при запросе к API: {ex.Message}", false);
            }
        }

        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
        {
            try
            {
                product.Image = "Images/noimage.jpg";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = _httpClient.BaseAddress
                };
                
                request.Content = new StringContent(
                    JsonSerializer.Serialize(product, _serializerOptions),
                    Encoding.UTF8,
                    "application/json");
                
                var response = await _httpClient.SendAsync(request, CancellationToken.None);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content
                        .ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
                    return responseData ?? ResponseData<Dish>.Error("Не удалось десериализовать ответ");
                }
                
                _logger.LogError($"-----> Объект не добавлен. Error: {response.StatusCode.ToString()}");
                return ResponseData<Dish>.Error($"Объект не добавлен. Error: {response.StatusCode.ToString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"-----> Ошибка при запросе к API: {ex.Message}");
                return ResponseData<Dish>.Error($"Ошибка при запросе к API: {ex.Message}");
            }
        }
    }
}

