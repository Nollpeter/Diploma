using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.Exam;
using BlazorCraft.Web.Shared.Examples.Routing;
using Bunit;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using NSubstitute;

namespace BlazorCraft.Web.Tests.Exam;

[TestForPage(typeof(Pages._11_Exam.Exam))]
public class Test_Exam_PuttingItTogether : ExamTestBase<Exercise_Exam>
{
    private IExamEmployeeService _employeeService;

    protected override async Task<TestContext> SetupTestContext()
    {
        var setupTestContext = await base.SetupTestContext();
        var examEmployeeService = new ExamEmployeeService().FillTestData();
        
        _employeeService = Substitute.For<IExamEmployeeService>();
        _employeeService.GetEmployeeForEdit(Arg.Any<int>()).ReturnsForAnyArgs(async ci =>
        {
            var result = await examEmployeeService.GetEmployeeForEdit(ci.Arg<int>());
            return result;
        });
        _employeeService.GetEmployee(Arg.Any<int>()).ReturnsForAnyArgs(async ci =>
        {
            var result = await examEmployeeService.GetEmployee(ci.Arg<int>());
            return result;
        });
        _employeeService.AddEmployee(Arg.Any<ExamEmployee>()).ReturnsForAnyArgs(async ci => { await examEmployeeService.AddEmployee(ci.Arg<ExamEmployee>()); });
        _employeeService.UpdateEmployee(Arg.Any<ExamEmployee>()).ReturnsForAnyArgs(async ci => { await examEmployeeService.UpdateEmployee(ci.Arg<ExamEmployee>()); });
        _employeeService.DeleteEmployee(Arg.Any<ExamEmployee>()).ReturnsForAnyArgs(async ci => { await examEmployeeService.DeleteEmployee(ci.Arg<ExamEmployee>()); });
        _employeeService.GetEmployees().Returns(async ci => await examEmployeeService.GetEmployees());
        
        setupTestContext.Services.AddScoped<IExamEmployeeService>((_) => _employeeService);
        return setupTestContext;
    }

    // Employees are loaded on render
    [Title("Employees are loaded after rendering component")]
    // TODO Description
    [Description("")]
    [Precondition]
    public async Task GivenExam_WhenComponentRendered_ThenEmployeesAreLoaded()
    {
        var ctx = await SetupTestContext();
        var examEmployees = await _employeeService.GetEmployees();
        var renderedComponent = ctx.RenderComponent<Exercise_Exam>();
        var instanceEmployees = renderedComponent.Instance.Employees;
        var missingEmployees = new List<ExamEmployee>();
        foreach (var employee in examEmployees)
        {
            if (!instanceEmployees.Contains(employee))
            {
                missingEmployees.Add(employee);
            }
        }

        if (missingEmployees.Any())
        {
            throw new TestRunException("Not all employees were loaded!");
            //TODO Ide a missing employeek
            
        }
    }
    
    //Table items are bound
    [Title("Employees are bound to Table")]
    //TODO Description
    [Description("")]
    public async Task GivenExam_WhenComponentRendered_ThenEmployeesListBoundToTableItems()
    {
        var ctx = await SetupTestContext();
        var examEmployees = await _employeeService.GetEmployees();
        var renderedComponent = ctx.RenderComponent<Exercise_Exam>();
        var table = renderedComponent.FindComponent<MudTable<ExamEmployee>>();
        if (!Equals(table.Instance.Items, renderedComponent.Instance.Employees))
        {
            throw new TestRunException($"The Items property of the Table is not bound to the Employees property of {nameof(Exercise_Exam)}");
        }
    }
    
    // Table headers rendered properly
    [Title("Table headers rendered correctly")]
    // TODO Description
    [Description("")]
    [Precondition]
    public async Task GivenExam_WhenComponentRendered_ThenTableHeadersProperlyRendered()
    {
        var ctx = await SetupTestContext();
        var examEmployees = await _employeeService.GetEmployees();
        var renderedComponent = ctx.RenderComponent<Exercise_Exam>();
        var table = renderedComponent.FindComponent<MudTable<ExamEmployee>>();
        var headers = table.FindComponents<MudTh>();
        if (headers.Count != 6)
        {
            throw new TestRunException("Invalid number of table headers! There should be 6 of them: Id, FirstName, LastName, Position, Salary, and one extra header for Create Employee");
        }

        if (headers.All(p => ctx.Render(p.Instance.ChildContent).Markup != "Id"))
        {
            throw new TestRunException("Missing header for Id!");
        }
        if (headers.All(p => ctx.Render(p.Instance.ChildContent).Markup != "First Name"))
        {
            throw new TestRunException("Missing header for First Name!");
        }
        if (headers.All(p => ctx.Render(p.Instance.ChildContent).Markup != "Last Name"))
        {
            throw new TestRunException("Missing header for Last Name!");
        }
        if (headers.All(p => ctx.Render(p.Instance.ChildContent).Markup != "Position"))
        {
            throw new TestRunException("Missing header for Position!");
        }
        if (headers.All(p => ctx.Render(p.Instance.ChildContent).Markup != "Salary"))
        {
            throw new TestRunException("Missing header for Salary!");
        }
        if (headers.All(p => p.HasComponent<MudButton>() && p.FindComponent<MudButton>().Instance.ChildContent?.ToString() == "Create new"))
        {
            throw new TestRunException("Missing header for Create new employee!");
        }
        
        
    }

