using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Student_Class_Library;
using Student_MVC_App.data;
using Student_MVC_App.Data;
using System;

namespace Student_MVC_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register StudentsContext so DI provides DbContextOptions<StudentsContext>
            // Uses connection string "DefaultConnection" from appsettings.json
            builder.Services.AddDbContext<StudentsContext>(options =>
                options.UseSqlite($"Data Source=C:\\Users\\joemh\\source\\repos\\adamhoban1\\lab-6\\student MVC App\\student.db"));

            // Identity
            builder.Services.AddIdentity<Appusers, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<StudentsContext>()
                .AddDefaultTokenProviders();

            // optional: simple IEmailSender stub so registration pages won't throw
            builder.Services.AddTransient<IEmailSender, NullEmailSender>();

            // Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // ensure DB + migrations apply (development only)
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var ctx = services.GetRequiredService<StudentsContext>();
                ctx.Database.Migrate();

                // seed roles and an admin user
                //await IdentitySeed.InitializeAsync(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();


        }
        public class NullEmailSender : IEmailSender
        {
            public Task SendEmailAsync(string email, string subject, string htmlMessage) =>
                Task.CompletedTask;
        }
    }
}
