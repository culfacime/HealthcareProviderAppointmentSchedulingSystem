using DotNetEnv;
using FluentValidation.AspNetCore;
using Hangfire;
using Healthcare.API.Extensions;
using Healthcare.API.Filters;
using Healthcare.Core.DB;
using Healthcare.Core.Mapping;
using Healthcare.Core.Repositories;
using Healthcare.Core.Services;
using Healthcare.Core.UnitOfWorks;
using Healthcare.Core.Validators;
using Healthcare.Repository.Repository;
using Healthcare.Repository.UnitOfWorks;
using Healthcare.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
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

builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(x =>
{
    x.GroupNameFormat = "'v'VVV";
    x.SubstituteApiVersionInUrl = true;
});

builder.Services
.AddFluentValidation(fv =>
{
    fv.DisableDataAnnotationsValidation = true;
    fv.RegisterValidatorsFromAssemblyContaining<PatientValidator>();
    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    fv.RegisterValidatorsFromAssembly(Assembly.GetEntryAssembly());
    fv.AutomaticValidationEnabled = true;
    fv.ValidatorOptions.LanguageManager.Culture = new System.Globalization.CultureInfo("tr");
});


builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

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
