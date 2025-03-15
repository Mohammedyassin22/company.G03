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
        IEnumerable<T> GetAll();
        T Get(int id);
        int Add(T model);
        int Update(T model);
        int Delete(T model);
    }
}
