using System.Data.Common;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ShopOnline.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _context;
        public ProductRepository(ShopOnlineDbContext context)
        {
            _context = context;    
        }
        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await this._context.Products.ToListAsync();
            return products;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await this._context.ProductCategories.ToListAsync();
            return categories;
        }
        public async Task<Product> GetItem(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(c => c.Id == id);
            return product;
        }
        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await _context.ProductCategories.FindAsync(id);
            return category;
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            var products = await (from product in _context.Products
                                  where product.CategoryId == id
                                  select product).ToListAsync();
            return products;
        }

    }
}