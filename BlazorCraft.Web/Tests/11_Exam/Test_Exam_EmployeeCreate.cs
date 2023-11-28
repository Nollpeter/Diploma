using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._11_Exam;
using BlazorCraft.Web.Shared._Exercises.Exam;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCraft.Web.Tests._11_Exam;

[TestForPage(typeof(Pages._11_Exam.Exam))]
public class Test_Exam_EmployeeCreate : ExamTestBase<ExamEmployeeCreate>
{
    protected override async Task<TestContext> SetupTestContext()
    {
        var setupTestContext = await base.SetupTestContext();
        setupTestContext.Services.AddExamEmployeeService();
        return setupTestContext;
    }

    [ComponentUsedInMarkupTitle(typeof(EmployeeForm))]
    [ComponentUsedInMarkupDescription(typeof(EmployeeForm))]
    [Precondition]
    public async Task GivenEmployeeCreate_WhenRendered_ThenHasEmployeeFormDeclared()
    {
        var ctx = await SetupTestContext();
        ExamEmployee employee = new ExamEmployee()
        {
            Id = 10,
        };
        var renderedComponent = ctx.RenderComponent<ExamEmployeeCreate>(
            builder => builder.Add(form => form.Employee, employee));
        var hasComponent = renderedComponent.HasComponent<EmployeeForm>();
        if (!hasComponent)
        {
            throw new TestRunException($"The component has no {nameof(EmployeeForm)} component declared");
        }
    }

    [Title("Employee is bound to " + nameof(EmployeeForm))]
    [Description("This test verifies that the employee object is correctly bound to the " + nameof(EmployeeForm) + " within the " + nameof(ExamEmployeeCreate) + " component. It ensures that the same employee instance used in " +
                 nameof(ExamEmployeeCreate) + " is also present in the " + nameof(EmployeeForm) + ", by checking for reference equality.")]
    public async Task GivenEmployeeCreate_WhenRendered_ThenEmployeeFormEmployeeBoundToEmployeeCreateEmployee()
    {
        var ctx = await SetupTestContext();

        ExamEmployee employee = new ExamEmployee()
        {
            Id = 10,
        };
        var renderedComponent = ctx.RenderComponent<ExamEmployeeCreate>(
            builder => builder.Add(form => form.Employee, employee));
        var findComponent = renderedComponent.FindComponent<EmployeeForm>();
        var instanceEmployee = findComponent.Instance.Employee;
        if (!ReferenceEquals(employee, instanceEmployee))
        {
            throw new TestRunException($"Employee of the {nameof(EmployeeForm)} is not bound to the Employee of the {nameof(ExamEmployeeCreate)}");
        }
    }

    [Title(nameof(EmployeeForm) + " is Editable on render")]
    [Description("This test verifies that the " + nameof(EmployeeForm) + " component is editable upon rendering within the " + nameof(ExamEmployeeCreate) + " component. The test sets up the " + nameof(EmployeeForm) +
                 " with a predefined employee and checks if the form's edit mode is enabled by default. If the form is not in edit mode, the test fails, indicating an issue with the initial state of the " + nameof(EmployeeForm) + " when used in " +
                 nameof(ExamEmployeeCreate) + ".")]
    public async Task GivenEmployeeCreate_WhenRendered_ThenEmployeeFormIsEditable()
    {
        var ctx = await SetupTestContext();

        ExamEmployee employee = new ExamEmployee()
        {
            Id = 10,
        };
        var renderedComponent = ctx.RenderComponent<ExamEmployeeCreate>(
            builder => builder.Add(form => form.Employee, employee));
        var findComponent = renderedComponent.FindComponent<EmployeeForm>();
        var isEditMode = findComponent.Instance.IsEditMode;
        if (!isEditMode)
        {
            throw new TestRunException($"{nameof(EmployeeForm)} is not editable after render!");
        }
    }

    //TODO PRecondition: EmployeeService injected?


    [Title("Employee added if employee is valid on " + nameof(EmployeeForm) + " then new employee is added to EmployeeService")]
    [Description("This test verifies that when an employee is valid on " + nameof(EmployeeForm) + " then a new employee is added to " + nameof(IExamEmployeeService) + " and the Closed event of " + nameof(ExamEmployeeCreate) + " is invoked.")]
    public async Task GivenEmployeeCreate_WhenEmployeeFormValidEmployeeInvoked_ThenEmployeeAddedToEmployeeService()
    {
        var ctx = await SetupTestContext();
        ExamEmployee employee = new ExamEmployee()
        {
            Id = 10,
        };
        var isClosedInvoked = false;

        var renderedComponent = ctx.RenderComponent<ExamEmployeeCreate>(
            builder => builder
                .Add(form => form.Employee, employee)
                .Add(form => form.Closed, EventCallback.Factory.Create(this, () => { isClosedInvoked = true; })));
        var employeeService = ctx.Services.GetRequiredService<IExamEmployeeService>();
        var findComponent = renderedComponent.FindComponent<EmployeeForm>();

        await renderedComponent.InvokeAsync(async () => await findComponent.Instance.EmployeeValid.InvokeAsync());
        try
		{
			await employeeService.GetEmployee(employee.Id);
		}
        catch (KeyNotFoundException)
        {
            throw new TestRunException("Employee was not added to EmployeeService!");
        }

        if (!isClosedInvoked)
        {
            throw new TestRunException($"{nameof(ExamEmployeeCreate.Closed)} is not invoked on {nameof(EmployeeForm)}.{nameof(EmployeeForm.EmployeeValid)}!");
        }
    }


    [Title("Closed event invoked if " + nameof(EmployeeForm) + " is cancelled")]
    [Description("This test verifies that the Closed event is invoked when the Cancel method on " + nameof(EmployeeForm) + " is executed in " + nameof(ExamEmployeeCreate) + ".")]
    public async Task GivenEmployeeCreate_WhenEmployeeFormValidCancelInvoked_ThenClosedInvoked()
    {
        var ctx = await SetupTestContext();
        ExamEmployee employee = new ExamEmployee()
        {
            Id = 10,
        };
        var isClosedInvoked = false;

        var renderedComponent = ctx.RenderComponent<ExamEmployeeCreate>(
            builder => builder
                .Add(form => form.Employee, employee)
                .Add(form => form.Closed, EventCallback.Factory.Create(this, () => { isClosedInvoked = true; })));
        var findComponent = renderedComponent.FindComponent<EmployeeForm>();
        await renderedComponent.InvokeAsync(async () => await findComponent.Instance.Cancel.InvokeAsync());
        if (!isClosedInvoked)
        {
            throw new TestRunException($"{nameof(ExamEmployeeCreate.Closed)} is not invoked on {nameof(EmployeeForm)}.{nameof(EmployeeForm.EmployeeValid)}!");
        }
    }
}