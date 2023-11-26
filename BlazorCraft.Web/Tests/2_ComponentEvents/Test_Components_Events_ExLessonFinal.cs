using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Pages._2_ComponentEvents;
using BlazorCraft.Web.Shared._Exercises._2_ComponentEvents;
using Bunit;
using Employee = BlazorCraft.Web.Shared._Exercises._2_ComponentEvents.ComponentEvents_Ex2_EventCallBack.Employee;


namespace BlazorCraft.Web.Tests._2_ComponentEvents;


[TestForPage(typeof(ComponentEvents))]
public class Test_Components_Events_ExLessonFinal : ComponentTestBase<ComponentEvents_ExLessonFinal>
{
    public const string EmployeesParameterName = "Employees";
    
    [ComponentUsedInMarkupTitle(typeof(ComponentEvents_Ex2_EventCallBack))]
    [ComponentUsedInMarkupDescription(typeof(ComponentEvents_Ex2_EventCallBack))]
    [Precondition]
    public async Task GivenComponentEvents_ExLessonFinal_WhenDeclared_ThenComponentEvents_Ex2_EventCallBackComponentUsed()
    {
        var component = new ComponentEvents_ExLessonFinal();
        ValidateComponentUsage(component, typeof(ComponentEvents_Ex2_EventCallBack));
        
    }

    [Title("Employee is deleted upon clicking delete")]
    [Description("This test verifies that the employee is deleted from "+EmployeesParameterName+" upon clicking delete")]
    public async Task GivenComponentEvents_ExLessonFinal_WhenDeleteClicked_ThenEmployeeIsDeleted()
    {
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

        
    }
    
    [Title("Deleted employees modified upon clicking delete")]
    [Description("This test verifies that the employee is deleted from "+EmployeesParameterName+" upon clicking delete and deleted employees are modified")]
    public async Task GivenComponentEvents_ExLessonFinal_WhenDeleteClicked_ThenEmployeeIsDeletedAndDeletedEmployeesModified()
    {
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

        var innerHtml = renderedComponent.Find(".deleted_employees ul");
        innerHtml.MarkupMatches($"<ul><li>{employee1.Name}</li></ul>");

        
    }
}