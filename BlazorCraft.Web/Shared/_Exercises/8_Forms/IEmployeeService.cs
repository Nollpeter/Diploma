using BlazorCraft.Web.Shared._Exercises._8_Forms;

namespace BlazorCraft.Web.Shared._Exercises.Forms;

using Employee = Forms_Ex_LessonFinal.Employee;

public interface IEmployeeService
{
    public Task<List<Employee>> GetEmployees();
    public Task<Employee> GetEmployee(int id);
    public Task AddEmployee(Employee employee);
    public Task UpdateEmployee(Employee employee);
}

class EmployeeService : IEmployeeService
{

    private IDictionary<int, Employee> _employees = new Dictionary<int, Employee>();
    
    public Task<List<Employee>> GetEmployees()
    {
        return Task.FromResult(_employees.Values.ToList());
    }

    public Task<Employee> GetEmployee(int id)
    {
        return Task.FromResult(_employees[id]);
    }

    public Task AddEmployee(Employee employee)
    {
        var max = _employees.Keys.Any() ? _employees.Keys.Max() : 1;
        employee.Id = max + 1;
        _employees[employee.Id] = employee;
        return Task.CompletedTask;
    }

    public Task UpdateEmployee(Employee employee)
    {
        _employees[employee.Id] = employee;
        return Task.CompletedTask;
    }
}

public static class ServiceCollectionExtensions
{
    public static void AddEmployeeService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEmployeeService, EmployeeService>(provider =>
        {
            var employeeService = new EmployeeService();
            employeeService.AddEmployee(new Employee() { Id = 1, FirstName = "Tiffany", LastName = "Test", Salary = 40000, Position = "CEO", BirthDate = DateTime.Parse("1989.05.10") });
            employeeService.AddEmployee(new Employee() { Id = 2, FirstName = "Theodore", LastName = "Test", Salary = 35000, Position = "CFO", BirthDate = DateTime.Parse("1990.05.10") });
            employeeService.AddEmployee(new Employee() { Id = 3, FirstName = "Temujin", LastName = "Test", Salary = 34000, Position = "CTO", BirthDate = DateTime.Parse("1991.05.10") });
            employeeService.AddEmployee(new Employee() { Id = 4, FirstName = "Timothy", LastName = "Test", Salary = 30000, Position = "Developer", BirthDate = DateTime.Parse("1992.05.10") });


            return employeeService;
        });
    }
}