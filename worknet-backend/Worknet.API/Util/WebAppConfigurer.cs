using Worknet.Shared.Constantsl;

namespace Worknet.API.Util;
internal static class WebAppConfigurer
{
    public static async Task ConfugureWebApp(WebApplication app)
    {
        app.UseCors(AppSettings.FrontendAppName);
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
}