using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sample.Application;
using Sample.Common.Caching;
using Sample.Common.Database;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.Extentions;
using Sample.Common.FilterList;
using Sample.Common.Logging;
using Sample.Common.Processing;
using Sample.Common.Processing.Providers;
using Sample.Common.UserSessions;
using Sample.Common.Web;
using Sample.Domain.Resources;
using Sample.Domain.System.Oauth;
using Sample.Domain.System.Users;
using Sample.Infrastructure;
using Sample.Infrastructure.Authentication;
using Sample.Infrastructure.Database;
using Sample.Infrastructure.Domain;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .AddJsonFile($"hosting.{builder.Environment.EnvironmentName}.json", optional: true);

// Cấu hình Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.With<ThreadIdEnricher>()
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddSingleton(Log.Logger);

var configuration = builder.Configuration;
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration
        .GetConnectionString("redis");
});

//builder.Services.AddTransient<ISequenceProvider, SequenceProvider>();
//var assemblies = AppDomain.CurrentDomain.GetAssemblies();

//builder.Services.AddConventionalServices(assemblies);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
}

);

builder.Services.AddHttpContextAccessor(); // cần để inject IHttpContextAccessor
builder.Services.AddScoped<IUserSession, UserSession>();
// 1. Serilog logger
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

// 2. Cache
builder.Services.AddMemoryCache();
var cacheExpirations = new Dictionary<string, TimeSpan>
{
    ["UserCache"] = TimeSpan.FromMinutes(10),
    ["ProductCache"] = TimeSpan.FromMinutes(5),
    // Thêm các cache key và expiration khác
};

builder.Services.AddSingleton(cacheExpirations);
builder.Services.AddScoped<ICacheStore, MemoryCacheStore>();

// 3. Commands scheduler
builder.Services.AddScoped<ICommandsScheduler, CommandsScheduler>();

// 4. SQL connection factory
builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

// 5. Dapper unit of work
builder.Services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();
builder.Services.AddScoped<IEFUnitOfWork, EFUnitOfWork>();

// 6. Generic repository
builder.Services.AddScoped(typeof(IDapperRepository<>), typeof(GenericDapperRepository<>));
builder.Services.AddScoped(typeof(IEFRepository<>), typeof(GenericEFRepository<>));

// 7. MediatR handlers
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(SampleClass).Assembly);
});
builder.Services.AddScoped<ISequenceProvider, SequenceProvider>();

//builder.Services.Scan(scan => scan
//    .FromAssemblies(typeof(IChecker).Assembly)
//    .AddClasses(c => c.AssignableTo<IChecker>())
//    .AsImplementedInterfaces()
//    .WithScopedLifetime()
//);

builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(SampleClass))
    .AddClasses(c => c.Where(type =>
        type.Namespace != null &&
        type.Namespace.EndsWith(".Checker")
    ))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);


//builder.Services.Scan(scan => scan
//    .FromAssemblies(typeof(SampleClass).Assembly)
//    .AddClasses(c => c.Where(t => t.Name.EndsWith("Checker")))
//    .AsImplementedInterfaces()
//    .WithScopedLifetime()
//);


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

var services = builder.Services;

// Add DevExpress
//services.AddDevExpressControls();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var secretKey = configuration[JwtTokenConstants.SecretKey];
    if (string.IsNullOrEmpty(secretKey))
        throw new InvalidOperationException($"JWT secret key '{JwtTokenConstants.SecretKey}' is not configured.");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier,
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidIssuer = configuration[JwtTokenConstants.Issuer],
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidAudience = configuration[JwtTokenConstants.Audience],
        IssuerSigningKey = credentials.Key,
        ClockSkew = TimeSpan.Zero,
        SaveSigninToken = true,
    };
});
builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
builder.Services.AddScoped<IPasswordProvider, PasswordProvider>();


services.AddControllers(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilter<LanguageResource>));
    options.Filters.Add(typeof(ValidatorActionFilter));
    options.ModelBinderProviders.Insert(0, new FilterRequestBinderProvider());
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});

services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressConsumesConstraintForFormFileParameters = true;
    options.SuppressInferBindingSourcesForParameters = true;
    options.SuppressModelStateInvalidFilter = true;
});

services.AddMemoryCache();

services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024;
    options.UseCaseSensitivePaths = true;
});

// CORS
services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
//services.AddDevExpressControls();
//services.ConfigureReportingServices(configurator =>
//{
//    configurator.ConfigureReportDesigner(designerConfigurator => { });
//    configurator.ConfigureWebDocumentViewer(viewerConfigurator => viewerConfigurator.UseCachedReportSourceBuilder());
//});

// Swagger
//services.AddEndpointsApiExplorer();
//services.AddSwaggerGen();
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCaching();

app.MapControllers();

app.Run();
