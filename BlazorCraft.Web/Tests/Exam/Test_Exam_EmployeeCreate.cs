using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.Exam;
using BlazorCraft.Web.Tests.Routing;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests.Exam;

[TestForPage(typeof(Pages._11_Exam.Exam))]
public class Test_Exam_EmployeeCreate : ExamTestBase<ExamEmployeeCreate>
{
    protected override TestContext SetupTestContext()
    {
        var setupTestContext = base.SetupTestContext();
        setupTestContext.Services.AddExamEmployeeService();
        return setupTestContext;
    }

    //Preconditon: EmployeeForm is declared
    [ComponentUsedInMarkupTitle(typeof(EmployeeForm))]
    [ComponentUsedInMarkupDescription(typeof(EmployeeForm))]
    [Precondition]
    public async Task GivenEmployeeCreate_WhenRendered_ThenHasEmployeeFormDeclared()
    {
        var ctx = SetupTestContext();
        ExamEmployee employee = new ExamEmployee()
        {
            Id = 10,
        };
        var renderedComponent = ctx.RenderComponent<ExamEmployeeCreate>(
            builder => builder.Add(form => form.Employee, employee));
        var hasComponent = renderedComponent.HasComponent<EmployeeForm>();
        if (!hasComponent)
        {
            throw new TestRunException($"The component has no {nameof(ExamEmployeeCreate)} component declared");
        }
    }

    //PRe: Employee of the Form is the Employee set as parameter
    [Title("Employee is bound to " + nameof(EmployeeForm))]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeCreate_WhenRendered_ThenEmployeeFormEmployeeBoundToEmployeeCreateEmployee()
    {
        var ctx = SetupTestContext();

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

    // PRe: Employee form iseditmode = true
    [Title(nameof(EmployeeForm) + " is Editable on render")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeCreate_WhenRendered_ThenEmployeeFormIsEditable()
    {
        var ctx = SetupTestContext();

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
    //PRecondition: EmployeeService injected?

    // Valid employee on form -> Employee added, Closed invoked
    [Title("Employee added if employee is valid on " + nameof(EmployeeForm) + " then new employee is added to EmployeeService")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeCreate_WhenEmployeeFormValidEmployeeInvoked_ThenEmployeeAddedToEmployeeService()
    {
        var ctx = SetupTestContext();
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
        
        await renderedComponent.InvokeAsync(async() => await findComponent.Instance.EmployeeValid.InvokeAsync());
        try
        {
            var examEmployee = await employeeService.GetEmployee(employee.Id);
        }
        catch (KeyNotFoundException e)
        {
            throw new TestRunException("Employee was not added to EmployeeService!");
        }
        if (!isClosedInvoked)
        {
            throw new TestRunException($"{nameof(ExamEmployeeCreate.Closed)} is not invoked on {nameof(EmployeeForm)}.{nameof(EmployeeForm.EmployeeValid)}!");
        }
        
        
        
    }


    // Form canceled -> Closed invoked
    [Title("Closed event invoked if " + nameof(EmployeeForm) + " is cancelled")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeCreate_WhenEmployeeFormValidCancelInvoked_ThenClosedInvoked()
    {
        var ctx = SetupTestContext();
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
        await renderedComponent.InvokeAsync(async() => await findComponent.Instance.Cancel.InvokeAsync());
        if (!isClosedInvoked)
        {
            throw new TestRunException($"{nameof(ExamEmployeeCreate.Closed)} is not invoked on {nameof(EmployeeForm)}.{nameof(EmployeeForm.EmployeeValid)}!");
        }
        
    }
}