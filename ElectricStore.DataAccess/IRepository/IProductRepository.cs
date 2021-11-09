using ElectricStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricStore.DataAccess.IRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        Task UpdateAsync(Product product);
        Task<int> CountAsync(string search = null, int categoryId = 0,int brandId=0, int maximum = 0, int minimum = 0, int sortBy = 0);
        Task<IEnumerable<Product>> SearchAsync(int pageNumber, int pageSize, string search = null,int brandId=0, int categoryId = 0, int maximum = 0, int minimum = 0, int sortBy = 0);
        Task<int> Maximum();
        Task<int> MiniMum();
    }
}
