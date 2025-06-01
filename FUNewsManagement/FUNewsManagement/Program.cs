using FUNews.BLL.InterfaceService;
using FUNews.BLL.Service;
using FUNews.DAL;
using FUNews.DAL.InterfaceRepository;
using FUNews.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            // 1. Đăng ký DbContext với connection string
            builder.Services.AddDbContext<FUNewsDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));
            
            // 2. Đăng ký Repository
            //    → Bắt buộc phải có hai tham số: interface và class cụ thể
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            
            // 3. Đăng ký Service
            builder.Services.AddScoped<ITagService, TagService>();

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
