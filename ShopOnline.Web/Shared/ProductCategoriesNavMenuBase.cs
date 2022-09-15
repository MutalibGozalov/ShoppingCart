using System.Reflection.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ShopOnline.Web.Services.Contracts;
using ShopOnline.Models.Dtos;


namespace ShopOnline.Web.Shared
{
    public class ProductCategoriesNavMenuBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable<ProductCategoryDto> ProductCategoryDtos { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ProductCategoryDtos = await ProductService.GetProductCategories();
            }
            catch (System.Exception ex)
            {
                
                ErrorMessage = ex.Message;
            }
        }
    }
}