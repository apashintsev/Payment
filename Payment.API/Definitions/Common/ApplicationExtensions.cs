namespace Payment.API.Definitions.Common
{
    public static class AppDefinitionExtensions
    {
        public static void AddDefinitions(this IServiceCollection source, WebApplicationBuilder builder, params Type[] entryPointsAssembly)
        {
            IServiceCollection source2 = source;
            WebApplicationBuilder builder2 = builder;
            ILogger<AppDefinition> requiredService = source2.BuildServiceProvider().GetRequiredService<ILogger<AppDefinition>>();
            List<IAppDefinition> list = new();
            for (int i = 0; i < entryPointsAssembly.Length; i++)
            {
                List<IAppDefinition> list2 = entryPointsAssembly[i].Assembly.ExportedTypes.Where((x) => !x.IsAbstract && typeof(IAppDefinition)!.IsAssignableFrom(x)).Select(new Func<Type, object>(Activator.CreateInstance)).Cast<IAppDefinition>()
                    .ToList();
                List<IAppDefinition> list3 = (from x in list2
                                              where x.Enabled
                                              orderby x.OrderIndex
                                              select x).ToList();
                if (requiredService.IsEnabled(LogLevel.Debug))
                {
                    requiredService.LogDebug("[AppDefinitions] Founded: {AppDefinitionsCountTotal}. Enabled: {AppDefinitionsCountEnabled}", list2.Count, list3.Count);
                    requiredService.LogDebug("[AppDefinitions] Registered [{Total}]", string.Join(", ", list3.Select((x) => x.GetType().Name).ToArray()));
                }

                list.AddRange(list3);
            }

            list.ForEach(delegate (IAppDefinition app)
            {
                app.ConfigureServices(source2, builder2);
            });
            source2.AddSingleton((IReadOnlyCollection<IAppDefinition>)list);
        }

        public static void UseDefinitions(this WebApplication source)
        {
            WebApplication source2 = source;
            ILogger<AppDefinition> requiredService = source2.Services.GetRequiredService<ILogger<AppDefinition>>();
            source2.Services.GetRequiredService<IWebHostEnvironment>();
            List<IAppDefinition> list = (from x in source2.Services.GetRequiredService<IReadOnlyCollection<IAppDefinition>>()
                                         where x.Enabled
                                         orderby x.OrderIndex
                                         select x).ToList();
            list.ForEach(delegate (IAppDefinition x)
            {
                x.ConfigureApplication(source2);
            });
            if (requiredService.IsEnabled(LogLevel.Debug))
            {
                requiredService.LogDebug("Total application definitions configured {Count}", list.Count);
            }
        }
    }
}
