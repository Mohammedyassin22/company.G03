using company.G03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.BLL.Interface
{
    public interface IGenericRepository<T>where T :BaseEntity
    {
        Task<IEnumerable<T>> GetAllasync();
        Task<T> Getasync(int id);
        Task Addasync(T model);
        void Update(T model);
        void Delete(T model);
    }
}
