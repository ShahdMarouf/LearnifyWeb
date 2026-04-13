
using Domain.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presistance;
using Presistance.Data.Contexts;
using Presistance.Repositories;
using ServiceAbstarction;
using ServiceImpementation;
using System.Text;

namespace LearnifyWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region  Add services to the container.
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
              {
                  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                  {
                      Name = "Authorization",
                      Type = SecuritySchemeType.Http,
                      Scheme = "bearer",
                      BearerFormat = "JWT",
                      In = ParameterLocation.Header,
                      Description = "Enter: Bearer {your JWT token}"
                  });

                  options.AddSecurityRequirement(new OpenApiSecurityRequirement
                  {
                   {
                    new OpenApiSecurityScheme
                     {
                        Reference = new OpenApiReference
                          {
                           Type = ReferenceType.SecurityScheme,
                           Id = "Bearer"
                           }
                      },
                         new string[] {}
                   }
                });
              });
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddDbContext<storeDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
            });
            builder.Services.AddScoped<IDataSeed, DataSeeder>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ITokenSevice, TokenService>();
            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
            builder.Services.AddScoped<IStudentCourseService, StudentCourseService>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IScheduleService, ScheduleService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services.AddScoped<INoteService, NoteService>();
            builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = builder.Configuration["Jwt:Issuer"],
                     ValidAudience = builder.Configuration["Jwt:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                 };
             });

            builder.Services.AddAuthorization();
            #endregion

            var app = builder.Build();
            #region DataSeeding
            //using var Scope = app.Services.CreateScope();
            //var Seed = Scope.ServiceProvider.GetRequiredService<IDataSeed>();
            //// SeedStudentsAsync now orchestrates the correct order: Migration -> Courses -> Offerings -> Students
            //await Seed.SeedStudentsAsync();
            //await Seed.SeedStudentCoursesAsync();
            try
            {
                using var scope = app.Services.CreateScope();
                var seed = scope.ServiceProvider.GetRequiredService<IDataSeed>();

                await seed.SeedStudentsAsync();
                await seed.SeedStudentCoursesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            #endregion
            #region Configure the HTTP request pipeline.

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            #endregion



            app.Run();
        }
    }
}
