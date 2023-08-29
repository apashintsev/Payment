using Payment.API.Definitions.Common;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace Payment.API.Definitions.Serilog
{
    public class SerilogDefinition : AppDefinition
    {
        public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.Enrich.FromLogContext()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Console()
                    .WriteTo.Debug()
                    .WriteTo.File("/log/logs.log",
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        fileSizeLimitBytes: 10485760);
            });
        }
    }
}
