using System.Reflection;
using AngleSharp.Html;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.Exam;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NSubstitute;

namespace BlazorCraft.Web.Tests.Exam;

[TestForPage(typeof(Pages._11_Exam.Exam))]
public class Test_Exam_EmployeeDetails : ExamTestBase<ExamEmployeeDetails>
{
    private IExamEmployeeService _employeeService;
    private readonly IServiceProvider _serviceProvider;
    private Func<bool> GetEmployeeForEditCalled;
    private Func<bool> UpdateEmployeeCalled;

    public Test_Exam_EmployeeDetails(IJSRuntime jsRuntime, IServiceProvider serviceProvider) : base(jsRuntime)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task<TestContext> SetupTestContext()
    {
        var setupTestContext = await base.SetupTestContext();
        var examEmployeeService = new ExamEmployeeService().FillTestData();
        jsMethodCalls = new List<(string methodName, object[] args)>();
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
        _employeeService.UpdateEmployee(Arg.Any<ExamEmployee>()).ReturnsForAnyArgs(async ci => { await examEmployeeService.UpdateEmployee(ci.Arg<ExamEmployee>()); });
        _employeeService.DeleteEmployee(Arg.Any<ExamEmployee>()).ReturnsForAnyArgs(async ci => { await examEmployeeService.DeleteEmployee(ci.Arg<ExamEmployee>()); });
        setupTestContext.Services.AddScoped<IExamEmployeeService>((_) => _employeeService);
        GetEmployeeForEditCalled = () => _employeeService.ReceivedCalls().Any(p => p.GetMethodInfo().Name == nameof(IExamEmployeeService.GetEmployeeForEdit));
        UpdateEmployeeCalled = () => _employeeService.ReceivedCalls().Any(p => p.GetMethodInfo().Name == nameof(IExamEmployeeService.UpdateEmployee));
        return setupTestContext;
    }

    //GetEmployeeForEdit called

    [Title("EmployeeService.GetEmployeeForEdit is called upon rendering")]
    //TODO Description
    [Description("")]
    [Precondition]
    public async Task GivenEmployeeDetails_WhenRendered_ThenGetEmployeeForEditIsCalled()
    {
        var ctx = await SetupTestContext();
        ExamEmployee employee = new ExamEmployee()
        {
            Id = 1,
        };
        var renderedEmployeeDetails = await RenderedEmployeeDetails(ctx,
            builder => builder.Add(form => form.EmployeeId, employee.Id));
        if (!GetEmployeeForEditCalled())
        {
            throw new TestRunException("EmployeeService.GetEmployeeForEdit is NOT called upon rendering!");
        }
    }

    //[Precondition] EmployeeForm component is used in markup

    //Preconditon: EmployeeForm is declared

    [ComponentUsedInMarkupTitle(typeof(EmployeeForm))]
    //TODO Description, ami figyelmeztet, hogy csak akkor lesz jó, ha condition renderingnél is be van töltve minden
    [ComponentUsedInMarkupDescription(typeof(EmployeeForm))]
    [Precondition]
    public async Task GivenEmployeeDetails_WhenRendered_ThenHasEmployeeFormDeclared()
    {
        var ctx = await SetupTestContext();
        ExamEmployee employee = new ExamEmployee()
        {
            Id = 1,
        };
        var renderedEmployeeDetails = await RenderedEmployeeDetails(ctx,
            builder => builder.Add(form => form.EmployeeId, employee.Id));
        var hasComponent = renderedEmployeeDetails.HasComponent<EmployeeForm>();
        if (!hasComponent)
        {
            throw new TestRunException($"The component has no {nameof(EmployeeForm)} component declared");
        }
    }
    
    //UpdateImage is JSInvokeable
    [Title("UpdateImage is callable from javascript")]
    //TODO Description
    [Description("")]
    [Precondition]
    public async Task GivenExamEmployeeEdit_WhenRendered_ThenUpdateImageIsCallableFromJs()
    {
        ValidateMethodWithNameAndAttributeExists(Component, nameof(ExamEmployeeDetails.UpdateImage), typeof(JSInvokableAttribute));
    }

    // Employee is bound to EmployeeForm

    //PRe: Employee of the Form is the Employee set as parameter

