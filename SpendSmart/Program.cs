using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;
using SpendSmart.Services;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

namespace SpendSmart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var keyVaultName = "mvc";
            var kvUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
            builder.Configuration.AddAzureKeyVault(kvUri, new DefaultAzureCredential());
            var connectionString = builder.Configuration["SqlConnection"];

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddDbContext<SpendSmartDbContext>(options =>   per usare in memory
            //    options.UseInMemoryDatabase("SpendSmartDb")
            //);

            builder.Services.AddDbContext<SpendSmartDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

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
