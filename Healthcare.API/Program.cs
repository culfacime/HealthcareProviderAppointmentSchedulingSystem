using DotNetEnv;
using Hangfire;
using Healthcare.API.Extensions;
using Healthcare.Core.DB;
using Healthcare.Core.Repositories;
using Healthcare.Core.Services;
using Healthcare.Core.UnitOfWorks;
using Healthcare.Repository.Repository;
using Healthcare.Repository.UnitOfWorks;
using Healthcare.Service.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

builder.Services.AddHangfire(configuration => configuration
        .UseSqlServerStorage(Env.GetString("DB_CONNECTION"), new Hangfire.SqlServer.SqlServerStorageOptions
        {
             PrepareSchemaIfNecessary = true
        }));

builder.Services.AddHangfireServer();

EnvExtensions.LoadEnv();

builder.Services.AddDbContext<HealthcareDbContext>(x =>
{
    x.UseSqlServer(Env.GetString("DB_CONNECTION"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
