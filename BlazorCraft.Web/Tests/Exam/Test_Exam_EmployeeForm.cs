using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Pages._11_Exam;
using BlazorCraft.Web.Shared._Exercises.Exam;
using BlazorCraft.Web.Shared.UiHelpers;
using BlazorCraft.Web.Tests.Routing;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;
using NSubstitute;

namespace BlazorCraft.Web.Tests.Exam;

[TestForPage(typeof(Pages._11_Exam.Exam))]
public class Test_Exam_EmployeeForm : ComponentTestBase<EmployeeForm>
{
    public interface IFormFieldDeclaration<TComponentType> where TComponentType : IComponent
    {
        public string Label { get; }
        public Type Type => typeof(TComponentType);
        public string RequiredError => $"The {GetType().Name} field is required.";

    }
    public class Id : IFormFieldDeclaration<MudNumericField<int>>
    {
        public string Label => "Id";
    }
    public class FirstName : IFormFieldDeclaration<MudTextField<string?>>
    {
        public string Label => "First Name";
    }
    
    public class LastName : IFormFieldDeclaration<MudTextField<string?>>
    {
        public string Label => "Last Name";
    }
    
    public class Position : IFormFieldDeclaration<MudEnumSelect<BlazorCraft.Web.Shared._Exercises.Exam.Position>>
    {
        public string Label => "Position";
    }
    
    public class Gender : IFormFieldDeclaration<MudEnumSelect<BlazorCraft.Web.Shared._Exercises.Exam.Gender>>
    {
        public string Label => "Gender";
    }
    
    public class BirthDate : IFormFieldDeclaration<MudDatePicker>
    {
        public string Label => "Birth Date";
    }
    
    public class Salary : IFormFieldDeclaration<MudNumericField<int?>>
    {
        public string Label => "Salary";
    }
    
    public class Address : IFormFieldDeclaration<MudTextField<string?>>
    {
        public string Label => "Address";
    }
    
    public class HireDate : IFormFieldDeclaration<MudDatePicker>
    {
        public string Label => "Hire Date";
    }
    private class TestRunContext
    {
        public static Id Id => new();
        public static FirstName FirstName => new();
        public static LastName LastName => new();
        public static Position Position => new();
        public static Gender Gender => new();
        public static BirthDate BirthDate => new();
        public static Salary Salary => new();
        public static Address Address => new();
        public static HireDate HireDate => new();
    }
    #region Preconditions
    
    //DataAnnotationsValidatorUsed
    [ComponentUsedInMarkupTitle(typeof(DataAnnotationsValidator))]
    [ComponentUsedInMarkupDescription(typeof(DataAnnotationsValidator))]
    [Precondition]
    public async Task DataAnnotationsValidatorUsed()
    {
        ValidateComponentUsage(Component, typeof(DataAnnotationsValidator));
    }
    //Editform used
    [ComponentUsedInMarkupTitle(typeof(EditForm))]
    [ComponentUsedInMarkupDescription(typeof(EditForm))]
    [Precondition]
    public async Task EditFormUsed()
    {
        ValidateComponentUsage(Component, typeof(EditForm));
    }
    private void ValidateDeclaredField<TComponentType>(IFormFieldDeclaration<TComponentType> field) where TComponentType : IComponent
    {
        var ctx = SetupTestContext();
        var renderedComponent = ctx.RenderComponent<EmployeeForm>(builder => builder.Add(form => form.Employee, new ExamEmployee()));
        GetFormField(renderedComponent, field);
    }

    private IRenderedComponent<TComponentType> GetFormField<TComponentType>(IRenderedComponent<EmployeeForm> form ,IFormFieldDeclaration<TComponentType> field) where TComponentType : IComponent
    {
        var components = form.FindComponents<TComponentType>();

        string GetLabel(object component)
        {
            var propertyInfo = component.GetType().GetProperties().FirstOrDefault(p => p.Name == nameof(MudField.Label));
            var label = propertyInfo.GetValue(component).ToString();
            return label;
        }

        var firstOrDefault = components.FirstOrDefault(p => p.GetLabel() == field.Label);
        if (firstOrDefault == null)
        {
            throw new TestRunException($"Could not find Component of type {typeof(TComponentType)} with label {field.Label} inside the Employee form");
        }

        return firstOrDefault;
    }
    
