
using System.Text;
using System.Net.Http.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using Newtonsoft.Json;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartServices : IShoppingCartService
    {
        private readonly HttpClient httpClient;
        public event Action<int> OnShoppingCartChanged;

        public ShoppingCartServices(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);
                
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent) 
                        return default(CartItemDto);   

                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message - {message}");
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");// wrongg

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>().ToList();
                    }

                    return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<CartItemDto> DeleteItem(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/ShoppingCart/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }

                return default(CartItemDto);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto); 
                var content = new StringContent(jsonRequest, Encoding.UTF8,"application/json-patch+json");

                var response = await httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                
                return null;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            if (OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }
    }
}