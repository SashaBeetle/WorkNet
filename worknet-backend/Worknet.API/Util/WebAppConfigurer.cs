namespace Worknet.API.Util;
internal static class WebAppConfigurer
{
    public static async Task ConfugureWebApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}