    //table body rendered properly
    [Title("Table body rendered correctly")]
    // TODO Description
    [Description("")]
    [Precondition]
    public async Task GivenExam_WhenComponentRendered_ThenTableBodyProperlyRenders()
    {
        await GivenExam_WhenComponentRendered_ThenEmployeesListBoundToTableItems();
        var ctx = await SetupTestContext();
        var examEmployees = await _employeeService.GetEmployees();
        var renderedComponent = ctx.RenderComponent<Exercise_Exam>();
        var table = renderedComponent.FindComponent<MudTable<ExamEmployee>>();
        
        var employee = examEmployees[0];
        {
            var renderedFragment = ctx.Render(table.Instance.RowTemplate(employee));

            var columns = renderedFragment.FindComponents<MudTd>();
            if (columns.Count != 6)
            {
                throw new TestRunException("Invalid number of table body columns! There should be 6 of them: Id, FirstName, LastName, Position, Salary, and one extra cell for Employee Details");
            }

            var markups = columns.Select(p => ctx.Render(p.Instance.ChildContent).Markup).ToArray();
            if (markups[0] != employee.Id.ToString())
            {
                throw new TestRunException($"First column should be Employee.Id: {employee.Id}, instead found: {markups[0]}");
            }
            
            if (markups[1] != employee.FirstName)
            {
                throw new TestRunException($"First column should be Employee.FirstName: {employee.FirstName}, instead found: {markups[0]}");
            }

            if (markups[2] != employee.LastName)
            {
                throw new TestRunException($"First column should be Employee.LastName: {employee.LastName}, instead found: {markups[0]}");
            }
            if (markups[3] != employee.Position.ToString())
            {
                throw new TestRunException($"First column should be Employee.Position: {employee.Position}, instead found: {markups[0]}");
            }
            if (markups[4] != employee.Salary.ToString())
            {
                throw new TestRunException($"First column should be Employee.Salary: {employee.Salary}, instead found: {markups[0]}");
            }

            var detailsColumn = ctx.Render(columns[5].Instance.ChildContent);
            if (!detailsColumn.HasComponent<MudIconButton>())
            {
                throw new TestRunException("Missing column for employee details!!");
            }
        }
    }
    
    
    //Clicking details icon open employee details
    [Title("Clicking on details icon opens "+nameof(ExamEmployeeDetails))]
    // TODO Description
    [Description("")]
    public async Task GivenExam_WhenDetailsIconClicked_ThenExamEmployeeDetailsOpens()
    {
        var ctx = await SetupTestContext();
        var renderedComponent = ctx.RenderComponent<Exercise_Exam>();
        await renderedComponent.Find($"#{Exercise_Exam.DetailsId(1)}").ClickAsync(new MouseEventArgs());
        var hasComponent = renderedComponent.HasComponent<ExamEmployeeDetails>();
        if (!hasComponent)
        {
            throw new TestRunException($"Clicking Details Icon does not open {nameof(ExamEmployeeDetails)}!");
        }
    }
    
    // EmployeeEdit opened with Employee bound on Clicking Details
    [Title(nameof(ExamEmployeeDetails)+" employee id is bound to employee id for which details was clicked")]
    // TODO Description
    [Description("")]
    public async Task GivenExam_WhenDetailsIconClicked_ThenEmployeeIsBound()
    {
        var ctx = await SetupTestContext();
        var renderedComponent = ctx.RenderComponent<Exercise_Exam>();
        var employeeId = 3;
        await renderedComponent.Find($"#{Exercise_Exam.DetailsId(employeeId)}").ClickAsync(new MouseEventArgs());
        var employeeDetails = renderedComponent.FindComponent<ExamEmployeeDetails>();
        if (employeeDetails.Instance.EmployeeId != employeeId)
        {
            throw new TestRunException($"The employee Id is not bound to the {nameof(ExamEmployeeDetails)}.{nameof(ExamEmployeeDetails.EmployeeId)}!");
        }
    }

    [Title("Clicking Create new button opens " + nameof(ExamEmployeeCreate))]
    // TODO Description
    [Description("")]
    public async Task GivenExam_WhenCreateNewClicked_ThenEmployeeCreateOpens()
    {
        var ctx = await SetupTestContext();
        var renderedComponent = ctx.RenderComponent<Exercise_Exam>();
        await renderedComponent.Find($"#{Exercise_Exam.CreateButtonId}").ClickAsync(new MouseEventArgs());
        if (!renderedComponent.HasComponent<ExamEmployeeCreate>())
        {
            throw new TestRunException($"Clicking Employee Create button does not open {nameof(ExamEmployeeCreate)}!");
        }
    }
    
