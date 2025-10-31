

using Bazar.Application.Interfaces;
using Bazar.Application.Mappings;
using Bazar.Application.Services;
using Bazar.Domain.Entites;
using Bazar.Domain.Interfaces;
using Bazar.Infrastracture;
using Bazar.Infrastracture.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bazar.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

           
            var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (string.IsNullOrWhiteSpace(connStr))
                    throw new InvalidOperationException(
                        "Connection string 'DefaultConnection' not found.");

                options.UseSqlServer(connStr);
            });
            builder.Services.AddIdentity<User, IdentityRole<int>>() 
         .AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();

            // 3. Cookie Authentication
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "AuthCookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/api/auth/login";
                options.AccessDeniedPath = "/api/auth/access-denied";
                options.SlidingExpiration = true;
            });
            // Add repositories to the container.
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IRepositoryProduct, RepositoryProduct>();
            builder.Services.AddScoped<IRepositoryCategory, RepositoryCategory>();
            builder.Services.AddScoped<IRepositoryAdvertisements, RepositoryAdvertisements>();
            // Add services to the container.
            builder.Services.AddScoped<IAdvertisementsService , AdvertisementsService>();
            builder.Services.AddScoped<ICategoryService , CategoryService>();
            builder.Services.AddScoped<IProductService , ProductService>();
            builder.Services.AddScoped<IUserService,UesrService>();
            // Add auto mapper
            builder.Services.AddAutoMapper(cfe => cfe.AddProfile<AutoMapperProfile>());
            // ≈÷«›… Œœ„… «·–ﬂ«¡ «·«’ÿ‰«⁄Ì
            builder.Services.AddScoped<IAIService>(provider =>
            {
                var apiKey = builder.Configuration["OpenAI:ApiKey"];
                return new AIService(apiKey);
            });

            //----------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
