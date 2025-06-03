using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Worknet.BLL.Interfaces;
using Worknet.BLL.Mapping;
using Worknet.BLL.Services;
using Worknet.Core.Configurations;
using Worknet.Core.Entities;
using Worknet.DAL;
using Worknet.Infrastructure;
using Worknet.Shared.Constantsl;

namespace Worknet.API.Util;
internal static class WebAppBuilder
{
    public static WebApplication BuildWebApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var configurations = builder.Configuration;

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(AppSettings.FrontendAppName, policy =>
            {
                policy.WithOrigins("http://localhost:4200") 
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddOpenApi();

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddDbContext<WorknetDbContext>(options =>
            options.UseNpgsql(configurations.GetConnectionString("WorknetDatabase")));

        AddJwtAuthentication(configurations, services);
        AddConfigs(configurations, services);
        AddServices(configurations, services);

        return builder.Build();
    }

    public static void AddJwtAuthentication(IConfiguration configurations, IServiceCollection services)
    {
        var key = configurations["Jwt:Key"];

        if (key is null)
            throw new ArgumentNullException(nameof(key));
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configurations["Jwt:Issuer"],
                ValidAudience = configurations["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });
    }

    public static void AddConfigs(IConfiguration configurations, IServiceCollection services)
    {
        services.Configure<JwtConfig>(configurations.GetSection(JwtConfig.ConfigName));
        services.Configure<GoogleDriveConfig>(configurations.GetSection(GoogleDriveConfig.ConfigName));
    }

    public static void AddServices(IConfiguration configurations, IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<WorknetDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProfileService, ProfileService>();
        
        services.AddInfrastructure(configurations);

    }
}