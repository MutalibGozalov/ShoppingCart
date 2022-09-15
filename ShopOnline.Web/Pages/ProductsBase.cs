using Microsoft.AspNetCore.Components;
using ShopOnline.Web.Services.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Pages  // This class used to get data from Api
{
    public class ProductBase : ComponentBase // .razor-un ProductClass-i inherit etmesi ucun ProductBase pzu ComponentBase-i inherit etmelidir
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public IEnumerable<ProductDto> dtoBaseProducts { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                 dtoBaseProducts = await ProductService.GetItems();
                 var shoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                 var totalQty = shoppingCartItems.Sum(i => i.Qty);

                 ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        protected IOrderedEnumerable<IGrouping<string, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in dtoBaseProducts
                   group product by product.CategoryName into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup; // IGrouping() 2 olculu listdir. Daxilinde muxtelif propertisine gore qruplandirilmis objectler list-i var. Her Group-un olcusu ferqli ola biler, her bir Group-a da Key value-su ile access etmek olar. Burada Key olaraq categoryId propertisi islenilib ve 4 eded categoryId movcuddur.Yeni 4 group
        }

        protected string GetCtegoryName(IGrouping<string, ProductDto> productGroup)
        {
            //return productGroup.FirstOrDefault(pg => pg.CategoryId == productGroup.Key).CategoryName;
            return productGroup.Key;
        }
    }
}