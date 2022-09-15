using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using ShopOnline.Api.Extensions;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await this._productRepository.GetItems();
                var productCategories = await this._productRepository.GetCategories();

                if (products == null || productCategories == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.JoinWith(productCategories);
                    return Ok(productDtos);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")] // :int isn't necessary
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await this._productRepository.GetItem(id);

                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    var productCategory = await _productRepository.GetCategory(product.CategoryId);
                    var productDto = product.JoinWith(productCategory);
                    return Ok(productDto);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await _productRepository.GetCategories();
                var productCategoryDtos = productCategories.JoinWith();

                return Ok(productCategoryDtos);
            }
            catch (System.Exception)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, 
                                "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")] // categoryId mecburi deyil, GetItemsByCategory(int categoryId) integer deyer parametri oldugu ucun API calling linkinin sonuna(product/GetItemsByCategory/) avtomatik id elave olunacaqdir. Amma integer parametrin adini burada qeyd etmekle API call linkinde GetItemsByCategorie-den evvel id-ni elave etmek mumkun olur.
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepository.GetItemsByCategory(categoryId);
                var productCategories = await _productRepository.GetCategories();

                var productDtos = products.JoinWith(productCategories);

                return Ok(productDtos);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                                "Error retrieving data from the database");
            }
        }
    }
}