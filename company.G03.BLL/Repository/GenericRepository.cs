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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _context;
        public GenericRepository(CompanyDbContext context)
        {
            _context = context;
        }
        public async Task Addasync(T model)
        {
           await _context.AddAsync(model);
        }

        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
        }

        public async  Task<T> Getasync(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(e => e.Dept).FirstOrDefaultAsync(e => e.Id==id) as T;
            }
                return _context.Set<T>().Find(id);
        }

        public async  Task<IEnumerable<T>> GetAllasync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>)await _context.Employees.Include(e=>e.Dept).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public void Update(T model)
        {
            _context.Set<T>().Update(model);
        }
    }
}
