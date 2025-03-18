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
        public void Add(T model)
        {
            _context.Set<T>().Add(model);
        }

        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
        }

        public T Get(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return _context.Employees.Include(e => e.Dept).FirstOrDefault(e => e.Id==id) as T;
            }
                return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>)_context.Employees.Include(e=>e.Dept).ToList();
            }
            return _context.Set<T>().ToList();
        }

        public void Update(T model)
        {
            _context.Set<T>().Update(model);
        }
    }
}
