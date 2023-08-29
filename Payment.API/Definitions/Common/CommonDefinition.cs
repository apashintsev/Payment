namespace Payment.API.Definitions.Common;

/// <summary>
/// AspNetCore common configuration
/// </summary>
public class CommonDefinition : AppDefinition
{
    /// <summary>
    /// Configure services for current application
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddLocalization();
        services.AddHttpContextAccessor();
        services.AddResponseCaching();
        services.AddMemoryCache();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
    }

    /// <summary>
    /// Configure application for current application
    /// </summary>
    /// <param name="app"></param>
    public override void ConfigureApplication(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.MapControllers();
    }
}