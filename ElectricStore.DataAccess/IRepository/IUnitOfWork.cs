
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricStore.DataAccess.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        public ICategoryRepository Category { get; }
        public IBrandRepository Brand { get; }
        public IProductRepository Product { get;  }
        public IApplicationUserRepository ApplicationUser { get; }
        public IShoppingCartRepository ShoppingCart { get; }
        public IOrderDetailsRepository OrderDetails { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IImageSliderRepository ImageSlider { get;  }
        Task SaveAsync();
    }
}