    // Field for ID
    [Title("Field for Id defined")]
    [Description("")]
    [Precondition]
    public async Task FieldForIdDefined()
    {
        ValidateDeclaredField(TestRunContext.Id);
    }
    
    
    // FIeld for FIrstname
    [Title("Field for FirstName defined")]
    [Description("")]
    [Precondition]
    public async Task FieldForFirstNameDefined()
    {
        ValidateDeclaredField(TestRunContext.FirstName);
    }
    
    // Field for last name
    [Title("Field for LastName defined")]
    [Description("")]
    [Precondition]
    public async Task FieldForLastNameDefined()
    {
        ValidateDeclaredField(TestRunContext.LastName);
    }

    // field for Position
    [Title("Field for Position defined")]
    [Description("")]
    [Precondition]
    public async Task FieldForPositionDefined()
    {
        ValidateDeclaredField(TestRunContext.Position);
    }

    // field for Gender
    [Title("Field for Gender defined")]
    [Description("")]
    [Precondition]
    public async Task FieldForGenderDefined()
    {
        ValidateDeclaredField(TestRunContext.Gender);
    }

    // field for BirthDate
    [Title("Field for BirthDate defined")]
    [Description("")]
    [Precondition]
    public async Task FieldForBirthDateDefined()
    {
        ValidateDeclaredField(TestRunContext.BirthDate);
    }

    // field for Salaray
    [Title("Field for Salary defined")]
    [Description("")]
    [Precondition]
    public async Task FieldForSalaryDefined()
    {
        ValidateDeclaredField(TestRunContext.Salary);
    }

    //Field for address
    [Title("Field for Address defined")]
    [Description("")]
    [Precondition]
    public async Task FieldForAddressDefined()
    {
        ValidateDeclaredField(TestRunContext.Address);
    }

    // Field for Hire date
    [Title("Field for HireDate defined")]
    [Description("")]
    [Precondition]
    public async Task FieldForHireDateDefined()
    {
        ValidateDeclaredField(TestRunContext.HireDate);
    }
    #endregion

    protected TestContext SetupTestContext()
    {
        TestContext ctx = new TestContext();
        ctx.Services.AddMudServices();
        ctx.JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        ctx.JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        return ctx;
    }
    
    //Iseditmode = false -> All fields disabled
    [Title("All fields disabled if EditMode = false")]
    // TODO Description
    [Description("")]
    public async Task GivenEmployeeForm_WhenEditModeFalse_ThenAllFieldsDisabled()
    {
        var testContext = SetupTestContext();
        var form = testContext.RenderComponent<EmployeeForm>(builder => 
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, false));

