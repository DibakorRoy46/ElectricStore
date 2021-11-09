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
        public async Task<int> Maximum()
        {
            var maximumPrice = await _db.Product.MaxAsync(x => x.Price);
            return (int)(maximumPrice);
        }

        public async Task<int> MiniMum()
        {
            return (int)(await _db.Product.MinAsync(x => x.Price));
        }

        public async Task<int> CountAsync(string search = null, int categoryId = 0, int brandId = 0, int maximum = 0, int minimum = 0, int sortBy = 0)
        {
            var productList = await _db.Product.Include(x => x.Category).Include(x => x.Brand).ToListAsync();
            if (!String.IsNullOrEmpty(search))
            {
                productList = productList.Where(x => x.Name.Contains(search)).ToList();
                if (brandId != 0)
                {
                    productList = productList.Where(x => x.BrandId == brandId).ToList();
                    if (categoryId != 0)
                    {
                        productList = productList.Where(x => x.CategoryId == categoryId).ToList();
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (categoryId != 0)
                    {
                        productList = productList.Where(x => x.CategoryId == categoryId).ToList();
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (brandId != 0)
                {
                    productList = productList.Where(x => x.BrandId == brandId).ToList();
                    if (categoryId != 0)
                    {
                        productList = productList.Where(x => x.CategoryId == categoryId).ToList();
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (categoryId != 0)
                    {
                        productList = productList.Where(x => x.CategoryId == categoryId).ToList();
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return productList.Count();
        }

        public async Task<IEnumerable<Product>> SearchAsync(int pageNumber, int pageSize, string search = null, int brandId = 0, int categoryId = 0, int maximum = 0, int minimum = 0, int sortBy = 0)
        {
            var productList = await _db.Product.Include(x => x.Category).Include(x => x.Brand).ToListAsync();
            if(!String.IsNullOrEmpty(search))
            {
                productList = productList.Where(x => x.Name.Contains(search)).ToList();
                if(brandId!=0)
                {
                    productList = productList.Where(x => x.BrandId == brandId).ToList();
                    if(categoryId!=0)
                    {
                        productList = productList.Where(x => x.CategoryId == categoryId).ToList();
                        if(maximum!=0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if(minimum!=0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if(sortBy!=0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                       case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (categoryId != 0)
                    {
                        productList = productList.Where(x => x.CategoryId == categoryId).ToList();
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (brandId != 0)
                {
                    productList = productList.Where(x => x.BrandId == brandId).ToList();
                    if (categoryId != 0)
                    {
                        productList = productList.Where(x => x.CategoryId == categoryId).ToList();
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (categoryId != 0)
                    {
                        productList = productList.Where(x => x.CategoryId == categoryId).ToList();
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (maximum != 0)
                        {
                            productList = productList.Where(x => x.Price < maximum).ToList();
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (minimum != 0)
                            {
                                productList = productList.Where(x => x.Price > minimum).ToList();
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (sortBy != 0)
                                {
                                    switch (sortBy)
                                    {
                                        case 1:
                                            productList = productList.OrderByDescending(x => x.Id).ToList();
                                            break;
                                        case 2:
                                            productList = productList.OrderBy(x => x.Id).ToList();
                                            break;
                                        case 3:
                                            productList = productList.OrderBy(x => x.Price).ToList();
                                            break;
                                        default:
                                            productList = productList.OrderByDescending(x => x.Price).ToList();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return productList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();         
        }

       
    }
}
