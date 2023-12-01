using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._11_Exam;
using BlazorCraft.Web.Shared._Exercises.Exam;
using BlazorCraft.Web.Shared.UiHelpers.MudComponents;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace BlazorCraft.Web.Tests._11_Exam;

[TestForPage(typeof(Pages._11_Exam.Exam))]
public class Test_Exam_EmployeeForm : ExamTestBase<ExamEmployeeForm>
{
    public interface IFormFieldDeclaration<TComponentType, TValue> where TComponentType : IComponent
    {
        public string Label { get; }
        public Type Type => typeof(TComponentType);
        public string RequiredError => $"The {GetType().Name} field is required.";
        public TComponentType Instance { get; set; }
        public TValue Value { get; set; }

        public IRenderedComponent<TComponentType> RenderedComponent { get; protected set; }

        public void Initialize(IRenderedComponent<ExamEmployeeForm> form)
        {
            var components = form.FindComponents<TComponentType>();

            var firstOrDefault = components.FirstOrDefault(p => p.GetLabel() == Label);

            RenderedComponent = firstOrDefault ?? throw new TestRunException($"Could not find Component of type {typeof(TComponentType)} with label {Label} inside the Employee form");
            Instance = firstOrDefault.Instance;
        }
    }

    public abstract class NumericFormField<T> : IFormFieldDeclaration<MudNumericField<T>, T>
    {
        public abstract string Label { get; }

        public MudNumericField<T> Instance { get; set; } = null!;

        public IRenderedComponent<MudNumericField<T>> RenderedComponent { get; set; } = null!;

        public T Value
        {
            get => Instance.Value;
            set => Instance.Value = value;
        }
    }

    public abstract class TextFormField : IFormFieldDeclaration<MudTextField<string?>, string?>
    {
        public abstract string Label { get; }

        public MudTextField<string?> Instance { get; set; } = null!;

        public string? Value
        {
            get => Instance.Value;
            set => Instance.Value = value;
        }

        public IRenderedComponent<MudTextField<string?>> RenderedComponent { get; set; } = null!;
    }

    public abstract class EnumFormField<TEnum> : IFormFieldDeclaration<MudEnumSelect<TEnum>, TEnum?> where TEnum : struct, Enum
    {
        public abstract string Label { get; }
        public MudEnumSelect<TEnum> Instance { get; set; } = null!;

        public TEnum? Value
        {
            get => Instance.Value;
            set => Instance.Value = value;
        }

        public IRenderedComponent<MudEnumSelect<TEnum>> RenderedComponent { get; set; } = null!;
    }

    public abstract class DateFormField : IFormFieldDeclaration<MudDatePicker, DateTime?>
    {
        public abstract string Label { get; }
        public MudDatePicker Instance { get; set; } = null!;

        public DateTime? Value
        {
            get => Instance.Date;
            set => Instance.Date = value;
        }

        public IRenderedComponent<MudDatePicker> RenderedComponent { get; set; } = null!;
    }

    public class Id : NumericFormField<int>
    {
        public override string Label => "Id";
    }

    public class FirstName : TextFormField
    {
        public override string Label => "First Name";
    }

    public class LastName : TextFormField
    {
        public override string Label => "Last Name";
    }

    public class Position : EnumFormField<EmployeePosition>
    {
        public override string Label => "Position";
    }

    public class Gender : EnumFormField<EmployeeGender>
    {
        public override string Label => "Gender";
    }

    public class BirthDate : DateFormField
    {
        public override string Label => "Birth Date";
    }

    public class Salary : NumericFormField<int?>
    {
        public override string Label => "Salary";
    }

    public class Address : TextFormField
    {
        public override string Label => "Address";
    }

    public class HireDate : DateFormField
    {
        public override string Label => "Hire Date";
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

    [ComponentUsedInMarkupTitle(typeof(DataAnnotationsValidator))]
    [ComponentUsedInMarkupDescription(typeof(DataAnnotationsValidator))]
    [Precondition]
    public Task DataAnnotationsValidatorUsed()
    {
        ValidateComponentUsage(Component, typeof(DataAnnotationsValidator));
        return Task.CompletedTask;
    }

