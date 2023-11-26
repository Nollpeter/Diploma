using System.ComponentModel.DataAnnotations;
using AngleSharp.Dom;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._7_Forms;
using BlazorCraft.Web.Shared._Exercises.Forms;
using BlazorCraft.Web.Shared.Examples._1_Components._5_RenderFragments;
using BlazorCraft.Web.Tests.Routing;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Employee = BlazorCraft.Web.Shared._Exercises._7_Forms.Forms_Ex_LessonFinal.Employee;

namespace BlazorCraft.Web.Tests.Forms;

[TestForPage(typeof(Pages._7_Forms.Forms))]
public class Test_Forms_Ex_LessonFinal : ComponentTestBase<Forms_Ex_LessonFinal>
{
    public const string CreateButtonLabel = "Create";
    public const string SaveButtonLabel = "Save";
    private readonly IJSRuntime _jsRuntime;

    public Test_Forms_Ex_LessonFinal(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    [Title("Employee properties annotated with Validation attributes")]
    [Description("This test verifies that you have annotated the Employee type with the proper Validation attributes")]
    [Precondition]
    public async Task GivenEmployeeProperties_WhenAnnotated_ThenValidationAttributesAreApplied()
    {
        Employee employee = new Employee();
        ValidatePropertyAnnotatedWithAttribute(employee, nameof(Employee.Salary), typeof(RequiredAttribute));
        ValidatePropertyAnnotatedWithAttribute(employee, nameof(Employee.Salary), typeof(RangeAttribute));
        ValidatePropertyAnnotatedWithAttribute(employee, nameof(Employee.FirstName), typeof(RequiredAttribute));
        ValidatePropertyAnnotatedWithAttribute(employee, nameof(Employee.LastName), typeof(RequiredAttribute));
        ValidatePropertyAnnotatedWithAttribute(employee, nameof(Employee.BirthDate), typeof(RequiredAttribute));
        ValidatePropertyAnnotatedWithAttribute(employee, nameof(Employee.Position), typeof(RequiredAttribute));
    }


    [ComponentUsedInMarkupTitle(typeof(DataAnnotationsValidator))]
    [ComponentUsedInMarkupDescription(typeof(DataAnnotationsValidator))]
    [Precondition]
    public async Task GivenForms_Ex_LessonFinal_WhenDeclared_ThenDataAnnotationsValidatorComponentUsed()
    {
        ValidateComponentUsage(Component, typeof(DataAnnotationsValidator));
    }

    [ComponentUsedInMarkupTitle(typeof(ListComponent5<Employee>))]
    [ComponentUsedInMarkupDescription(typeof(ListComponent5<Employee>))]
    [Precondition]
    public async Task GivenForms_Ex_LessonFinal_WhenDeclared_ThenListComponent5Used()
    {
        ValidateComponentUsage(Component, typeof(ListComponent5<Employee>));
    }

    [Title("Create button is rendered for the list component Title")]
    [Description("This test verifies that the create button is rendered for the list component Title")]
    [Precondition]
    public async Task GivenListComponentTitle_WhenRendered_ThenCreateButtonIsPresent()
    {
        var renderedComponent = SetupTestContext().RenderComponent<Forms_Ex_LessonFinal>();
        var buttons = renderedComponent.FindAll("button");
        var createNewButton = buttons.FirstOrDefault(p => p.InnerHtml.Trim() == CreateButtonLabel);
        if (!buttons.Any() || createNewButton == null)
        {
            throw new TestRunException("There is no \"Create\" button on the component!");
        }
    }

    [Title("Table is rendered properly")]
    [Description("This test verifies teh employees table is rendered properly with all its fields")]
    [Precondition]
    public async Task GivenEmployeesTable_WhenRendering_ThenItIsRenderedProperlyWithAllFields()
    {
        var testContext = SetupTestContext();
        var renderedComponent = testContext.RenderComponent<Forms_Ex_LessonFinal>();

        var theads = renderedComponent.FindAll("thead");
        if (theads.Count != 1)
        {
            throw new TestRunException("Cannot find <thead> tag for the employees table");
        }

        var thead = theads.First();
        thead.InnerHtml.MarkupMatches($"<tr><th>Id</th><th>First Name</th><th>Last Name</th><th>Position</th><th>Salary</th><th>Birthdate</th></tr>");

        var tbodys = renderedComponent.FindAll("tbody");
        if (tbodys.Count != 1)
        {
            throw new TestRunException("Cannot find <tbody> tag for the employees table");
        }

        var tbody = tbodys.First();
        var employeeService = testContext.Services.GetRequiredService<IEmployeeService>();
        var employees = await employeeService.GetEmployees();
        AssertTableWithEmployees(employees, tbody);
    }

    private void AssertTableWithEmployees(IList<Employee> employees, IElement tbody)
    {
        string expectedmarkup = "";
        foreach (var employee in employees)
        {
            expectedmarkup += $"<tr><td>{employee.Id}</td><td>{employee.FirstName}</td><td>{employee.LastName}</td><td>{employee.Position}</td><td>{employee.Salary}</td><td>{employee.BirthDate}</td></tr>{Environment.NewLine}";
        }

        tbody.InnerHtml.MarkupMatches(expectedmarkup);
    }

    [Title("Form is hidden by default")]
    [Description("This test verifies that the edit form for employees is hidden by default")]
    public async Task GivenEmployeeEditForm_WhenDefaultRendered_ThenItIsHidden()
    {
        var renderedComponent = SetupTestContext().RenderComponent<Forms_Ex_LessonFinal>();

        var forms = renderedComponent.FindAll("form");
        if (forms.Any())
        {
            throw new TestRunException("The edit form for employees should be hidden by default");
        }
    }

    private IElement GetButtonWithLabel(IRenderedComponent<Forms_Ex_LessonFinal> renderedComponent, string label)
    {
        var buttons = renderedComponent.FindAll("button");
        var createNewButton = buttons.FirstOrDefault(p => p.InnerHtml.Trim() == label);
        return createNewButton!;
    }

    [Title("Clicking create shows form")]
    [Description("This test verifies that the edit form for employees is displays once the Create button is clicked")]
    public async Task GivenCreateButton_WhenClicked_ThenEmployeeEditFormIsDisplayed()
    {
        var renderedComponent = SetupTestContext().RenderComponent<Forms_Ex_LessonFinal>();

        var createButton = GetButtonWithLabel(renderedComponent, CreateButtonLabel);
        await createButton.ClickAsync(new MouseEventArgs());

        var forms = renderedComponent.FindAll("form");
        if (!forms.Any())
        {
            throw new TestRunException("The edit form for employees is not visible after clicking the \"Create\" button");
        }
    }

    [Title("Invalid employee cannot be saved")]
    [Description("This test verifies that an employee with invalid or missing fields cannot be saved")]
    public async Task GivenInvalidEmployee_WhenSaving_ThenCannotBeSaved()
    {
        var renderedComponent = SetupTestContext().RenderComponent<Forms_Ex_LessonFinal>();

        var createButton = GetButtonWithLabel(renderedComponent, CreateButtonLabel);

        await createButton.ClickAsync(new MouseEventArgs());

        var forms = renderedComponent.FindAll("form");
        if (!forms.Any())
        {
            throw new TestRunException("The edit form for employees is not visible after clicking the \"Create\" button");
        }

        await createButton.ClickAsync(new MouseEventArgs());
        var saveButton = GetButtonWithLabel(renderedComponent, SaveButtonLabel);

        async Task ClickSave()
        {
            await saveButton.ClickAsync(new MouseEventArgs());
        }

        await ClickSave();
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.FirstName), true);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.LastName), true);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.Salary), true);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.Position), true);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.BirthDate), true);

        await SetEmployeeFirstName(renderedComponent, "Test FirstName");
        await ClickSave();

        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.FirstName), false);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.LastName), true);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.Salary), true);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.Position), true);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.BirthDate), true);

        await SetEmployeeLastName(renderedComponent, "Test LastName");
        await ClickSave();

        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.FirstName), false);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.LastName), false);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.Salary), true);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.Position), true);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.BirthDate), true);

        await SetEmployeeSalary(renderedComponent, 30000);
        await ClickSave();

        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.FirstName), false);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.LastName), false);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.Salary), false);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.Position), true);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.BirthDate), true);

        await SetEmployeePosition(renderedComponent, "Test LastName");
        await ClickSave();

        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.FirstName), false);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.LastName), false);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.Salary), false);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.Position), false);
        ValidateRequiredErrorMessage(renderedComponent, nameof(Employee.BirthDate), true);

        await SetEmployeeBirthDate(renderedComponent, DateTime.Today);
        await ClickSave();

        var validationSummary = renderedComponent.FindAll(".validation-summary");
        var errorsShown = validationSummary.Any();
        if (errorsShown)
        {
            throw new TestRunException($"Unexpected validation errors: {validationSummary[0].InnerHtml}");
        }
    }

    private void ValidateRequiredErrorMessage(IRenderedComponent<Forms_Ex_LessonFinal> renderedComponent, string propertyName, bool shouldShow)
    {
        var element = renderedComponent.Find(".validation-summary");
        if (shouldShow)
        {
            if (!element.InnerHtml.Contains($"The {propertyName} field is required"))
            {
                throw new TestRunException($"Missing error message for required field: {propertyName}");
            }
        }
        else
        {
            if (element.InnerHtml.Contains($"The {propertyName} field is required"))
            {
                throw new TestRunException($"Error message is shown for filled required field: {propertyName}");
            }
        }
    }

    private async Task SetEmployeeFirstName(IRenderedComponent<Forms_Ex_LessonFinal> renderedComponent, string? firstName)
    {
        await renderedComponent.Find(".first-name").ChangeAsync(new ChangeEventArgs() { Value = firstName });
    }

    private async Task SetEmployeeLastName(IRenderedComponent<Forms_Ex_LessonFinal> renderedComponent, string? lastName)
    {
        await renderedComponent.Find(".last-name").ChangeAsync(new ChangeEventArgs() { Value = lastName });
    }

    private async Task SetEmployeePosition(IRenderedComponent<Forms_Ex_LessonFinal> renderedComponent, string? position)
    {
        await renderedComponent.Find(".position").ChangeAsync(new ChangeEventArgs() { Value = position });
    }

    private async Task SetEmployeeSalary(IRenderedComponent<Forms_Ex_LessonFinal> renderedComponent, int? salary)
    {
        await renderedComponent.Find(".salary").ChangeAsync(new ChangeEventArgs() { Value = salary?.ToString() });
    }

    private async Task SetEmployeeBirthDate(IRenderedComponent<Forms_Ex_LessonFinal> renderedComponent, DateTime? birthDate)
    {
        await renderedComponent.Find(".birthdate").ChangeAsync(new ChangeEventArgs() { Value = birthDate?.ToString() });
    }

    [Title("Valid employee is added to the list")]
    [Description("This test verifies that a valid employee is added to the table")]
    public async Task GivenValidEmployee_WhenAdded_ThenAppearsInTable()
    {
        var testContext = SetupTestContext();
        var renderedComponent = testContext.RenderComponent<Forms_Ex_LessonFinal>();

        var createButton = GetButtonWithLabel(renderedComponent, CreateButtonLabel);

        await createButton.ClickAsync(new MouseEventArgs());

        var forms = renderedComponent.FindAll("form");
        if (!forms.Any())
        {
            throw new TestRunException("The edit form for employees is not visible after clicking the \"Create\" button");
        }

        Employee employee = new Employee()
        {
            FirstName = "Test",
            LastName = "Test",
            Position = "Position",
            Salary = 30000,
            BirthDate = DateTime.Parse("1993.06.11")
        };

        var employeeService = testContext.Services.GetRequiredService<IEmployeeService>();
        var employees = await employeeService.GetEmployees();
        var max = employees.Select(p => p.Id).Max();
        await SetEmployeeFirstName(renderedComponent, employee.FirstName);
        await SetEmployeeLastName(renderedComponent, employee.LastName);
        await SetEmployeePosition(renderedComponent, employee.Position);
        await SetEmployeeSalary(renderedComponent, employee.Salary);
        await SetEmployeeBirthDate(renderedComponent, employee.BirthDate);
        renderedComponent.Render();

        await GetButtonWithLabel(renderedComponent, SaveButtonLabel).ClickAsync(new MouseEventArgs());

        try
        {
            employee.Id = max + 1;
            employees.Add(employee);
            var tbody = renderedComponent.Find("tbody");
            AssertTableWithEmployees(employees, tbody);
        }
        catch (KeyNotFoundException e)
        {
            throw new TestRunException("The valid employee cannot be found in the employee service after saving");
        }
    }

    private TestContext SetupTestContext()
    {
        TestContext testContext = new TestContext();
        testContext.Services.AddEmployeeService();
        testContext.Services.AddSingleton(_jsRuntime);
        return testContext;
    }
}