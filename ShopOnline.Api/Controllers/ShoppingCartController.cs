using System.Data;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Models.Dtos;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Api.Repositories;
using ShopOnline.Api.Extensions;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
            
        }

        [HttpGet]
        [Route("{userId}/GetItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
        {
            try
            {
                var cartItems = await _shoppingCartRepository.GetItems(userId);
                
                if(cartItems ==null)
                    return NoContent();

                var products = await _productRepository.GetItems();

                if (products == null)
                {
                    throw new Exception("No products exists in the system");
                }

                var cartItemsDto = cartItems.JoinWith(products);
                return Ok(cartItemsDto);
                    
            }
            catch (System.Exception ex)
            {
              return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")] // :int isn't necessary
        public async Task<ActionResult<CartItemDto>> GetItem(int id)
        {
            try
            {
                var cartItem = await _shoppingCartRepository.GetItem(id);

                if(cartItem == null)
                    return NotFound();
                
                var product = await _productRepository.GetItem(cartItem.ProductId);

                if(product == null)
                    return NotFound();
                
                var cartItemDto = cartItem.JoinWith(product);
                return Ok(cartItemDto);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var newCartItem = await _shoppingCartRepository.AddItem(cartItemToAddDto);

                if(newCartItem == null)
                    return NoContent();

                var product = await _productRepository.GetItem(newCartItem.ProductId);

                if(product == null)
                    throw new Exception($"Something went wrong when attempting to retrieve product (productId:({cartItemToAddDto.ProductId})");
                
                var newCartItemDto = newCartItem.JoinWith(product);

                return CreatedAtAction(nameof(GetItem), new {id = newCartItemDto.Id}, newCartItemDto);
            }
            catch (System.Exception ex)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
        {
            try
            {
                var cartItem = await _shoppingCartRepository.DeleteItem(id);

                if (cartItem == null)
                    return NotFound();
                
                var product = await _productRepository.GetItem(id);

                return Ok(cartItem.JoinWith(product));
            }
            catch (System.Exception ex)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDto>> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var cartItem = await _shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);
                if (cartItem == null)
                    return NotFound();
                
                var product = await _productRepository.GetItem(cartItem.ProductId);
                var cartItemDto = cartItem.JoinWith(product);
                return Ok(cartItemDto);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
} 