    [ComponentUsedInMarkupTitle(typeof(EditForm))]
    [ComponentUsedInMarkupDescription(typeof(EditForm))]
    [Precondition]
    public Task EditFormUsed()
    {
        ValidateComponentUsage(Component, typeof(EditForm));
        return Task.CompletedTask;
    }

    private async Task ValidateDeclaredField<TComponentType, TValue>(IFormFieldDeclaration<TComponentType, TValue> field) where TComponentType : IComponent
    {
        var ctx = await SetupTestContext();
        var renderedComponent = ctx.RenderComponent<ExamEmployeeForm>(builder => builder.Add(form => form.Employee, new ExamEmployee()));
        GetFormField(renderedComponent, field);
    }

    private IRenderedComponent<TComponentType> GetFormField<TComponentType, TValue>(IRenderedComponent<ExamEmployeeForm> form, IFormFieldDeclaration<TComponentType, TValue> field) where TComponentType : IComponent
    {
        var components = form.FindComponents<TComponentType>();

        var firstOrDefault = components.FirstOrDefault(p => p.GetLabel() == field.Label);
        if (firstOrDefault == null)
        {
            throw new TestRunException($"Could not find Component of type {typeof(TComponentType)} with label {field.Label} inside the Employee form");
        }

        field.Initialize(form);

        return firstOrDefault;
    }

    [Title("Input field for " + nameof(TestRunContext.Id) + " defined")]
    [Description("This test verifies that you have declared the input field for " + nameof(TestRunContext.Id))]
    [Precondition]
    public async Task FieldForIdDefined()
    {
        await ValidateDeclaredField(TestRunContext.Id);
    }


    [Title("Input field for " + nameof(FirstName) + " defined")]
    [Description("This test verifies that you have declared the input field for " + nameof(TestRunContext.FirstName))]
    [Precondition]
    public async Task FieldForFirstNameDefined()
    {
        await ValidateDeclaredField(TestRunContext.FirstName);
    }

    [Title("Input field for " + nameof(LastName) + " defined")]
    [Description("This test verifies that you have declared the input field for " + nameof(TestRunContext.LastName))]
    [Precondition]
    public async Task FieldForLastNameDefined()
    {
        await ValidateDeclaredField(TestRunContext.LastName);
    }

    [Title("Input field for " + nameof(Position) + " defined")]
    [Description("This test verifies that you have declared the input field for " + nameof(TestRunContext.Position))]
    [Precondition]
    public async Task FieldForPositionDefined()
    {
        await ValidateDeclaredField(TestRunContext.Position);
    }

    [Title("Input field for " + nameof(Gender) + " defined")]
    [Description("This test verifies that you have declared the input field for " + nameof(TestRunContext.Gender))]
    [Precondition]
    public async Task FieldForGenderDefined()
    {
        await ValidateDeclaredField(TestRunContext.Gender);
    }

    [Title("Input field for " + nameof(BirthDate) + " defined")]
    [Description("This test verifies that you have declared the input field for " + nameof(TestRunContext.BirthDate))]
    [Precondition]
    public async Task FieldForBirthDateDefined()
    {
        await ValidateDeclaredField(TestRunContext.BirthDate);
    }

    [Title("Input field for " + nameof(Salary) + " defined")]
    [Description("This test verifies that you have declared the input field for " + nameof(TestRunContext.Salary))]
    [Precondition]
    public async Task FieldForSalaryDefined()
    {
        await ValidateDeclaredField(TestRunContext.Salary);
    }

    [Title("Input field for " + nameof(Address) + " defined")]
    [Description("This test verifies that you have declared the input field for " + nameof(TestRunContext.Address))]
    [Precondition]
    public async Task FieldForAddressDefined()
    {
        await ValidateDeclaredField(TestRunContext.Address);
    }

