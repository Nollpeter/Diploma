using System.Reflection;
using BlazorCraft.Web.Infrastructure;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.DependencyInjection;
using BlazorCraft.Web.Shared._Exercises.JsInterop;
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
        Shared._Exercises.Forms.ServiceCollectionExtensions.AddEmployeeService(serviceCollection);
        serviceCollection.AddSingleton<IAsyncLockProvider, AsyncLockProvider>();
        serviceCollection.AddTests();
        
    }

    public static void AddTests(this IServiceCollection serviceCollection)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            var testForPageAttribute = type.GetCustomAttribute<TestForPageAttribute>();
            if (testForPageAttribute != null)
            {
                serviceCollection.AddScoped(type);
            }
        }
    }
}