using ElectricStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricStore.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Product> Product { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<Brand> Brand { get; set; }
        public IEnumerable<ImageSlider> ImageSlider { get; set; }
    }
}