    [Title("Input field for " + nameof(HireDate) + " defined")]
    [Description("This test verifies that you have declared the input field for " + nameof(TestRunContext.HireDate))]
    [Precondition]
    public async Task FieldForHireDateDefined()
    {
        await ValidateDeclaredField(TestRunContext.HireDate);
    }

    #endregion

    [Title("All fields disabled if EditMode = false")]
    [Description("This test verifies that all the fields in the " + nameof(ExamEmployeeForm) + " component are disabled when the " + nameof(ExamEmployeeForm.IsEditMode) +
                 " property is set to `false`. It does this by setting up a test context, rendering the " + nameof(ExamEmployeeForm) + " component with an instance of " + nameof(ExamEmployee) + " and " + nameof(ExamEmployeeForm.IsEditMode) +
                 " set to `false`, and then validating that each individual form field - " + nameof(TestRunContext.Id) + ", " + nameof(TestRunContext.FirstName) + ", " + nameof(TestRunContext.LastName) + ", " + nameof(TestRunContext.Position) +
                 ", " + nameof(TestRunContext.Gender) + ", " + nameof(TestRunContext.BirthDate) + ", " + nameof(TestRunContext.Salary) + ", " + nameof(TestRunContext.Address) + ", and " + nameof(TestRunContext.HireDate) + " - is disabled.")]
    public async Task GivenEmployeeForm_WhenEditModeFalse_ThenAllFieldsDisabled()
    {
        var testContext = await SetupTestContext();
        var form = testContext.RenderComponent<ExamEmployeeForm>(builder =>
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, false));

        ValidateDisabled(GetFormField(form, TestRunContext.Id), true);
        ValidateDisabled(GetFormField(form, TestRunContext.FirstName), true);
        ValidateDisabled(GetFormField(form, TestRunContext.LastName), true);
        ValidateDisabled(GetFormField(form, TestRunContext.Position), true);
        ValidateDisabled(GetFormField(form, TestRunContext.Gender), true);
        ValidateDisabled(GetFormField(form, TestRunContext.BirthDate), true);
        ValidateDisabled(GetFormField(form, TestRunContext.Salary), true);
        ValidateDisabled(GetFormField(form, TestRunContext.Address), true);
        ValidateDisabled(GetFormField(form, TestRunContext.HireDate), true);
    }

    private void ValidateDisabled<TComponent>(IRenderedComponent<TComponent> component, bool expectedState) where TComponent : IComponent
    {
        if (component.GetDisabled() != expectedState)
        {
            throw new TestRunException($"The form field's Disabled property is expected to be {expectedState}, but it is {component.GetDisabled()}. " +
                                       $"Have you bound its \"Disabled\" property properly according to the exercise description??");
        }
    }

    [Title("Edit and Cancel buttons are hidden if EditMode = false")]
    [Description("This test verifies that the 'Edit' and 'Cancel' buttons in the " + nameof(ExamEmployeeForm) + " component are hidden when the " + nameof(ExamEmployeeForm.IsEditMode) + " property is set to `false`")]
    public async Task GivenEmployeeForm_WhenEditMOdeFalse_ThenEditAndCancelButtonsHidden()
    {
        var testContext = await SetupTestContext();
        var form = testContext.RenderComponent<ExamEmployeeForm>(builder =>
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

    [Title("ID disabled if EditMode = true")]
    [Description("This test confirms that the `Id` field within the " + nameof(ExamEmployeeForm) + " component remains disabled when the " + nameof(ExamEmployeeForm.IsEditMode) + " property is set to `true`.")]
    public async Task GivenEmployeeForm_WhenEditModeTrue_ThenIdDisabled()
    {
        var testContext = await SetupTestContext();
        var form = testContext.RenderComponent<ExamEmployeeForm>(builder =>
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, true));

        ValidateDisabled(GetFormField(form, TestRunContext.Id), true);
    }

    [Title("Rest of the fields NOT Disabled if EditMode = true")]
    [Description("This test checks that when the " + nameof(ExamEmployeeForm.IsEditMode) + " property is `true`, all other fields within the " + nameof(ExamEmployeeForm) + " component, except for `Id`, are not disabled. It ensures that the state of fields matches expected behaviour when in edit mode.")]
    public async Task GivenEmployeeForm_WhenEditModeTrue_ThenOtherFieldsNotDisabled()
    {
        var testContext = await SetupTestContext();
        var form = testContext.RenderComponent<ExamEmployeeForm>(builder =>
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, true));

        ValidateDisabled(GetFormField(form, TestRunContext.FirstName), false);
        ValidateDisabled(GetFormField(form, TestRunContext.LastName), false);
        ValidateDisabled(GetFormField(form, TestRunContext.Position), false);
        ValidateDisabled(GetFormField(form, TestRunContext.Gender), false);
        ValidateDisabled(GetFormField(form, TestRunContext.BirthDate), false);
        ValidateDisabled(GetFormField(form, TestRunContext.Salary), false);
        ValidateDisabled(GetFormField(form, TestRunContext.Address), false);
        ValidateDisabled(GetFormField(form, TestRunContext.HireDate), false);
    }