        ValidateDisabled(GetFormField(form, TestRunContext.Id),true);
        ValidateDisabled(GetFormField(form, TestRunContext.FirstName),true);
        ValidateDisabled(GetFormField(form, TestRunContext.LastName),true);
        ValidateDisabled(GetFormField(form, TestRunContext.Position),true);
        ValidateDisabled(GetFormField(form, TestRunContext.Gender),true);
        ValidateDisabled(GetFormField(form, TestRunContext.BirthDate),true);
        ValidateDisabled(GetFormField(form, TestRunContext.Salary),true);
        ValidateDisabled(GetFormField(form, TestRunContext.Address),true);
        ValidateDisabled(GetFormField(form, TestRunContext.HireDate),true);
    }
    
    void ValidateDisabled<TComponent>(IRenderedComponent<TComponent> component, bool expectedState) where TComponent : IComponent
    {
        if (component.GetDisabled() != expectedState)
        {
            throw new TestRunException($"The form field's Disabled property is expected to be {expectedState}, but it is {component.GetDisabled()}. " +
                                       $"Have you bound its \"Disabled\" property properly according to the exercise description??");
        }
    }
    
    //iseditmode = false -> Buttons hidden
    [Title("Edit and Cancel buttons are hidden if EditMode = false")]
    public async Task GivenEmployeeForm_WhenEditMOdeFalse_ThenEditAndCancelButtonsHidden()
    {
        var testContext = SetupTestContext();
        var form = testContext.RenderComponent<EmployeeForm>(builder => 
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, false));

        var buttons = 
            form.FindComponents<MudButton>()
                .Where(p => p.FindAll("#save").Any() || p.FindAll("#cancel").Any());
        if (buttons.Any())
        {
            throw new TestRunException("Buttons are displayed with EditMode = false!");
        }
    }
    
    //iseditmode = true -> Id disabled
    [Title("ID disabled if EditMode = true")]
    // TODO Description
    [Description("")]
    public async Task GivenEmployeeForm_WhenEditModeTrue_ThenIdDisabled()
    {
        var testContext = SetupTestContext();
        var form = testContext.RenderComponent<EmployeeForm>(builder => 
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, true));

        ValidateDisabled(GetFormField(form, TestRunContext.Id),true);
    }
    
    //iseditmode = true -> other fields enabled
    [Title("Rest of the fields NOT Disabled if EditMode = true")]
    // TODO Description
    [Description("")]
    public async Task GivenEmployeeForm_WhenEditModeTrue_ThenOtherFieldsNotDisabled()
    {
        var testContext = SetupTestContext();
        var form = testContext.RenderComponent<EmployeeForm>(builder => 
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, true));

        ValidateDisabled(GetFormField(form, TestRunContext.FirstName),false);
        ValidateDisabled(GetFormField(form, TestRunContext.LastName),false);
        ValidateDisabled(GetFormField(form, TestRunContext.Position),false);
        ValidateDisabled(GetFormField(form, TestRunContext.Gender),false);
        ValidateDisabled(GetFormField(form, TestRunContext.BirthDate),false);
        ValidateDisabled(GetFormField(form, TestRunContext.Salary),false);
        ValidateDisabled(GetFormField(form, TestRunContext.Address),false);
        ValidateDisabled(GetFormField(form, TestRunContext.HireDate),false);
    }
    
    //iseditmode = true -> buttons displayed
    [Title("Edit and Cancel buttons are displayed if EditMode = true")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeForm_WhenEditModeTrue_ThenEditAndCancelButtonsDisplayed()
    {
        
        var testContext = SetupTestContext();
        var form = testContext.RenderComponent<EmployeeForm>(builder => 
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, true));

        var buttons = 
            form.FindComponents<MudButton>()
                .Where(p => p.FindAll("#save").Any() || p.FindAll("#cancel").Any());;
        if (!buttons.Any())
        {
            throw new TestRunException("Buttons are hidden with EditMode = true!");
        }
        
        var cancel = buttons.FirstOrDefault(p => p.FindAll("#cancel").Any());
        if (cancel == null)
        {
            throw new TestRunException("Cancel button is not displayed when IsEditMode = true!");
        }
    }
    
    // Cancelbutton -> cancel event called
    [Title("Clicking cancel invokes Cancel Event")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeForm_WhenCancelClicked_ThenCancelEventInvoked()
    {
        await GivenEmployeeForm_WhenEditModeTrue_ThenEditAndCancelButtonsDisplayed();

        
        var testContext = SetupTestContext();
        var isCancelInvoked = false;
        var form = testContext.RenderComponent<EmployeeForm>(builder => 
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, true)
                .Add(form => form.Cancel, EventCallback.Factory.Create(this, () => isCancelInvoked = true)));

        var buttons = form.FindComponents<MudButton>();
        var cancelButton = buttons.FirstOrDefault(p => p.FindAll("#cancel").Any());
        await cancelButton.Find("#cancel").ClickAsync(new MouseEventArgs());
        if (!isCancelInvoked)
        {
            throw new TestRunException("Cancel event was not invoked when clicking Cancel button!");
        }
    }
    
    //fields are bound to employee
    [Title("Form fields are bound to Employee")]
    //TODO description 
    [Description("")]
    public async Task GivenEmployeeForm_WhenCreated_ThenFormFieldsAreBoundToEmployeeObject()
    {
        var testContext = SetupTestContext();
        var employee = new ExamEmployee() {Id = 10};
        var form = testContext.RenderComponent<EmployeeForm>(builder =>
        {
            builder.Add(form => form.Employee, employee)
                .Add(form => form.IsEditMode, true);
        });

       
        async Task ValidateDataBinding<TComponent,TValue>( IRenderedComponent<EmployeeForm> form, IFormFieldDeclaration<TComponent> field, 
            Action<TValue> setValueOfEmployee,
            Func<TValue> getValueOfEmployee,
            TValue value1, TValue value2) where TComponent : IComponent 
        {
            var formComponent = GetFormField(form, field);
            var eventCallback = formComponent.GetValueChanged<TComponent,TValue>();
            await form.InvokeAsync(async () => await eventCallback.InvokeAsync(value1));
            var valueOfEmployee = getValueOfEmployee();
            if (!valueOfEmployee.Equals(value1))
            {
                throw new TestRunException($"Data binding does not work in the direction of form input -> employee for field {field.Label}");
            }

            setValueOfEmployee(value2);
            
            form.SetParametersAndRender(builder => builder.Add(form => form.Employee, employee));
            var value = formComponent.GetValue<TComponent, TValue>();
            if (!value2.Equals(value))
            {
                throw new TestRunException($"Data binding does not work in the direction of employee -> form input for field {field.Label}");
            }
        }

        await ValidateDataBinding(form, TestRunContext.FirstName, (value) => employee.FirstName = value, () => employee.FirstName, "Test1", "Test2");
        await ValidateDataBinding(form, TestRunContext.LastName, (value) => employee.LastName = value, () => employee.LastName, "Test1", "Test2");
        await ValidateDataBinding(form, TestRunContext.Salary, (value) => employee.Salary = value, () => employee.Salary, 20000, 35000);
        await ValidateDataBinding(form, TestRunContext.BirthDate, (value) => employee.BirthDate =value, () => employee.BirthDate, DateTime.Now, DateTime.MaxValue);
        await ValidateDataBinding(form, TestRunContext.Address, (value) => employee.Address = value, () => employee.Address, "Test1", "Test2");
        await ValidateDataBinding(form, TestRunContext.HireDate, (value) => employee.HireDate =value, () => employee.HireDate, DateTime.Now, DateTime.MaxValue);
        
        await ValidateDataBinding(form, TestRunContext.Position, (value) => employee.Position = value, () => employee.Position, Shared._Exercises.Exam.Position.CEO, Shared._Exercises.Exam.Position.CFO);
        await ValidateDataBinding(form, TestRunContext.Gender, (value) => employee.Gender = value, () => employee.Gender, Shared._Exercises.Exam.Gender.Male, Shared._Exercises.Exam.Gender.Female);
    }
    
    // invalid fields -> Save -> Errors
    [Title("Entering invalid fields -> Error displayed for fields")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeForm_WhenInvalidFieldsEntered_ThenErrorMessageForFields()
    {
        var testContext = SetupTestContext();
        var employee = new ExamEmployee() {Id = 10};
        var employeeValidInvoked = false;
        var form = testContext.RenderComponent<EmployeeForm>(builder =>
        {
            builder.Add(form => form.Employee, employee)
                .Add(form => form.IsEditMode, true)
                .Add(form => form.EmployeeValid, EventCallback.Factory.Create(this, () => employeeValidInvoked = true));
        });

        await form.Find("#save").ClickAsync(new MouseEventArgs());

        void ValidateRequiredError<TComponent>(IFormFieldDeclaration<TComponent> field) where TComponent : IComponent
        {
            var fieldComponent = GetFormField(form, field);
            if (fieldComponent.GetErrorText() != field.RequiredError)
            {
                throw new TestRunException($"Missing required error for field {field.Label} while saving invalid form! " +
                                           $"Have you annotated the property with the proper Validation attribute?");
            }
        }
        ValidateRequiredError(TestRunContext.FirstName);
        ValidateRequiredError(TestRunContext.LastName);
        ValidateRequiredError(TestRunContext.Position);
        ValidateRequiredError(TestRunContext.Gender);
        ValidateRequiredError(TestRunContext.BirthDate);
        ValidateRequiredError(TestRunContext.Salary);
        ValidateRequiredError(TestRunContext.HireDate);

        if (employeeValidInvoked)
        {
            throw new TestRunException($"{nameof(EmployeeForm.EmployeeValid)} is invoked with invalid Employee!");
        }
        
        employee.Salary = 100_000;
        form.SetParametersAndRender(builder => builder.Add(form => form.Employee, employee)
            .Add(form => form.IsEditMode, true));
        await form.Find("#save").ClickAsync(new MouseEventArgs());
        
        var fieldComponent = GetFormField(form, TestRunContext.Salary);
        if (fieldComponent.Instance.ErrorText != $"The field {nameof(ExamEmployee.Salary)} must be between 10000 and 50000.")
        {
            throw new TestRunException($"Missing range error for field {nameof(ExamEmployee.Salary)} while saving invalid form! " +
                                       $"Have you annotated the property with the proper Validation attribute?");
        }
    }
    // valid fields -> Save -> EmployeeValid called
    // invalid fields -> Save -> Errors
    [Title("Entering valid fields -> EmployeeValid is invoked")]
    //TODO Description
    [Description("")]
    public async Task GivenEmployeeForm_WhenFieldsEnteredAndSaveClicked_ThenValidEmployeeInvoked()
    {
        var testContext = SetupTestContext();
        var employee = new ExamEmployee()
        {
            Id = 10,
            Salary = 35000,
            Address = "My address",
            Gender = Shared._Exercises.Exam.Gender.Female,
            BirthDate = DateTime.Today,
            Position = Shared._Exercises.Exam.Position.Developer,
            LastName = "Test",
            FirstName = "Test",
            HireDate = DateTime.MinValue,
        };
        var employeeValidInvoked = false;
        var form = testContext.RenderComponent<EmployeeForm>(builder =>
        {
            builder.Add(form => form.Employee, employee)
                .Add(form => form.IsEditMode, true)
                .Add(form => form.EmployeeValid, EventCallback.Factory.Create(this, () => employeeValidInvoked = true));
        });

        await form.Find("#save").ClickAsync(new MouseEventArgs());

        void ValidateNoErrors<TComponent>(IFormFieldDeclaration<TComponent> field) where TComponent : IComponent
        {
            var fieldComponent = GetFormField(form, field);
            var errorText = fieldComponent.GetErrorText();
            if (!string.IsNullOrEmpty(errorText))
            {
                throw new TestRunException($"Missing required error for field {field.Label} while saving valid employee: \n" +
                                           errorText);
            }
        }
        ValidateNoErrors(TestRunContext.Id);
        ValidateNoErrors(TestRunContext.FirstName);
        ValidateNoErrors(TestRunContext.LastName);
        ValidateNoErrors(TestRunContext.Position);
        ValidateNoErrors(TestRunContext.Gender);
        ValidateNoErrors(TestRunContext.BirthDate);
        ValidateNoErrors(TestRunContext.Salary);
        ValidateNoErrors(TestRunContext.Address);
        ValidateNoErrors(TestRunContext.HireDate);

        if (!employeeValidInvoked)
        {
            throw new TestRunException($"{nameof(EmployeeForm.EmployeeValid)} is NOT invoked with valid Employee!");
        }
        
    }
}

