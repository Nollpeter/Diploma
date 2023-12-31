﻿using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._11_Exam;
using BlazorCraft.Web.Shared._Exercises.Exam;
using Bunit;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using NSubstitute;

namespace BlazorCraft.Web.Tests._11_Exam;

[TestForPage(typeof(Pages._11_Exam.Exam))]
public class Test_Exam_PuttingItTogether : ExamTestBase<Exercise_Exam>
{
    private IExamEmployeeService _employeeService = null!;

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

    [Title("Employees are loaded after rendering component")]
    [Description("This test verifies that all employees are correctly loaded into the " + nameof(Exercise_Exam) + " component upon rendering. If any employee records are missing, it implies a problem in the data fetching or rendering logic.")]
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
            throw new TestRunException($"Not all employees were loaded! Missing employees are {string.Join(", ", missingEmployees.Select(p => p.Id))}");
        }
    }

    [Title("Employees are bound to Table")]
    [Description("This test ensures that the list of employees in the " + nameof(Exercise_Exam) + " component is correctly bound to the Items property of the " + nameof(MudTable<ExamEmployee>) +
                 ". A mismatch here would suggest potential data binding issues in the table.")]
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

    [Title("Table headers rendered correctly")]
    [Description("This test verifies that the headers of the " + nameof(MudTable<ExamEmployee>) + " within the " + nameof(Exercise_Exam) +
                 " component are rendered correctly. The expected headers are 'Id', 'First Name', 'Last Name', 'Position', 'Salary', and a button to 'Create new' employee. If any headers are missing or incorrect, it indicates a rendering issue.")]
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

    [Title("Table body rendered correctly")]
    [Description("This test ensures that the body of the " + nameof(MudTable<ExamEmployee>) + " within the " + nameof(Exercise_Exam) +
                 " component is rendered correctly. It checks each column of the table row corresponding to an employee's data, from 'Id' to 'Salary'. It also verifies the presence of a 'Details' button. Any discrepancy indicates a rendering issue.")]
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


    [Title("Clicking on details icon opens " + nameof(ExamEmployeeDetails))]
    [Description("This test checks that clicking on the 'Details' icon within the " + nameof(Exercise_Exam) + " component opens the " + nameof(ExamEmployeeDetails) +
                 " component. If not, it indicates a potential issue with the event handling of the 'Details' button.")]
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

    [Title(nameof(ExamEmployeeDetails) + " employee id is bound to employee id for which details was clicked")]
    [Description("This test verifies that in the " + nameof(ExamEmployeeDetails) + " component, the employee Id is correctly bound to the Id of the employee for whom the 'Details' icon was clicked in the " + nameof(Exercise_Exam) +
                 " component. A mismatch in Ids points to a binding problem.")]
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
    [Description("This test checks that clicking the 'Create new' button within the " + nameof(Exercise_Exam) + " component opens the " + nameof(ExamEmployeeCreate) +
                 " component. If not, it implies a possible mishandling of the 'Create new' button's event.")]
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

    [Title("Empty employee is bound to " + nameof(ExamEmployeeCreate))]
    [Description("This test verifies that when the 'Create new' button within the " + nameof(Exercise_Exam) + " component is clicked, an empty " + nameof(ExamEmployee) + " instance is correctly bound to the " + nameof(ExamEmployeeCreate) +
                 " component. Any other scenario suggests an error in the data binding mechanism.")]
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

    [Title("On " + nameof(ExamEmployeeDetails) + "." + nameof(ExamEmployeeDetails.Closed) + " invoked, Employees are loaded")]
    [Description("This test ensures that upon the invocation of the 'Closed' event in the " + nameof(ExamEmployeeDetails) + " component, the employees' list in the " + nameof(Exercise_Exam) +
                 " component is refreshed and reloaded. If the list doesn't refresh properly, it might indicate an issue with the event handling or data fetching mechanism.")]
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

    [Title("On " + nameof(ExamEmployeeDetails) + "." + nameof(ExamEmployeeDetails.Closed) + " invoked, Employees are loaded")]
    [Description("This test checks if the list of employees in the " + nameof(Exercise_Exam) + " component is refreshed, i.e., a call to " + nameof(IExamEmployeeService.GetEmployees) + " is made when the 'Closed' event of " +
                 nameof(ExamEmployeeDetails) + " component is invoked. It also checks if the newly added employee is present in the refreshed list.")]
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
}