    [Title("Edit and Cancel buttons are displayed if EditMode = true")]
    [Description("This test ensures that the 'Edit' and 'Cancel' buttons within the " + nameof(ExamEmployeeForm) + " component are visible when the " + nameof(ExamEmployeeForm.IsEditMode) +
                 " property is `true`. If these buttons are not visible, it signals an error in the form's rendering behavior.")]
    public async Task GivenEmployeeForm_WhenEditModeTrue_ThenEditAndCancelButtonsDisplayed()
    {
        var testContext = await SetupTestContext();
        var form = testContext.RenderComponent<ExamEmployeeForm>(builder =>
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, true));

        var buttons =
            form.FindComponents<MudButton>()
                .Where(p => p.FindAll("#save").Any() || p.FindAll("#cancel").Any());
        ;
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

    [Title("Clicking cancel invokes Cancel Event")]
    [Description("This test verifies that clicking the 'Cancel' button within the " + nameof(ExamEmployeeForm) + " component triggers the 'Cancel' event. If the event is not invoked, it points to a discrepancy in the event handling mechanism.")]
    public async Task GivenEmployeeForm_WhenCancelClicked_ThenCancelEventInvoked()
    {
        await GivenEmployeeForm_WhenEditModeTrue_ThenEditAndCancelButtonsDisplayed();


        var testContext = await SetupTestContext();
        var isCancelInvoked = false;
        var form = testContext.RenderComponent<ExamEmployeeForm>(builder =>
            builder.Add(form => form.Employee, new ExamEmployee())
                .Add(form => form.IsEditMode, true)
                .Add(form => form.Cancel, EventCallback.Factory.Create(this, () => isCancelInvoked = true)));

        var buttons = form.FindComponents<MudButton>();
        var cancelButton = buttons.FirstOrDefault(p => p.FindAll("#cancel").Any());

        if (cancelButton == null)
        {
            throw new TestRunException("There is no cancel button!");
        }

        await cancelButton.Find("#cancel").ClickAsync(new MouseEventArgs());
        if (!isCancelInvoked)
        {
            throw new TestRunException("Cancel event was not invoked when clicking Cancel button!");
        }
    }

