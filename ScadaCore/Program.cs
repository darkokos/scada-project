using Microsoft.EntityFrameworkCore;
using ScadaCore.Data;

namespace ScadaCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        // DBContexts
        builder.Services.AddDbContext<UserContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("UsersDb")));
        builder.Services.AddDbContext<ValueAndAlarmContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ValuesAndAlarmsDb")));
        
        // Mappers
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        
        // Controllers
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