    [Title("Empty employee is bound to "+nameof(ExamEmployeeCreate))]
    // TODO Description
    [Description("")]
    public async Task GivenExam_WhenCreateNewClicked_ThenEmptyEmployeeBoundToEmployeeCreate()
    {
        var ctx = await SetupTestContext();
        var renderedComponent = ctx.RenderComponent<Exercise_Exam>();
        await renderedComponent.Find($"#{Exercise_Exam.CreateButtonId}").ClickAsync(new MouseEventArgs());
        if (!renderedComponent.HasComponent<ExamEmployeeCreate>())
        {
            throw new TestRunException($"Clicking Employee Create button does not open {nameof(ExamEmployeeCreate)}!");
        }

        var employee = renderedComponent.FindComponent<ExamEmployeeCreate>().Instance.Employee;
        if (employee != new ExamEmployee())
        {
            throw new TestRunException($"Employee bound to {nameof(ExamEmployeeCreate)} is not an empty employee! Bind an empty employee to it! (new Employee())");
        }
    }
    
    // On EmployeeEdit Close, List is refreshed
    [Title("On " + nameof(ExamEmployeeDetails)+"."+nameof(ExamEmployeeDetails.Closed) +" invoked, Employees are loaded")]
    // TODO Description
    [Description("")]
    public async Task GivenExam_WhenEmployeeDetailsCloseInvoked_ThenListIsRefreshed()
    {
        var ctx = await SetupTestContext();
        var renderedComponent = ctx.RenderComponent<Exercise_Exam>();
        var count = _employeeService.ReceivedCalls().Count(p => p.GetMethodInfo().Name == nameof(IExamEmployeeService.GetEmployees));
        await renderedComponent.Find($"#{Exercise_Exam.DetailsId(1)}").ClickAsync(new MouseEventArgs());
        var examEmployee = new ExamEmployee();
        await _employeeService.AddEmployee(examEmployee);
        var employeeDetails = renderedComponent.FindComponent<ExamEmployeeDetails>();
        await renderedComponent.InvokeAsync(async () => await employeeDetails.Instance.Closed.InvokeAsync());
        var secondCount = _employeeService.ReceivedCalls().Count(p => p.GetMethodInfo().Name == nameof(IExamEmployeeService.GetEmployees));
        if (secondCount != count + 1)
        {
            throw new TestRunException($"{nameof(IExamEmployeeService)}.{nameof(IExamEmployeeService.GetEmployees)} was not called after {nameof(ExamEmployeeDetails)}.+{nameof(ExamEmployeeDetails.Closed)} was invoked");
        }

        if (!renderedComponent.Instance.Employees.Contains(examEmployee))
        {
            throw new TestRunException($"Employees were not loaded! {nameof(Exercise_Exam.Employees)} does not contain new employee!");
        }
    }
    
    // ON EmployeeCreate Close, List is refreshed
    [Title("On " + nameof(ExamEmployeeDetails)+"."+nameof(ExamEmployeeDetails.Closed) +" invoked, Employees are loaded")]
    // TODO Description
    [Description("")]
    public async Task GivenExam_WhenEmployeeCreateCloseInvoked_ThenListIsRefreshed()
    {
        var ctx = await SetupTestContext();
        var renderedComponent = ctx.RenderComponent<Exercise_Exam>();
        var count = _employeeService.ReceivedCalls().Count(p => p.GetMethodInfo().Name == nameof(IExamEmployeeService.GetEmployees));
        await renderedComponent.Find($"#{Exercise_Exam.CreateButtonId}").ClickAsync(new MouseEventArgs());
        var employeeDetails = renderedComponent.FindComponent<ExamEmployeeCreate>();
        var examEmployee = new ExamEmployee();
        await _employeeService.AddEmployee(examEmployee);
        await renderedComponent.InvokeAsync(async () => await employeeDetails.Instance.Closed.InvokeAsync());
        var secondCount = _employeeService.ReceivedCalls().Count(p => p.GetMethodInfo().Name == nameof(IExamEmployeeService.GetEmployees));
        if (secondCount != count + 1)
        {
            throw new TestRunException($"{nameof(IExamEmployeeService)}.{nameof(IExamEmployeeService.GetEmployees)} was not called after {nameof(ExamEmployeeCreate)}.+{nameof(ExamEmployeeCreate.Closed)} was invoked");
        }
        
        
        if (!renderedComponent.Instance.Employees.Contains(examEmployee))
        {
            throw new TestRunException($"Employees were not loaded! {nameof(Exercise_Exam.Employees)} does not contain new employee!");
        }
    }
    
    //E2E tests
    //Create new employee
    //Edit employee
    //Delete employee
    public Test_Exam_PuttingItTogether(IJSRuntime jsRuntime) : base(jsRuntime)
    {
    }
}