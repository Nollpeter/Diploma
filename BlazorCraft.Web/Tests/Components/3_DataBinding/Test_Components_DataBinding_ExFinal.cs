using AngleSharp.Dom;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Pages._3_Components;
using BlazorCraft.Web.Shared._Exercises._1_Components._3_DataBinding;
using BlazorCraft.Web.Tests.Routing;
using Bunit;
using Microsoft.AspNetCore.Components;

using Employee = BlazorCraft.Web.Shared._Exercises._1_Components._3_DataBinding.ComponentDataBinding_Ex_LessonFinal.Employee;

namespace BlazorCraft.Web.Tests.Components._3_DataBinding;



[TestForPage(typeof(ComponentDataBinding))]
public class Test_Components_DataBinding_ExFinal : ComponentTestBase<ComponentDataBinding_Ex_LessonFinal>
{
    private static Type EditorComponentType = typeof(ComponentDataBinding_Ex1);
    
    // Component is used
    [ComponentUsedInMarkupTitle(typeof(ComponentDataBinding_Ex1))]
    [ComponentUsedInMarkupDescription(typeof(ComponentDataBinding_Ex1))]
    [Precondition]
    public async Task Test1()
    {
        var component = new ComponentDataBinding_Ex_LessonFinal();
        ValidateComponentUsage(component, EditorComponentType);
        
    }

    // Table rows rendered properly
    [Title("Table rows rendered correctly")]
    [Description("This test verifies that the table rows for each employee were rendered properly")]
    public async Task Test2()
    {
        TestContext testContext = new TestContext();
        List<Employee> employees = new List<Employee>()
        {
            new() { Id = 1, FirstName = "Test", LastName = "Theodore", IsEditorVisible = false },
            new() { Id = 2, FirstName = "Test", LastName = "Tiffany", IsEditorVisible = false }
        };
        var renderedComponent = testContext.RenderComponent<ComponentDataBinding_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(nameof(ComponentDataBinding_Ex_LessonFinal.Employees), employees)
        );

