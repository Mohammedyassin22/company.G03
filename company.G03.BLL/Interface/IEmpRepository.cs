using company.G03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.BLL.Interface
{
    public interface IEmpRepository
    {
        IEnumerable<Employee> GetAll();
        Employee Get(int id);
        int Add(Employee model);
        int Update(Employee model);
        int Delete(Employee model);
    }
}
