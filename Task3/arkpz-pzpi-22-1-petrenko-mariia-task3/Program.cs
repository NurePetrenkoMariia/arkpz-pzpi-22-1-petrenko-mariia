
using FarmKeeper.Data;
using FarmKeeper.Enums;
using FarmKeeper.Models;
using FarmKeeper.Repositories;
using FarmKeeper.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Security.Claims;

namespace FarmKeeper
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

                   options.JsonSerializerOptions.WriteIndented = true;
               });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Demo API", Version = "v1" 
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddDbContext<FarmKeeperDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("FarmKeeperConnectionString")));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                       System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])
                    )
                };
            });

            builder.Services.AddScoped<IAnimalRepository, SQLAnimalRepository>();
            builder.Services.AddScoped<IFarmRepository, SQLFarmRepository>();
            builder.Services.AddScoped<IUserRepository, SQLUserRepository>();
            builder.Services.AddScoped<IStableRepository, SQLStableRepository>();
            builder.Services.AddScoped<IAssignmentRepository, SQLAssignmentRepository>();
            builder.Services.AddScoped<INotificationRepository, SQLNotificationRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserTaskRepository, SQLUserTaskRepository>();
            builder.Services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
            builder.Services.AddScoped<SeedDbService>();
            builder.Services.AddScoped<IFeedMonitoringService, FeedMonitoringService>();

            builder.Services.AddCors(corsOptions => {
                corsOptions.AddPolicy("AllowAll", policy => {
                    policy.WithHeaders().AllowAnyHeader(); policy.WithHeaders().AllowCredentials();
                    policy.WithOrigins().AllowAnyMethod()
                       .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            app.UseCors("AllowAll");

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seedDbService = services.GetRequiredService<SeedDbService>();
                await seedDbService.SeedAdminUserAsync();
            }

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
