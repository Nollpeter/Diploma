using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Pages._3_Components;
using BlazorCraft.Web.Shared._Exercises.Components._2_Events;
using BlazorCraft.Web.Tests.Routing;
using Bunit;
using Microsoft.AspNetCore.Components;
using Employee = BlazorCraft.Web.Shared._Exercises.Components._2_Events.ComponentEvents_Ex2_EventCallBack.Employee;


namespace BlazorCraft.Web.Tests.Components._2_Events;


[TestForPage(typeof(ComponentEvents))]
public class Test_Components_Events_ExLessonFinal : ComponentTestBase<ComponentEvents_ExLessonFinal>
{
    public const string EmployeesParameterName = "Employees";
    [Title(nameof(ComponentEvents_Ex2_EventCallBack) + " component is used")]
    [Description("This test verifies tha in your markup you actually used the "+nameof(ComponentEvents_Ex2_EventCallBack) + " component")]
    public async Task<TestRunResult> Test1()
    {
        var component = new ComponentEvents_ExLessonFinal();
        ValidateComponentUsage(component, typeof(ComponentEvents_Ex2_EventCallBack));
        return TestRunResult.Success;
    }

    [Title("Employee is deleted upon clicking delete")]
    [Description("This test verifies that the employee is deleted from "+EmployeesParameterName+" upon clicking delete")]
    public async Task<TestRunResult> Test2()
    {
        var component = new ComponentEvents_ExLessonFinal();
        ValidateComponentUsage(component, typeof(ComponentEvents_Ex2_EventCallBack));
        
        TestContext testContext = new TestContext();
        Random r = new Random();
        List<Employee> list = new List<Employee>()
        {
            new(r.Next(1000), $"test_{r.Next(1000)}"),
            new(r.Next(1000), $"test_{r.Next(1000)}"),
        };

        Employee calledEmployee = null;

        var renderedComponent = testContext.RenderComponent<ComponentEvents_ExLessonFinal>(
            ComponentParameter.CreateParameter(EmployeesParameterName, list));


        var buttons = renderedComponent.FindAll(".btn");
        
        buttons.FirstOrDefault().Click();

        if (list.Count > 1)
        {
            throw new TestRunException("Employee was not deleted from the grid upon clicking delete button");
        }

        return TestRunResult.Success;
    }
    
    //Deleted employees modified
    [Title("Employee is deleted upon clicking delete")]
    [Description("This test verifies that the employee is deleted from "+EmployeesParameterName+" upon clicking delete")]
    public async Task<TestRunResult> Test3()
    {
        var component = new ComponentEvents_ExLessonFinal();
        ValidateComponentUsage(component, typeof(ComponentEvents_Ex2_EventCallBack));
        
        TestContext testContext = new TestContext();
        Random r = new Random();
        var employee1 = new Employee(r.Next(1000), $"test_{r.Next(1000)}");
        List<Employee> list = new List<Employee>()
        {
            employee1,
            new(r.Next(1000), $"test_{r.Next(1000)}"),
        };

        Employee calledEmployee = null;

        var renderedComponent = testContext.RenderComponent<ComponentEvents_ExLessonFinal>(
            ComponentParameter.CreateParameter(EmployeesParameterName, list));


        var buttons = renderedComponent.FindAll(".btn");
        
        buttons.FirstOrDefault().Click();

        var innerHtml = renderedComponent.Find(".deleted_employees").InnerHtml;
        innerHtml.MarkupMatches($"<ul><li>{employee1.Name}</li></ul>");

        return TestRunResult.Success;
    }

    /*public const string EmployeesParameterName = "Employees";
    public const string EventCallBackPropertyName = "ListItemDeleted";

    [Title(EmployeesParameterName + " parameter defined")]
    [Description("This test verifies that you have defined the " + EmployeesParameterName +
                 " Parameter property and annotated it with the [Paramaeter] attribute")]
    public async Task<TestRunResult> Test1()
    {
        var component = new ComponentEvents_ExLessonFinal();
        ValidateComponentProperty(component, EmployeesParameterName, typeof(List<Employee>));
        return TestRunResult.Success;
    }

    [Title(EventCallBackPropertyName + " parameter defined")]
    [Description("This test verifies that you have defined the " + EventCallBackPropertyName +
                 " Parameter property and annotated it with the [Paramaeter] attribute")]
    public async Task<TestRunResult> Test2()
    {
        var component = new ComponentEvents_ExLessonFinal();
        ValidateComponentProperty(component, EventCallBackPropertyName, typeof(EventCallback<Employee>));
        return TestRunResult.Success;
    }

    [Title(EmployeesParameterName + " are rendered properly")]
    [Description("This test verifies that you render the values of " + EmployeesParameterName + " properly")]
    public async Task<TestRunResult> Test3()
    {
        var component = new ComponentEvents_ExLessonFinal();
        ValidateComponentProperty(component, EmployeesParameterName, typeof(List<Employee>));
        return TestRunResult.Success;
    }

    [Title(EventCallBackPropertyName + " event is called upon clicking on delete button")]
    [Description("This test verifies that upon clicking on the delete button for a row, the event is actually called")]
    public async Task<TestRunResult> Test4()
    {
        var component = new ComponentEvents_ExLessonFinal();
        ValidateComponentProperty(component, EmployeesParameterName, typeof(List<Employee>));
        ValidateComponentProperty(component, EventCallBackPropertyName, typeof(EventCallback<Employee>));

        TestContext testContext = new TestContext();
        Random r = new Random();
        List<Employee> list = new List<Employee>()
        {
            new(r.Next(1000), $"test_{r.Next(1000)}"),
            new(r.Next(1000), $"test_{r.Next(1000)}"),
        };

        Employee calledEmployee = null;

        var renderedComponent = testContext.RenderComponent<ComponentEvents_ExLessonFinal>(
            ComponentParameter.CreateParameter(EmployeesParameterName, list),
            ComponentParameter.CreateParameter(EventCallBackPropertyName,
                EventCallback.Factory.Create<Employee>(testContext, employee => calledEmployee = employee)));


        var buttons = renderedComponent.FindAll(".btn");
        buttons.FirstOrDefault().Click();

        if (calledEmployee == null)
        {
            throw new TestRunException(EventCallBackPropertyName + " was not called upon clicking the delete button");
        }

        if (calledEmployee != list[0])
        {
            throw new TestRunException(EventCallBackPropertyName + " was called but not with the correct employee");
        }

        return TestRunResult.Success;
    }*/
}