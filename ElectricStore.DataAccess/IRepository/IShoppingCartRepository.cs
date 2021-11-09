using ElectricStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricStore.DataAccess.IRepository
{
    public interface IShoppingCartRepository:IRepository<ShoppingCart>
    { 
        Task UpdateAsync(ShoppingCart shoppingCartModified); 
    }
}
