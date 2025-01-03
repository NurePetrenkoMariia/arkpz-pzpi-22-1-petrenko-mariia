
using FarmKeeper.Data;
using FarmKeeper.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper
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
            builder.Services.AddDbContext<FarmKeeperDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("FarmKeeperConnectionString")));

            builder.Services.AddScoped<IAnimalRepository, SQLAnimalRepository>();
            builder.Services.AddScoped<IFarmRepository, SQLFarmRepository>();
            builder.Services.AddScoped<IUserRepository, SQLUserRepository>();
            builder.Services.AddScoped<IStableRepository, SQLStableRepository>();
            builder.Services.AddScoped<IAssignmentRepository, SQLAssignmentRepository>();
            builder.Services.AddScoped<INotificationRepository, SQLNotificationRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
