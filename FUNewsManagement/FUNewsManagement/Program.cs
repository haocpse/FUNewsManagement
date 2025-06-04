using FuNews.Modals.Mapping;
using FUNews.BLL.InterfaceService;
using FUNews.BLL.Service;
using FUNews.DAL;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using FUNews.DAL.Repository;
using FUNews.Modals.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FUNewsManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            // Add Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Authen/Index";
                    options.AccessDeniedPath = "/Home/Error";
                });
            
            // 1. Đăng ký DbContext với connection string
            builder.Services.AddDbContext<FUNewsDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));
            
            // 2. Đăng ký Repository
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<INewsRepository, NewsRepository>();
            builder.Services.AddScoped<INewsTagRepository, NewsTagRepository>();
            builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();

            // 3. Đăng ký Service
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<INewsService, NewsService>();
            builder.Services.AddScoped<INewsTagService, NewsTagService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            // 4. Đăng ký AutoMapper
            builder.Services.AddAutoMapper(typeof(TagMappingProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(NewsMappingProfile).Assembly);
            

            // Add session services
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Add authentication middleware before authorization
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