    [Title("Form fields are bound to Employee")]
    [Description("This test ensures that the fields within the " + nameof(ExamEmployeeForm) + " component are correctly bound to the corresponding properties of the given " + nameof(ExamEmployee) +
                 " instance. An erroneous data binding mechanism would result in mismatched or untracked data between the form fields and the employee instance.")]
    public async Task GivenEmployeeForm_WhenCreated_ThenFormFieldsAreBoundToEmployeeObject()
    {
        var testContext = await SetupTestContext();
        var employee = new ExamEmployee() { Id = 10 };
        var form = testContext.RenderComponent<ExamEmployeeForm>(builder =>
        {
            builder.Add(form => form.Employee, employee)
                .Add(form => form.IsEditMode, true);
        });


        async Task ValidateDataBinding<TComponent, TValue>(IRenderedComponent<ExamEmployeeForm> form, IFormFieldDeclaration<TComponent, TValue> field,
            Action<TValue> setValueOfEmployee,
            Func<TValue> getValueOfEmployee,
            TValue value1, TValue value2) where TComponent : IComponent
        {
            var formField = GetFormField(form, field);
            var eventCallback = formField.GetValueChanged<TComponent, TValue>();
            await form.InvokeAsync(async () => await eventCallback.InvokeAsync(value1));
            var valueOfEmployee = getValueOfEmployee();
            if (!valueOfEmployee!.Equals(value1))
            {
                throw new TestRunException($"Data binding does not work in the direction of form input -> employee for field {field.Label}");
            }

            setValueOfEmployee(value2);

            form.SetParametersAndRender(builder => builder.Add(p => p.Employee, employee));
            var value = formField.GetValue<TComponent, TValue>();
            if (!value2!.Equals(value))
            {
                throw new TestRunException($"Data binding does not work in the direction of employee -> form input for field {field.Label}");
            }
        }

        await ValidateDataBinding(form, TestRunContext.FirstName, (value) => employee.FirstName = value, () => employee.FirstName, "Test1", "Test2");
        await ValidateDataBinding(form, TestRunContext.LastName, (value) => employee.LastName = value, () => employee.LastName, "Test1", "Test2");
        await ValidateDataBinding(form, TestRunContext.Salary, (value) => employee.Salary = value, () => employee.Salary, 20000, 35000);
        await ValidateDataBinding(form, TestRunContext.BirthDate, (value) => employee.BirthDate = value, () => employee.BirthDate, DateTime.Now, DateTime.MaxValue);
        await ValidateDataBinding(form, TestRunContext.Address, (value) => employee.Address = value, () => employee.Address, "Test1", "Test2");
        await ValidateDataBinding(form, TestRunContext.HireDate, (value) => employee.HireDate = value, () => employee.HireDate, DateTime.Now, DateTime.MaxValue);

        await ValidateDataBinding(form, TestRunContext.Position, (value) => employee.Position = value, () => employee.Position, EmployeePosition.CEO, EmployeePosition.CFO);
        await ValidateDataBinding(form, TestRunContext.Gender, (value) => employee.Gender = value, () => employee.Gender, EmployeeGender.Male, EmployeeGender.Female);
    }

    [Title("Entering invalid fields -> Error displayed for fields")]
    [Description("This test confirms that whenever invalid entries are made into the fields of the " + nameof(ExamEmployeeForm) +
                 " component, appropriate error messages are displayed for those fields. Also, it tests the behaviour of the form when the " + nameof(ExamEmployeeForm.EmployeeValid) + " event is invoked with an invalid employee.")]
    public async Task GivenEmployeeForm_WhenInvalidFieldsEntered_ThenErrorMessageForFields()
    {
        var testContext = await SetupTestContext();
        var employee = new ExamEmployee() { Id = 10 };
        var employeeValidInvoked = false;
        var form = testContext.RenderComponent<ExamEmployeeForm>(builder =>
        {
            builder.Add(form => form.Employee, employee)
                .Add(form => form.IsEditMode, true)
                .Add(form => form.EmployeeValid, EventCallback.Factory.Create(this, () => employeeValidInvoked = true));
        });

        await form.Find("#save").ClickAsync(new MouseEventArgs());

        void ValidateRequiredError<TComponent, TValue>(IFormFieldDeclaration<TComponent, TValue> field) where TComponent : IComponent
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
            throw new TestRunException($"{nameof(ExamEmployeeForm.EmployeeValid)} is invoked with invalid Employee!");
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

    [Title("Entering valid fields -> EmployeeValid is invoked")]
    [Description("This test verifies that when valid entries are made into the fields of the " + nameof(ExamEmployeeForm) + " component and 'Save' is clicked, the " + nameof(ExamEmployeeForm.EmployeeValid) + " event is correctly invoked.")]
    public async Task GivenEmployeeForm_WhenFieldsEnteredAndSaveClicked_ThenValidEmployeeInvoked()
    {
        var testContext = await SetupTestContext();
        var employee = new ExamEmployee()
        {
            Id = 10,
            Salary = 35000,
            Address = "My address",
            Gender = EmployeeGender.Female,
            BirthDate = DateTime.Today,
            Position = EmployeePosition.Developer,
            LastName = "Test",
            FirstName = "Test",
            HireDate = DateTime.MinValue,
        };
        var employeeValidInvoked = false;
        var form = testContext.RenderComponent<ExamEmployeeForm>(builder =>
        {
            builder.Add(form => form.Employee, employee)
                .Add(form => form.IsEditMode, true)
                .Add(form => form.EmployeeValid, EventCallback.Factory.Create(this, () => employeeValidInvoked = true));
        });

        await form.Find("#save").ClickAsync(new MouseEventArgs());

        void ValidateNoErrors<TComponent, TValue>(IFormFieldDeclaration<TComponent, TValue> field) where TComponent : IComponent
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
            throw new TestRunException($"{nameof(ExamEmployeeForm.EmployeeValid)} is NOT invoked with valid Employee!");
        }
    }
}

