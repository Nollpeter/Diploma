namespace BlazorCraft.Web.Shared._Exercises.DependencyInjection;

public static class DependencyInjection_EmployeeServiceRegistration
{
    public static void AddEmployeeService(this IServiceCollection serviceCollection)
    {
        // Register EmployeeService 
        serviceCollection.AddScoped<IEmployeeService, EmployeeService>();
    }
}