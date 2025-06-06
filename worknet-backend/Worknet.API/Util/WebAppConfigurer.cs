using Microsoft.AspNetCore.Builder;
using Worknet.Shared.Constantsl;
using Worknet.Shared.Helpers;

namespace Worknet.API.Util;
internal static class WebAppConfigurer
{
    public static async Task ConfugureWebApp(WebApplication app)
    {
        var serviceProvider = app.Services;

        app.UseCors(AppSettings.FrontendAppName);
        ConfigureStaticMembers(serviceProvider);

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }

    private static void ConfigureStaticMembers(IServiceProvider serviceProvider)
    {
        UserHelper.Configure(serviceProvider.GetRequiredService<IHttpContextAccessor>());
    }
}