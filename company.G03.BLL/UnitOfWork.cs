using company.G03.BLL.Interface;
using company.G03.BLL.Repository;
using company.G03.DAL.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _dbContext;
        public IDeptRepository DeptRepository { get; }

        public IEmpRepository EmpRepository { get; }
        public UnitOfWork (CompanyDbContext company)
        {
            _dbContext = company;
            DeptRepository=new DeptRepository(company);
            EmpRepository=new EmpRepository(company);
        }

        public async Task<int> Complete()
        {
            return await _dbContext.SaveChangesAsync();
        }

        

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public async Task<int> Completeasync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
