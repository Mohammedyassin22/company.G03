using company.G03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.BLL.Interface
{
    public interface IDeptRepository:IGenericRepository<Department>
{
        List<Department> GetName(string name);
    }
}
