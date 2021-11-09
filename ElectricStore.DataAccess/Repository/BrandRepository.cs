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
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _db;
        public BrandRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public async Task UpdateAsync(Brand brand)
        {
            Brand brandObj = await _db.Brand.FirstOrDefaultAsync(x => x.Id == brand.Id);
            if(brandObj!=null)
            {
                brandObj.Name = brand.Name;
            }
        }
    }
}
