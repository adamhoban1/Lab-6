using Microsoft.EntityFrameworkCore;
using Student_Class_Library;
using Student_MVC_App.data;
using Student_MVC_App.Data;

namespace Student_MVC_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register StudentsContext so DI provides DbContextOptions<StudentsContext>
            // Uses connection string "DefaultConnection" from appsettings.json
            builder.Services.AddDbContext<StudentsContext>(options =>
                options.UseSqlite($"Data Source=C:\\Users\\joemh\\source\\repos\\adamhoban1\\lab-6\\student MVC App\\student.db"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
