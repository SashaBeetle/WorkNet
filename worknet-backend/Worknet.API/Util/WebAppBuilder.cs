using Microsoft.EntityFrameworkCore;
using Worknet.DAL;

namespace Worknet.API.Util;
internal static class WebAppBuilder
{
    public static WebApplication BuildWebApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        services.AddControllers();
        services.AddOpenApi();

        services.AddDbContext<WorknetDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("WorknetDatabase")));

        return builder.Build();
    }
}