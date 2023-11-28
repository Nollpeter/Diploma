using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._11_Exam;
using BlazorCraft.Web.Shared._Exercises.Exam;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NSubstitute;

namespace BlazorCraft.Web.Tests._11_Exam;

[TestForPage(typeof(Pages._11_Exam.Exam))]
public class Test_Exam_EmployeeDetails : ExamTestBase<ExamEmployeeDetails>
{
    private IExamEmployeeService _employeeService = null!;
	private Func<bool> GetEmployeeForEditCalled = null!;
	private Func<bool> UpdateEmployeeCalled = null!;

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

    [Title("EmployeeService.GetEmployeeForEdit is called upon rendering")]
    [Description("This test verifies that the " + nameof(IExamEmployeeService) + "." + nameof(IExamEmployeeService.GetEmployeeForEdit) + " method is invoked when the Employee Details component is rendered. " +
                 "It ensures that employee data is fetched and displayed correctly as part of the component's initialization process.")]
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

    [Title("UpdateImage is callable from javascript")]
    //TODO Description
    [Description("This test verifies that the " + nameof(ExamEmployeeDetails) + "." + nameof(ExamEmployeeDetails.UpdateImage) + " method is marked with the " + nameof(JSInvokableAttribute) +
                 ", ensuring that it can be called from JavaScript in the context of the ExamEmployeeEdit component.")]
    [Precondition]
    public Task GivenExamEmployeeEdit_WhenRendered_ThenUpdateImageIsCallableFromJs()
	{
		ValidateMethodWithNameAndAttributeExists(Component, nameof(ExamEmployeeDetails.UpdateImage), typeof(JSInvokableAttribute));
		return Task.CompletedTask;
	}

    [Title(nameof(EmployeeForm) + " Employee is bound to " + nameof(IExamEmployeeService) + " Employee")]
    [Description(
        "This test verifies that the Employee instance within the " + nameof(EmployeeForm) + " is correctly bound to the Employee object managed by the " + nameof(IExamEmployeeService) + ". It ensures consistent data representation across the " +
        nameof(EmployeeForm) + " and " + nameof(ExamEmployeeDetails) + " components.")]
    [Precondition]
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
        var renderedEmployeeDetails = ctx.RenderComponent<ExamEmployeeDetails>(
            parameterBuilder);
        renderedEmployeeDetails.SetParametersAndRender();
        await WaitForState(GetEmployeeForEditCalled);
        return renderedEmployeeDetails;
    }

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

    [Title("Clicking Edit button makes employee form editable")]
    [Description(
        "This test verifies that the " + nameof(EmployeeForm) + " is in a non-editable state immediately after rendering. It checks that the IsEditMode property of the " + nameof(EmployeeForm) +
        " instance is false, ensuring the form remains read-only until explicitly enabled for editing.")]
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
    [Description("This test verifies that clicking the Edit button in the " + nameof(ExamEmployeeDetails) +
                 " component disables the button. It ensures the button's state changes to 'disabled' after being clicked, preventing further edits until the current editing process is completed.")]
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

    [Title("Saving employee form updates EmployeeService employee")]
    [Description("This test verifies that saving changes in the " + nameof(EmployeeForm) + " component within " + nameof(ExamEmployeeDetails) + " correctly updates the employee in the " + nameof(IExamEmployeeService) +
                 ". It ensures that modifications made to the employee's details are accurately reflected in the EmployeeService after the save operation is performed.")]
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

    [Title("Closed event is invoked when pressing Close button")]
    [Description("This test verifies that the closed event is triggered when the Close button is pressed in the " + nameof(ExamEmployeeDetails) +
                 " component. It ensures that the appropriate event is invoked, reflecting the expected behavior when a user interacts with the Close button.")]
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

    [Title("Profile picture is loaded exactly once")]
    [Description("This test verifies that the profile picture in the " + nameof(ExamEmployeeDetails) +
                 " component is loaded exactly once when the component is rendered. It checks the number of calls made to the method responsible for fetching the profile picture, ensuring that it is invoked only once regardless of multiple render cycles, thus preventing unnecessary network requests or performance issues.")]
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
    [Description("This test verifies that clicking the Delete button in the " + nameof(ExamEmployeeDetails) + " component successfully deletes the employee from the " + nameof(IExamEmployeeService) +
                 ". It simulates a button click and then attempts to retrieve the same employee, expecting a KeyNotFoundException to confirm that the employee no longer exists in the service.")]
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
}