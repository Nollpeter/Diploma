namespace BlazorCraft.Web.Shared._Exercises.Exam;

public interface IExamEmployeeService
{
    public Task<List<ExamEmployee>> GetEmployees();
    public Task<ExamEmployee> GetEmployee(int id);
    public Task<ExamEmployee> GetEmployeeForEdit(int id);
    public Task AddEmployee(ExamEmployee employee);
    public Task UpdateEmployee(ExamEmployee employee);
    public Task DeleteEmployee(ExamEmployee employee);
}

public class ExamEmployeeService : IExamEmployeeService
{

    private IDictionary<int, ExamEmployee> _employees = new Dictionary<int, ExamEmployee>();
    
    public Task<List<ExamEmployee>> GetEmployees()
    {
        return Task.FromResult(_employees.Values.ToList());
    }

    public async Task<ExamEmployee> GetEmployee(int id)
    {
        await Task.CompletedTask;
        return _employees[id];
    }

    public async Task<ExamEmployee> GetEmployeeForEdit(int id)
    {
        var employeeForEdit = new ExamEmployee(await GetEmployee(id));
        return employeeForEdit;
    }

    public Task AddEmployee(ExamEmployee employee)
    {
        var max = _employees.Keys.Any() ? _employees.Keys.Max() : 0;
        employee.Id = max + 1;
        _employees[employee.Id] = employee;
        return Task.CompletedTask;
    }

    public Task UpdateEmployee(ExamEmployee employee)
    {
        _employees[employee.Id] = employee;
        return Task.CompletedTask;
    }

    public Task DeleteEmployee(ExamEmployee employee)
    {
        _employees.Remove(employee.Id);
        return Task.CompletedTask;
    }
}

public static class ServiceCollectionExtensions
{
    public static void AddExamEmployeeService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IExamEmployeeService, ExamEmployeeService>(_ =>
        {
            var employeeService = new ExamEmployeeService();
            FillTestData(employeeService);
            return employeeService;
        });
    }

    public static IExamEmployeeService FillTestData(this IExamEmployeeService employeeService)
    {
        employeeService.AddEmployee(new ExamEmployee()
            { Id = 1, FirstName = "Tiffany", LastName = "Test", Salary = 40000, Position = EmployeePosition.CEO, BirthDate = DateTime.Parse("1989.05.10"), HireDate = DateTime.Parse("2022.01.01"), Gender = EmployeeGender.Female });
        employeeService.AddEmployee(new ExamEmployee()
            { Id = 2, FirstName = "Theodore", LastName = "Test", Salary = 35000, Position = EmployeePosition.CFO, BirthDate = DateTime.Parse("1990.05.10"), HireDate = DateTime.Parse("2022.01.01"), Gender = EmployeeGender.Male });
        employeeService.AddEmployee(new ExamEmployee()
            { Id = 3, FirstName = "Temujin", LastName = "Test", Salary = 34000, Position = EmployeePosition.CTO, BirthDate = DateTime.Parse("1991.05.10"), HireDate = DateTime.Parse("2022.01.01"), Gender = EmployeeGender.Male });
        employeeService.AddEmployee(new ExamEmployee()
            { Id = 4, FirstName = "Timothy", LastName = "Test", Salary = 30000, Position = EmployeePosition.Developer, BirthDate = DateTime.Parse("1992.05.10"), HireDate = DateTime.Parse("2022.01.01"), Gender = EmployeeGender.Male });
        return employeeService;
    }
}