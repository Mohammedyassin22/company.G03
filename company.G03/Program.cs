using Microsoft.EntityFrameworkCore;
using company.G03.DAL.Data.Context;
using company.G03.BLL.Repository;
using company.G03.BLL.Interface;
using company.G03.PL.Mapping;
using company.G03.BLL;
namespace company.G03
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAutoMapper(typeof(EmpProfile));
            builder.Services.AddAutoMapper(typeof(DeptProfile));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<DeptRepository>();
            builder.Services.AddScoped<IDeptRepository, DeptRepository>();
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IEmpRepository,EmpRepository>();
            var app = builder.Build();
           
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
