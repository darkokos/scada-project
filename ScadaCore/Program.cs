using Microsoft.EntityFrameworkCore;
using ScadaCore.Data;
using ScadaCore.Drivers;
using ScadaCore.Repositories;
using ScadaCore.Services;

namespace ScadaCore;

public class UserState
{
    public Dictionary<string, string> Data { get; } = new Dictionary<string, string>();
}

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
        
        // Drivers
        builder.Services.AddScoped<IAnalogRealTimeDriver, AnalogRealTimeDriver>();
        builder.Services.AddScoped<IDigitalRealTimeDriver, DigitalRealTimeDriver>();
        
        // Services
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITagService, TagService>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();
        builder.Services.AddScoped<ITagLogService, TagLogService>();
        builder.Services.AddScoped<ITagLogRepository, TagLogRepository>();
        builder.Services.AddScoped<IAlarmService, AlarmService>();
        builder.Services.AddScoped<IAlarmRepository, AlarmRepository>();
        builder.Services.AddScoped<IAlarmLogRepository, AlarmLogDbRepository>();
        builder.Services.AddScoped<IReportService, ReportService>();
        
        builder.Services.AddSingleton<UserState>();
        
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
