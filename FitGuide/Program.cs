
using Core.Identity;
using Core.Identity.Entities;
using Core.Identity.Interfaces;
using Core.Interface;
using FitGuide.ErrorsManaged;
using FitGuide.MiddleWares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using Repository;
using Repository.Repositories;
using ServiceLayer;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace FitGuide
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;
            
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppIdentityDbContext>(options=>
            options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

            builder.Services.AddDbContext<FitGuideContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User,IdentityRole>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts=5;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();
            builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty))

                };
            });
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
            builder.Services.AddScoped(typeof(IGeneric<>), typeof(GenericRepo<>));



            builder.Services.Configure<ApiBehaviorOptions>(
               options =>
               {
                   options.InvalidModelStateResponseFactory =
                   (actioncontext) =>
                   {
                       var errors = actioncontext.ModelState.Where(e => e.Value.Errors.Count() > 0)
                       .SelectMany(p => p.Value.Errors).Select(e => e.ErrorMessage).ToList();
                       var response = new ApiValidationErrorResponse()
                       {
                           Errors = errors
                       };
                       return new BadRequestObjectResult(response);
                   };



               }
              );


            //builder.services.addauthentication(options =>
            //{
            //    options.defaultauthenticatescheme = jwtbearerdefaults.authenticationscheme;
            //    options.defaultchallengescheme = jwtbearerdefaults.authenticationscheme;
            //})



            var app=builder.Build();



            using (var src = app.Services.CreateScope())
            {
                var services = src.ServiceProvider;//resolve the serices that you want to use as a depedndency injection
                var _dbcontext = services.GetRequiredService<AppIdentityDbContext>();
                var _identitydbccontext = services.GetRequiredService<AppIdentityDbContext>();
                var _usermanager = services.GetRequiredService<UserManager<User>>();
                var _logger = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    await _dbcontext.Database.MigrateAsync();
                    await _identitydbccontext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    var logger = _logger.CreateLogger<Program>();
                    logger.LogError(ex, "Error Occured During Migration");

                }
            }
            app.UseMiddleware<ExceptionMiddleWare>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ProfileTimerMiddleWare>();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.HttpOnly= true; 
            //    options.ExpireTimeSpan = TimeSpan.FromDays(7);
            //    options.SlidingExpiration = true;
            //    options.LoginPath= "/controller/Login";
            //});

            app.MapControllers();

            app.Run();
        }
    }
}
