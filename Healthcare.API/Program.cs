using DotNetEnv;
using FluentValidation.AspNetCore;
using Hangfire;
using Healthcare.API.Configurations.ApiVersion;
using Healthcare.API.Extensions;
using Healthcare.API.Filters;
using Healthcare.API.Jobs;
using Healthcare.Core.DB;
using Healthcare.Core.Mapping;
using Healthcare.Core.Repositories;
using Healthcare.Core.Services;
using Healthcare.Core.UnitOfWorks;
using Healthcare.Core.Validators;
using Healthcare.Repository.Repository;
using Healthcare.Repository.UnitOfWorks;
using Healthcare.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("V1", new OpenApiInfo { Title = "API", Version = "V1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
builder.Services.ConfigureOptions<SwaggerConfiguration>();
builder.Services.AddControllersWithViews()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped(typeof(ReminderJob));
builder.Services.AddScoped(typeof(IEmailService),typeof(EmailService));
builder.Services.AddScoped(typeof(ISmsService),typeof(SmsService));

EnvExtensions.LoadEnv();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = Env.GetString("JWT_Issuer"),
                ValidAudience = Env.GetString("JWT_Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_Key")))
            };
        });

builder.Services.AddAuthorization();

builder.Services.AddHangfire(configuration => configuration
        .UseSqlServerStorage(Env.GetString("DB_CONNECTION"), new Hangfire.SqlServer.SqlServerStorageOptions
        {
             PrepareSchemaIfNecessary = true
        }));

builder.Services.AddHangfireServer();



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

builder.Services.AddFluentValidation(fv =>
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
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {

            o.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
        }

        o.UseResponseInterceptor(@"function (res) { 
                    if (res.status === 200) { 
                        try {
                        let X = JSON.parse(res.data);
                            if (X.token) {
                                ui.preauthorizeApiKey('Bearer', 'Bearer ' + X.token)
                            }
                        }
                        catch {
                        }
                    }
                    return res;
                }".Replace('\n', ' ').Replace('\r', ' '));
    });
}


app.UseHangfireDashboard();

using var scope = app.Services.CreateScope();
var reminder = scope.ServiceProvider.GetRequiredService<ReminderJob>();


RecurringJob.AddOrUpdate(
    "reminderJob",
    () => reminder.Run(),
    Cron.Hourly());

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
