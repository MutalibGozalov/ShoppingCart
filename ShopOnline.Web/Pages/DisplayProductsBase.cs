using ShopOnline.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace ShopOnline.Web.Pages
{
    public class DisplayProductsBase : ComponentBase
    {
        [Parameter] // this makes dtoPerRowProducts parameter down here to be attached value from Products.razor parent page <DisplayProducts dtoPerRowProducts ="@dtoPerRowProducts"> </DisplayProducts>
        public IEnumerable<ProductDto> dtoPerRowProducts { get; set; }
    }
}