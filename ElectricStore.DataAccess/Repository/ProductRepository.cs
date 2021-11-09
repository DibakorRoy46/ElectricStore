using ElectricStore.Data;
using ElectricStore.DataAccess.IRepository;
using ElectricStore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricStore.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public async Task UpdateAsync(Product product)
        {
            Product productObj = await _db.Product.FirstOrDefaultAsync(x => x.Id == product.Id);
            if(productObj!=null)
            {
                productObj.Name = product.Name;
                productObj.Code = product.Code;
                productObj.Price = product.Price;
                productObj.DiscountPrice = product.DiscountPrice;
                productObj.ShortDes = product.ShortDes;
                productObj.Description = product.Description;
                productObj.Quantity = product.Quantity;
                productObj.CategoryId = product.CategoryId;
                productObj.BrandId = product.BrandId;
                productObj.ImageUrl = product.ImageUrl;
            }
        }
    }
}
