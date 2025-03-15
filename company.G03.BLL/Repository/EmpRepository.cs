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
    public class EmpRepository : IEmpRepository
    {
        private readonly CompanyDbContext _context;
        public EmpRepository(CompanyDbContext context) {
            _context = context;
        }
        public int Add(Employee model)
        {
            _context.Employees.Add(model);
            return _context.SaveChanges();
        }

        public int Delete(Employee model)
        {
            _context.Employees.Remove(model);
            return _context.SaveChanges();
        }

        public Employee Get(int id)
        {
            return _context.Employees.Find(id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public int Update(Employee model)
        {
            _context.Employees.Update(model);
            return _context.SaveChanges();
        }
    }
}
