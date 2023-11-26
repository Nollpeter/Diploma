using BlazorCraft.Web.Shared._Exercises.RehderFragments;
using Employee = BlazorCraft.Web.Shared._Exercises.RehderFragments.RenderFragments_LessonFinal.Employee;

namespace BlazorCraft.Web.Shared._Exercises.DependencyInjection;

public interface IEmployeeService
{
    Task<List<Employee>> GetEmployees();
}

public class EmployeeService : IEmployeeService
{
    public EmployeeService()
    {
        Random r = new Random();
        _employees = new List<Employee>();
        for (int i = 0; i < r.Next(3,8); i++)
        {
            var randomValue = r.Next(100);
            _employees.Add(new Employee(1, $"Firstname_{randomValue}", $"LastName_{randomValue}", "Test position"));
        }
        
    }

    private List<Employee> _employees; 

    public async Task<List<Employee>> GetEmployees()
    {
        // In the real world, this would be an api call to fetch employees, let's simulate it with a delay
        await Task.Delay(100);
        return _employees;
    }
}