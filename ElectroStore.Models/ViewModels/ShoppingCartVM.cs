using ElectricStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricStore.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public bool ExistsinCart { get; set; }
    }
}
