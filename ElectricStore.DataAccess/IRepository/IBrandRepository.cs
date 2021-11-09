using ElectricStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricStore.DataAccess.IRepository
{
    public interface IBrandRepository:IRepository<Brand>
    {
        Task UpdateAsync(Brand brand);
    }
}
