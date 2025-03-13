using company.G03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.BLL.Interface
{
    public interface IDeptRepository
{
        IEnumerable<Department> GetAll();
        Department Get(int id);
        int Add(Department model);
        int Update(Department model);
        int Delete(Department model);
    }
}
