using Microsoft.EntityFrameworkCore;
using company.G03.DAL.Data.Context;
using company.G03.BLL.Repository;
using company.G03.BLL.Interface;
using company.G03.PL.Mapping;
using company.G03.BLL;
using company.G03.DAL.Models;
using Microsoft.AspNetCore.Identity;
namespace company.G03
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. ����� ������� ��������
            builder.Services.AddControllersWithViews();

            // 2. ����� ������� - ������� ������ �������
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDeptRepository, DeptRepository>();
            builder.Services.AddScoped<IEmpRepository, EmpRepository>();
            builder.Services.AddScoped<IDeptRepository, DeptRepository>();
            builder.Services.AddScoped<DeptRepository>();
            
            // 3. ����� AutoMapper
            builder.Services.AddAutoMapper(typeof(EmpProfile), typeof(DeptProfile));

            // 4. ����� ����� ��������
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // 5. ����� Identity (���� Authentication ��������)
            builder.Services.AddIdentity<AppUsers, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<CompanyDbContext>()
            .AddEntityFrameworkStores<CompanyDbContext>()
            .AddDefaultTokenProviders();

            // 6. ����� ����� ����� ��������
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

            // 7. ����� ������
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // ����� ���� ��� Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseStatusCodePagesWithReExecute("/Home/NotFound");

            // ������� ������
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
