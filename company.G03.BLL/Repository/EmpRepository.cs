using company.G03.BLL.Interface;
using company.G03.DAL.Data.Context;
using company.G03.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.BLL.Repository
{
    public class EmpRepository : GenericRepository<Employee>, IEmpRepository
    {
        private readonly CompanyDbContext _companyDbContext;
        public EmpRepository(CompanyDbContext context):base(context) {
          _companyDbContext = context;
        }

        public async Task<List<Employee>> GetNameasync(string name)
        {
            return await _companyDbContext.Employees
         .Include(e => e.Dept)
         .Where(x => x.Name.ToLower().Contains(name.ToLower()))
         .ToListAsync();
        }
    }
}
