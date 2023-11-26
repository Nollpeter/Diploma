using BlazorCraft.Web.Infrastructure;
using BlazorCraft.Web.Shared._Exercises.DependencyInjection;
using BlazorCraft.Web.Shared.Examples._7_DependencyInjection;

namespace BlazorCraft.Web.DI;

public static class ServiceCollectionExtensions
{
    public static void AddBlazorCraftServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ITestRunnerService, TestRunnerService>();
        serviceCollection.AddScoped<IPanelStateService, PanelStateService>();
        serviceCollection.AddExampleServices();
        serviceCollection.AddEmployeeService();
    }
}