using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Pages._3_DataBinding;
using BlazorCraft.Web.Shared._Exercises._3_DataBinding;
using Bunit;
using Microsoft.AspNetCore.Components;
using Employee = BlazorCraft.Web.Shared._Exercises._3_DataBinding.DataBinding_Ex_LessonFinal.Employee;

namespace BlazorCraft.Web.Tests._3_DataBinding;



[TestForPage(typeof(ComponentDataBinding))]
public class Test_DataBinding_Ex_LessonFinal : ComponentTestBase<DataBinding_Ex_LessonFinal>
{
    private static Type EditorComponentType = typeof(DataBinding_Ex1);
    
    [ComponentUsedInMarkupTitle(typeof(DataBinding_Ex1))]
    [ComponentUsedInMarkupDescription(typeof(DataBinding_Ex1))]
    [Precondition]
    public Task GivenComponentDataBinding_Ex_LessonFinal_WhenDeclared_ThenComponentDataBinding_Ex1ComponentUsed()
    {
        var component = new DataBinding_Ex_LessonFinal();
        ValidateComponentUsage(component, EditorComponentType);
		return Task.CompletedTask;
	}

    [Title("Table rows rendered correctly")]
    [Description("This test verifies that the table rows for each employee were rendered correctly")]
    public Task GivenComponentDataBinding_Ex_LessonFinal_WhenRendered_ThenTableRowsRenderedCorrectly()
    {
        TestContext testContext = new TestContext();
        List<Employee> employees = new List<Employee>()
        {
            new() { Id = 1, FirstName = "Test", LastName = "Theodore", IsEditorVisible = false },
            new() { Id = 2, FirstName = "Test", LastName = "Tiffany", IsEditorVisible = false }
        };
        var renderedComponent = testContext.RenderComponent<DataBinding_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(nameof(DataBinding_Ex_LessonFinal.Employees), employees)
        );

        var rows = renderedComponent.FindAll("tr");
        rows[1].MarkupMatches($"<tr> <td>{employees[0].Id}</td> <td>{employees[0].FirstName}</td> <td>{employees[0].LastName}</td> <td><button class=\"btn btn-primary\">Edit</button></td>  </tr>");
        rows[2].MarkupMatches($"<tr> <td>{employees[1].Id}</td> <td>{employees[1].FirstName}</td> <td>{employees[1].LastName}</td> <td><button class=\"btn btn-primary\">Edit</button></td>  </tr>");
		return Task.CompletedTask;
	}

    [Title("Clicking Edit button reveals editor")]
    [Description("This test verifies that clicking the edit button reveals the editor for the employee")]
    public Task GivenEditButton_WhenClicked_ThenEmployeeEditorIsRevealed()
    {
        TestContext testContext = new TestContext();
        List<Employee> employees = new List<Employee>()
        {
            new() { Id = 1, FirstName = "Test", LastName = "Theodore", IsEditorVisible = false },
            new() { Id = 2, FirstName = "Test", LastName = "Tiffany", IsEditorVisible = false }
        };
        var renderedComponent = testContext.RenderComponent<DataBinding_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(nameof(DataBinding_Ex_LessonFinal.Employees), employees)
        );

        var buttons = renderedComponent.FindAll("tr button");
        if(buttons.Count != employees.Count)
        {
            throw new TestRunException("Unexpected amount of buttons rendered");
        }
        buttons[0].Click();

        renderedComponent.Find(".employee-first-name");
        renderedComponent.Find(".employee-last-name");
		return Task.CompletedTask;
	}
    
    [Title("Editor change reflected in table")]
    [Description("This test verifies that once you edit something in the editor will be reflected in the employees table as well")]
    public async Task GivenEditor_WhenTextIsEdited_ThenChangeIsReflectedInEmployeesTable()
    {
        TestContext testContext = new TestContext();
        List<Employee> employees = new List<Employee>()
        {
            new() { Id = 1, FirstName = "Test", LastName = "Theodore", IsEditorVisible = false },
            new() { Id = 2, FirstName = "Test", LastName = "Tiffany", IsEditorVisible = false }
        };
        var renderedComponent = testContext.RenderComponent<DataBinding_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(nameof(DataBinding_Ex_LessonFinal.Employees), employees)
        );

        var buttons = renderedComponent.FindAll("tr button");
        if(buttons.Count != employees.Count)
        {
            throw new TestRunException("Unexpected amount of buttons rendered");
        }
        buttons[0].Click();

        var firstNameInput = renderedComponent.Find(".employee-first-name");
        var lastNameInput = renderedComponent.Find(".employee-last-name");

        var newFirstName = "New first name";
        var newLastName = "New last name";
        
        await firstNameInput.InputAsync(new ChangeEventArgs(){Value = newFirstName});
        await lastNameInput.InputAsync(new ChangeEventArgs(){Value = newLastName});

        if (employees[0].FirstName != newFirstName)
        {
            throw new TestRunException("Employee's first name change is not reflected in the employees table!");
        }
        
        if (employees[0].LastName != newLastName)
        {
            throw new TestRunException("Employee's last name change is not reflected in the employees table!");
        }
        
        
    }
}