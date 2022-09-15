using System.Linq;
using ShopOnline.Web.Services.Contracts;
using ShopOnline.Models.Dtos;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductServices : IProductService
    {
        private readonly HttpClient _httpClient;
        public ProductServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            // try
            // {
            //     var dtoProducts = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product");
            //     return dtoProducts;
            // }
            // catch (System.Exception)
            // {
                
            //     throw;
            // }

            try
            {
                var response = await _httpClient.GetAsync("api/Product");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }
        public async Task<ProductDto> GetItem(int id)
        {
           try
           {
             var response = await _httpClient.GetAsync($"api/Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(ProductDto);
                }

                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
           }
           catch (System.Exception)
           {
            
            throw;
           }
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Product/GetProductCategories");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return Enumerable.Empty<ProductCategoryDto>();
                    
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status Code - {response.StatusCode} Message = {message}");
                }
            }
            catch (System.Exception)
            {
                // Log exception
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/{categoryId}/GetItemsByCategory");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        return Enumerable.Empty<ProductDto>();
                    
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status Code - {response.StatusCode} Message = {message}");
                }
            }
            catch (System.Exception)
            {
                // Log exception
                throw;
            }
        }
    }
}