namespace BlazorCraft.Web.Shared.Examples._7_DependencyInjection;

public static class ExampleServiceRegistration
{
    public static void AddExampleServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IExampleTransientService, ExampleTransientService>();
        serviceCollection.AddScoped<IExampleScopedService, ExampleScopedService>();
    }
}