public static class FormFieldExtensions
{
    public static string GetLabel<T>(this IRenderedComponent<T> component) where T : IComponent
    {
        var propertyInfo = component.Instance.GetType().GetProperties().FirstOrDefault(p => p.Name == nameof(MudField.Label));
        var label = propertyInfo.GetValue(component.Instance).ToString();
        return label;
    }

    public static bool GetDisabled<T>(this IRenderedComponent<T> component) where T : IComponent
    {
        var propertyInfo = component.Instance.GetType().GetProperties().FirstOrDefault(p => p.Name == nameof(MudField.Disabled));
        var disabled = (bool)propertyInfo.GetValue(component.Instance);
        return disabled;
    }

    public static EventCallback<TValue> GetValueChanged<TComponent, TValue>(this IRenderedComponent<TComponent> component) where TComponent : IComponent
    {
        
        var propertyInfo = component.Instance.GetType().GetProperties().FirstOrDefault(p =>
        {
            var valueChangedName = component.Instance is MudDatePicker ? nameof(MudDatePicker.DateChanged) : nameof(MudTextField<string>.ValueChanged);
            return p.Name == valueChangedName;
        });
        var valueChanged = (EventCallback<TValue>)propertyInfo.GetValue(component.Instance);
        return valueChanged;
    }
    public static TValue GetValue<TComponent, TValue>(this IRenderedComponent<TComponent> component) where TComponent : IComponent
    {
        var propertyInfo = component.Instance.GetType().GetProperties().FirstOrDefault(p =>
        {
            var valueName = component.Instance is MudDatePicker ? nameof(MudDatePicker.Date) : nameof(MudTextField<string>.Value);
            return p.Name == valueName;
        });
        var valueChanged = (TValue)propertyInfo.GetValue(component.Instance);
        return valueChanged;
    }
    
    public static string GetErrorText<TComponent>(this IRenderedComponent<TComponent> component) where TComponent : IComponent
    {
        var propertyInfo = component.Instance.GetType().GetProperties().FirstOrDefault(p =>
        {
            var valueName = nameof(MudTextField<string>.ErrorText);
            return p.Name == valueName;
        });
        return propertyInfo.GetValue(component.Instance)?.ToString();
    }
    
}