    [Title(nameof(EmployeeForm) + " Employee is bound to " + nameof(IExamEmployeeService) + " Employee")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeDetails_WhenRendered_ThenEmployeeFormEmployeeBoundToEmployeeDetailsEmployee()
    {
        var ctx = await SetupTestContext();
        ExamEmployee employee = await _employeeService.GetEmployeeForEdit(1);
        var renderedEmployeeDetails = await RenderedEmployeeDetails(ctx,
            builder => builder.Add(form => form.EmployeeId, employee.Id));
        var findComponent = renderedEmployeeDetails.FindComponent<EmployeeForm>();
        var instanceEmployee = findComponent.Instance.Employee;
        if (employee != instanceEmployee)
        {
            throw new TestRunException($"Employee of the {nameof(EmployeeForm)} is not bound to the Employee of the {nameof(ExamEmployeeDetails)}");
        }
    }

    private async Task<IRenderedComponent<ExamEmployeeDetails>> RenderedEmployeeDetails(TestContext ctx, Action<ComponentParameterCollectionBuilder<ExamEmployeeDetails>>? parameterBuilder)
    {
        ExamEmployee employee;
        var renderedEmployeeDetails = ctx.RenderComponent<ExamEmployeeDetails>(
            parameterBuilder);
        renderedEmployeeDetails.SetParametersAndRender();
        await WaitForState(GetEmployeeForEditCalled);
        return renderedEmployeeDetails;
    }


    // EmployeeForm is NOT Editable on render

    // PRe: Employee form iseditmode = true

    [Title(nameof(EmployeeForm) + " is NOT Editable on render")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeDetails_WhenRendered_ThenEmployeeFormIsNotEditable()
    {
        var ctx = await SetupTestContext();

        ExamEmployee employee = new ExamEmployee()
        {
            Id = 1,
        };
        var renderedComponent = ctx.RenderComponent<ExamEmployeeDetails>(
            builder => builder.Add(form => form.EmployeeId, employee.Id));
        var findComponent = renderedComponent.FindComponent<EmployeeForm>();
        var isEditMode = findComponent.Instance.IsEditMode;
        if (isEditMode)
        {
            throw new TestRunException($"{nameof(EmployeeForm)} is editable after render!");
        }
    }

    // Edit switches editmode

    [Title("Clicking Edit button makes employee form editable")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeDetails_WhenEditButtonClick_ThenEmployeeFormIsEditable()
    {
        var ctx = await SetupTestContext();

        ExamEmployee employee = new ExamEmployee()
        {
            Id = 1,
        };
        var renderedComponent = await RenderedEmployeeDetails(ctx, builder => builder.Add(form => form.EmployeeId, employee.Id));
        await renderedComponent.Find($"#{ExamEmployeeDetails.EditButtonId}").ClickAsync(new MouseEventArgs());
        var employeeForm = renderedComponent.FindComponent<EmployeeForm>();
        if (!employeeForm.Instance.IsEditMode)
        {
            throw new TestRunException($"{nameof(EmployeeForm)} is not editable after clicking Edit button!");
        }
    }

    [Title("Clicking Edit button makes Edit button disabled")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeDetails_WhenEditButtonClick_ThenEditButtonDisabled()
    {
        var ctx = await SetupTestContext();

        ExamEmployee employee = new ExamEmployee() { Id = 1, };
        var renderedComponent = await RenderedEmployeeDetails(ctx, builder => builder.Add(form => form.EmployeeId, employee.Id));
        var editButton = renderedComponent.Find($"#{ExamEmployeeDetails.EditButtonId}");
        await editButton.ClickAsync(new MouseEventArgs());
        if (!editButton.HasAttribute("disabled"))
        {
            throw new TestRunException("Edit button is not disabled after clicking it!");
        }
    }

    // Employee added if employee is valid on EmployeeForm then new employee is added to EmployeeService

    // Edit switches editmode

    [Title("Saving employee form updates EmployeeService employee")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeDetails_()
    {
        var ctx = await SetupTestContext();
        var employee = await _employeeService.GetEmployee(1);
        var renderedComponent = ctx.RenderComponent<ExamEmployeeDetails>(
            builder => builder.Add(form => form.EmployeeId, employee.Id));
        var form = renderedComponent.FindComponent<EmployeeForm>();
        var newFirstName = "TestTest";
        form.Instance.Employee.FirstName = newFirstName;
        form.Render();
        await form.InvokeAsync(async () => await form.Instance.EmployeeChanged.InvokeAsync(form.Instance.Employee));
        form = renderedComponent.FindComponent<EmployeeForm>();
        await form.InvokeAsync(async () => await form.Instance.EmployeeValid.InvokeAsync());
        await WaitForState(UpdateEmployeeCalled);
        if (!UpdateEmployeeCalled())
        {
            throw new TestRunException("The employee was not modified in the EmployeeService!");
        }

        var modifiedEmployee = await _employeeService.GetEmployee(1);
        if (modifiedEmployee.FirstName != newFirstName)
        {
            throw new TestRunException("The EmployeeService.UpdateEmployee was called, however it was called with unchanged data!");
        }
    }

    //Closed event invoked if closed is pressed
    [Title("Closed event is invoked when pressing Close button")]
    //TODO Description
    [Description("")]
    public async Task GivenExamEmployeeEdit_WhenCloseButtonPressed_ThenCloseEventInvoked()
    {
        var ctx = await SetupTestContext();
        var employee = await _employeeService.GetEmployee(1);
        var closedInvoked = false;
        var renderedEmployeeDetails = ctx.RenderComponent<ExamEmployeeDetails>(
            builder =>
                builder.Add(form => form.EmployeeId, employee.Id)
                    .Add(employeeDetails => employeeDetails.Closed, EventCallback.Factory.Create(this, () => closedInvoked = true)));
        await renderedEmployeeDetails.Find($"#{ExamEmployeeDetails.CloseButtonId}").ClickAsync(new MouseEventArgs());
        await WaitForState(() => closedInvoked);
        if (!closedInvoked)
        {
            throw new TestRunException("Closed event was not called when pressing Close button!");
        }
    }

    // Picutre load invoked
    [Title("Profile picture is loaded exactly once")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeDetails_WhenRendered_ThenProfilePictureLoadedExactlyOnce()
    {
        var ctx = await SetupTestContext();
        var employee = await _employeeService.GetEmployee(1);
        var renderedEmployeeDetails = ctx.RenderComponent<ExamEmployeeDetails>(
            builder =>
                builder.Add(form => form.EmployeeId, employee.Id));
        renderedEmployeeDetails.SetParametersAndRender();
        renderedEmployeeDetails.SetParametersAndRender();
        var loadPictureCalls = jsMethodCalls.Where(p => p.methodName == "ExamHelper.fetchRandomPersonImage").ToList();
        if (loadPictureCalls.Count < 1)
        {
            throw new TestRunException("Profile picture was not loaded!");
        }

        if (loadPictureCalls.Count > 1)
        {
            throw new TestRunException("Profile picture was loaded multiple times! Hint: Are you calling it from the proper lifecycle method?");
        }
        
    }
    // Delete deletes employee

    [Title("Clicking Delete button deletes employee")]
    //TODO Description
    [Description("")]
    public async Task GivenExamEmployeeEdit_WhenDeleteButtonPressed_ThenEmployeeIsDeleted()
    {
        var ctx = await SetupTestContext();
        var employee = await _employeeService.GetEmployee(1);
        var renderedEmployeeDetails = ctx.RenderComponent<ExamEmployeeDetails>(
            builder =>
                builder.Add(form => form.EmployeeId, employee.Id));
        await renderedEmployeeDetails.Find($"#{ExamEmployeeDetails.DeleteButtonId}").ClickAsync(new MouseEventArgs());
        try
        {
            employee = await _employeeService.GetEmployee(1);
            throw new TestRunException("The employee was not deleted!");
        }
        catch (KeyNotFoundException)
        {
            // Success
        }
        
    }

    [Title("Clicking Delete button invokes Closed event")]
    //TODO Description
    [Description("")]
    public async Task GivenExamEmployeeEdit_WhenDeleteButtonPressed_ThenClosedEventInvoked()
    {
        var ctx = await SetupTestContext();
        var employee = await _employeeService.GetEmployee(1);
        var closedInvoked = false;
        var renderedEmployeeDetails = ctx.RenderComponent<ExamEmployeeDetails>(
            builder =>
                builder.Add(form => form.EmployeeId, employee.Id)
                    .Add(form => form.Closed, EventCallback.Factory.Create(this, () => closedInvoked = true)));
        await renderedEmployeeDetails.Find($"#{ExamEmployeeDetails.DeleteButtonId}").ClickAsync(new MouseEventArgs());
        await WaitForState(() => closedInvoked);
        if (!closedInvoked)
        {
            throw new TestRunException("Closed event was not invoked!");
        }

    }

    // Delete calls closed
}