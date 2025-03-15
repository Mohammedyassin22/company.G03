using company.G03.BLL.Interface;
using company.G03.DAL.Data.Context;
using company.G03.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.BLL.Repository
{
    public class DeptRepository : GenericRepository<Department>,IDeptRepository
    {
       public DeptRepository(CompanyDbContext context) : base(context) { }
    }
}
