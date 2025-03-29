using company.G03.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace company.G03.DAL.Data.Context
{
    public class CompanyDbContext:IdentityDbContext<AppUsers>
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Department> Departments {  get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
