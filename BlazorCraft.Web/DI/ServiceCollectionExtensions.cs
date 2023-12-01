using System.Reflection;
using BlazorCraft.Web.Infrastructure;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Infrastructure.TestLogging;
using BlazorCraft.Web.Shared._Exercises.DependencyInjection;
using BlazorCraft.Web.Shared._Exercises.Exam;
using BlazorCraft.Web.Shared.Examples._7_DependencyInjection;

namespace BlazorCraft.Web.DI;

public static class ServiceCollectionExtensions
{
    public static void AddBlazorCraftServices(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IAppInitService, AppInitService>();
        serviceCollection.AddScoped<ITestRunnerService, TestRunnerService>();
        serviceCollection.AddScoped<IPanelStateService, PanelStateService>();
        serviceCollection.AddExampleServices();
        serviceCollection.AddEmployeeService();
        serviceCollection.AddExamEmployeeService();
        serviceCollection.AddScoped<INavigationService, NavigationService>();
        Shared._Exercises.Forms.ServiceCollectionExtensions.AddEmployeeService(serviceCollection);
        serviceCollection.AddSingleton<IAsyncLockProvider, AsyncLockProvider>();
        serviceCollection.AddScoped<ITestLoggerService, TestLoggerService>();
        serviceCollection.AddScoped<IExamTestLoggingRepository,ExamTestLoggingRepository>();
        serviceCollection.AddScoped<IAllTestLoggingRepository,AllTestStateLoggingRepository>();
        serviceCollection.AddScoped<ISendResultsService, SendResultsService>();
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