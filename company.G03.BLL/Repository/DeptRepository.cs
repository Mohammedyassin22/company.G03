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
    public class DeptRepository : IDeptRepository
    {
        private readonly CompanyDbContext _context;
        public DeptRepository(CompanyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }
        public Department Get(int id)
        {
            return _context.Departments.Find(id);
        }
         public int Add(Department model)
        {
            _context.Add(model);
            return _context.SaveChanges();
        }
        public int Update(Department model)
        {
            _context.Update(model);
            return _context.SaveChanges();
        }
        public int Delete(Department model)
        {
            _context.Remove(model);
            return _context.SaveChanges();
        }
    }
}
