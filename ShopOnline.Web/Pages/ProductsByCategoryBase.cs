using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ShopOnline.Web.Services.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Pages
{
    public class ProductsByCategoryBase : ComponentBase
    {
        [Parameter]
        public int CategoryId { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        public string CategoryName { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                Products = await ProductService.GetItemsByCategory(CategoryId);

                if(Products != null && Products.Count() > 0)
                {
                    var productDto = Products.FirstOrDefault(p => p.CategoryId == CategoryId);

                    if (productDto != null)
                        CategoryName = productDto.CategoryName;
                }

            }
            catch (System.Exception ex)
            {
              ErrorMessage = ex.Message;
            }
        }
    }
}