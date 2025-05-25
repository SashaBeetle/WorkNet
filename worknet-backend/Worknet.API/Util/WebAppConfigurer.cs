namespace Worknet.API.Util;
internal static class WebAppConfigurer
{
    public static async Task ConfugureWebApp(WebApplication app)
    {
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