        var rows = renderedComponent.FindAll("tr");
        rows[1].MarkupMatches($"<tr> <td>{employees[0].Id}</td> <td>{employees[0].FirstName}</td> <td>{employees[0].LastName}</td> <td><button class=\"btn btn-primary\">Edit</button></td>  </tr>");
        rows[2].MarkupMatches($"<tr> <td>{employees[1].Id}</td> <td>{employees[1].FirstName}</td> <td>{employees[1].LastName}</td> <td><button class=\"btn btn-primary\">Edit</button></td>  </tr>");

        
    }

    // By default editor is hidden
    // Clicking button reveals editor
    
    [Title("Clicking Edit button reveals editor")]
    [Description("This test verifies that clicking the edit button reveals the editor for the employee")]
    public async Task Test3()
    {
        TestContext testContext = new TestContext();
        List<Employee> employees = new List<Employee>()
        {
            new() { Id = 1, FirstName = "Test", LastName = "Theodore", IsEditorVisible = false },
            new() { Id = 2, FirstName = "Test", LastName = "Tiffany", IsEditorVisible = false }
        };
        var renderedComponent = testContext.RenderComponent<ComponentDataBinding_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(nameof(ComponentDataBinding_Ex_LessonFinal.Employees), employees)
        );

        var buttons = renderedComponent.FindAll("tr button");
        if(buttons.Count != employees.Count)
        {
            throw new TestRunException("Unexpected amount of buttons rendered");
        }
        buttons[0].Click();

        renderedComponent.Find(".employee-first-name");
        renderedComponent.Find(".employee-last-name");
        
        
    }
    
    // Editor changes reflected in table
    [Title("Editor change reflected in table")]
    [Description("This test verifies that once you edit something in the editor will be reflected in the employees table as well")]
    public async Task Test4()
    {
        TestContext testContext = new TestContext();
        List<Employee> employees = new List<Employee>()
        {
            new() { Id = 1, FirstName = "Test", LastName = "Theodore", IsEditorVisible = false },
            new() { Id = 2, FirstName = "Test", LastName = "Tiffany", IsEditorVisible = false }
        };
        var renderedComponent = testContext.RenderComponent<ComponentDataBinding_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(nameof(ComponentDataBinding_Ex_LessonFinal.Employees), employees)
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
 

    /*[ParameterDefinedTitle(EmployeeFirstNameParamName)]
    [ParameterDefinedDescription(EmployeeFirstNameParamName, typeof(string))]
    public async Task Test1()
    {
        var component = new ComponentDataBinding_Ex_LessonFinal();
        ValidateComponentProperty(component, EmployeeFirstNameParamName, typeof(string));
        
    }

    [ParameterDefinedTitle(EmployeeLastNameParamName)]
    [ParameterDefinedDescription(EmployeeLastNameParamName, typeof(string))]
    public async Task Test2()
    {
        var component = new ComponentDataBinding_Ex_LessonFinal();
        ValidateComponentProperty(component, EmployeeLastNameParamName, typeof(string));
        
    }

    [ParameterDefinedTitle(EmployeeFirstNameChangedName)]
    [ParameterDefinedDescription(EmployeeFirstNameChangedName, typeof(EventCallback<string>))]
    public async Task Test3()
    {
        var component = new ComponentDataBinding_Ex_LessonFinal();
        ValidateComponentProperty(component, EmployeeFirstNameChangedName, typeof(EventCallback<string>));
        
    }

    [ParameterDefinedTitle(EmployeeLastNameChangedName)]
    [ParameterDefinedDescription(EmployeeLastNameChangedName, typeof(EventCallback<string>))]
    public async Task Test4()
    {
        var component = new ComponentDataBinding_Ex_LessonFinal();
        ValidateComponentProperty(component, EmployeeLastNameChangedName, typeof(EventCallback<string>));
        
    }

    [Title(EmployeeFirstNameParamName + " binding Consumer -> Component")]
    [Description("This test verifies that once the Consumer component changes the " + EmployeeFirstNameParamName +
                 " it is reflected in the component ")]
    public async Task Test5()
    {
        var component = new ComponentDataBinding_Ex_LessonFinal();

        ValidateComponentProperty(component, EmployeeFirstNameParamName, typeof(string));
        ValidateComponentProperty(component, EmployeeLastNameParamName, typeof(string));
        ValidateComponentProperty(component, EmployeeFirstNameChangedName, typeof(EventCallback<string>));
        ValidateComponentProperty(component, EmployeeLastNameChangedName, typeof(EventCallback<string>));

        TestContext testContext = new TestContext();

        string firstName = "Theodore";

        var renderedComponent = testContext.RenderComponent<ComponentDataBinding_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(EmployeeFirstNameParamName, firstName));

        var inputs = renderedComponent.FindAll("input").ToList();

        var idValue = inputs[0].GetAttribute("value");
        if (idValue != firstName.ToString())
        {
            throw new TestRunException(
                $"{EmployeeFirstNameParamName} is not bound properly, its value should be {firstName}, but it is instead {idValue}");
        }


        
    }

    [Title(EmployeeFirstNameParamName + " binding Component -> Consumer")]
    [Description("This test verifies that once the Component changes the " + EmployeeFirstNameParamName +
                 " it is reflected in the Consumer component ")]
    public async Task Test6()
    {
        var component = new ComponentDataBinding_Ex_LessonFinal();
        ValidateComponentProperty(component, EmployeeFirstNameParamName, typeof(string));
        ValidateComponentProperty(component, EmployeeLastNameParamName, typeof(string));
        ValidateComponentProperty(component, EmployeeFirstNameChangedName, typeof(EventCallback<string>));
        ValidateComponentProperty(component, EmployeeLastNameChangedName, typeof(EventCallback<string>));

        TestContext testContext = new TestContext();

        string firstName = "Theodore";

        EventCallback<string> idChanged = EventCallback.Factory.Create<string>(this, value =>
        {
            firstName = value;
        });

        var renderedComponent = testContext.RenderComponent<ComponentDataBinding_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(EmployeeFirstNameParamName, firstName),
            ComponentParameter.CreateParameter(EmployeeFirstNameChangedName, idChanged)
        );



        var inputs = renderedComponent.FindAll("input").ToList();

        var idValue = inputs[0].GetAttribute("value");
        if (idValue != firstName.ToString())
        {
            throw new TestRunException(
                $"{EmployeeFirstNameParamName} is not bound properly, its value should be {firstName}, but it is instead {idValue}");
        }

        var inputText = renderedComponent.FindAll("input");
        var changedValue = "Changed";

        await inputText[0].InputAsync(new ChangeEventArgs(){Value = changedValue});
        if (firstName != changedValue)
        {
            throw new TestRunException(
                $"{EmployeeLastNameParamName} is not bound two way. Upon changing the value of the input, the change is not reflected. Are you calling NameChanged?");
        }
        
    }

    [Title(EmployeeLastNameParamName + " binding Consumer -> Component")]
    [Description("This test verifies that once the Consumer component changes the " + EmployeeLastNameParamName +
                 " it is reflected in the component ")]
    public async Task Test7()
    {
        var component = new ComponentDataBinding_Ex_LessonFinal();

        ValidateComponentProperty(component, EmployeeFirstNameParamName, typeof(string));
        ValidateComponentProperty(component, EmployeeLastNameParamName, typeof(string));
        ValidateComponentProperty(component, EmployeeFirstNameChangedName, typeof(EventCallback<string>));
        ValidateComponentProperty(component, EmployeeLastNameChangedName, typeof(EventCallback<string>));

        TestContext testContext = new TestContext();

        string lastName = "Test";

        var renderedComponent = testContext.RenderComponent<ComponentDataBinding_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(EmployeeLastNameParamName, lastName));

        var input = renderedComponent.Find(".employee-last-name");

        Console.WriteLine(input.ToMarkup());
        var nameValue = input.GetAttribute("value");
        if (nameValue != lastName)
        {
            throw new TestRunException(
                $"{EmployeeLastNameParamName} is not bound properly, its value should be {lastName}, but it is instead {nameValue}");
        }


        
    }

    [Title(EmployeeLastNameParamName + " binding Component -> Consumer")]
    [Description("This test verifies that once the Component changes the " + EmployeeLastNameParamName +
                 " it is reflected in the Consumer component ")]
    public async Task Test8()
    {
       var component = new ComponentDataBinding_Ex_LessonFinal();
        ValidateComponentProperty(component, EmployeeFirstNameParamName, typeof(string));
        ValidateComponentProperty(component, EmployeeLastNameParamName, typeof(string));
        ValidateComponentProperty(component, EmployeeFirstNameChangedName, typeof(EventCallback<string>));
        ValidateComponentProperty(component, EmployeeLastNameChangedName, typeof(EventCallback<string>));

        TestContext testContext = new TestContext();

        string lastName = "Test";

        EventCallback<string> nameChanged = EventCallback.Factory.Create<string>(this, n =>
        {
            lastName = n;
        });

        var renderedComponent = testContext.RenderComponent<ComponentDataBinding_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(EmployeeLastNameParamName, lastName),
            ComponentParameter.CreateParameter(EmployeeLastNameChangedName, nameChanged)
        );

        var input = renderedComponent.Find(".employee-last-name");

        var changedValue = "Changed value";
        await input.InputAsync(new ChangeEventArgs(){Value = changedValue});

        if (lastName != changedValue)
        {
            throw new TestRunException(
                $"{EmployeeLastNameParamName} is not bound two way. Upon changing the value of the input, the change is not reflected. Are you calling NameChanged?");
        }

        
    }*/

}