public static class FormFieldExtensions
{
    public static string GetLabel<T>(this IRenderedComponent<T> component) where T : IComponent
    {
        var propertyInfo = component.Instance.GetType().GetProperties().FirstOrDefault(p => p.Name == nameof(MudField.Label));

        if (propertyInfo == null)
        {
            throw new KeyNotFoundException(nameof(MudField.Label));
        }

        return propertyInfo.GetValue(component.Instance)!.ToString()!;
    }

    public static bool GetDisabled<T>(this IRenderedComponent<T> component) where T : IComponent
    {
        var propertyInfo = component.Instance.GetType().GetProperties().FirstOrDefault(p => p.Name == nameof(MudField.Disabled));
        if (propertyInfo == null)
        {
            throw new KeyNotFoundException(nameof(MudField.Disabled));
        }

        var disabled = (bool)propertyInfo.GetValue(component.Instance)!;
        return disabled;
    }

    public static EventCallback<TValue> GetValueChanged<TComponent, TValue>(this IRenderedComponent<TComponent> component) where TComponent : IComponent
    {
        var propertyInfo = component.Instance.GetType().GetProperties().FirstOrDefault(p =>
        {
            var valueChangedName = component.Instance is MudDatePicker ? nameof(MudDatePicker.DateChanged) : nameof(MudTextField<string>.ValueChanged);
            return p.Name == valueChangedName;
        });

        if (propertyInfo == null)
        {
            throw new KeyNotFoundException(nameof(MudTextField<string>.ValueChanged));
        }

        var valueChanged = (EventCallback<TValue>)propertyInfo.GetValue(component.Instance)!;
        return valueChanged;
    }

    public static TValue GetValue<TComponent, TValue>(this IRenderedComponent<TComponent> component) where TComponent : IComponent
    {
        var valueName = component.Instance is MudDatePicker ? nameof(MudDatePicker.Date) : nameof(MudTextField<string>.Value);
        var propertyInfo = component.Instance.GetType().GetProperties().FirstOrDefault(p => p.Name == valueName);

        if (propertyInfo == null)
        {
            throw new KeyNotFoundException(valueName);
        }

        var valueChanged = (TValue)propertyInfo.GetValue(component.Instance)!;
        return valueChanged;
    }

    public static string GetErrorText<TComponent>(this IRenderedComponent<TComponent> component) where TComponent : IComponent
    {
        var valueName = nameof(MudTextField<string>.ErrorText);
        var propertyInfo = component.Instance.GetType().GetProperties().FirstOrDefault(p => p.Name == valueName);

        if (propertyInfo == null)
        {
            throw new KeyNotFoundException(valueName);
        }

        return propertyInfo.GetValue(component.Instance)?.ToString() ?? string.Empty;
    }
}