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
    public class ImageSliderRepository : Repository<ImageSlider>, IImageSliderRepository
    {
        private readonly ApplicationDbContext _db;
        public ImageSliderRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public async Task UpdateAsync(ImageSlider imageSlider)
        {
            ImageSlider imageSliderObj = await _db.ImageSlider.FirstOrDefaultAsync(x => x.Id == imageSlider.Id);
            if(imageSliderObj!=null)
            {
                imageSliderObj.Name = imageSliderObj.Name;
            }
        }
    }
}
