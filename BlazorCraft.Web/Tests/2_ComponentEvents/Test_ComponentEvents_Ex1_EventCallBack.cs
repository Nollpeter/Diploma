using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Pages._2_ComponentEvents;
using BlazorCraft.Web.Shared._Exercises._2_ComponentEvents;
using Bunit;
using Microsoft.AspNetCore.Components;
using Employee = BlazorCraft.Web.Shared._Exercises._2_ComponentEvents.ComponentEvents_Ex1_EventCallBack.Employee;

namespace BlazorCraft.Web.Tests._2_ComponentEvents;

[TestForPage(typeof(ComponentEvents))]
public class Test_ComponentEvents_Ex1_EventCallBack : ComponentTestBase<ComponentEvents_Ex1_EventCallBack>
{
    public const string EmployeesParameterName = "Employees";
    public const string EventCallBackPropertyName = "ListItemDeleted";

    [ParameterDefinedTitle(EmployeesParameterName)]
    [ParameterDefinedDescription(EmployeesParameterName, typeof(List<Employee>))]
    [Precondition]
    public async Task GivenComponentEvents_Ex2_EventCallBack_WhenDeclared_ThenEmployeesParameterDefined()
    {
        var component = new ComponentEvents_Ex1_EventCallBack();
        ValidateComponentProperty(component, EmployeesParameterName, typeof(List<Employee>));
        
    }

    [ParameterDefinedTitle(EventCallBackPropertyName)]
    [ParameterDefinedDescription(EventCallBackPropertyName, typeof(EventCallback<Employee>))]
    [Precondition]
    public async Task GivenComponentEvents_Ex2_EventCallBack_WhenDeclared_ThenEventCallBackParameterDefined()
    {
        var component = new ComponentEvents_Ex1_EventCallBack();
        ValidateComponentProperty(component, EventCallBackPropertyName, typeof(EventCallback<Employee>));
        
    }

    [Title(EmployeesParameterName + " are rendered properly")]
    [Description("This test verifies that you render the values of " + EmployeesParameterName + " properly")]
    public async Task GivenComponentEvents_Ex2_EventCallBack_WhenRendered_ThenEmployeesRenderedCorrectly()
    {
        TestContext testContext = new TestContext();
        Random r = new Random();
        List<Employee> list = new List<Employee>()
        {
            new(r.Next(1000), $"test_{r.Next(1000)}"),
            new(r.Next(1000), $"test_{r.Next(1000)}"),
        };

        Employee calledEmployee = null;

        var renderedComponent = testContext.RenderComponent<ComponentEvents_Ex1_EventCallBack>(
            ComponentParameter.CreateParameter(EmployeesParameterName, list),
            ComponentParameter.CreateParameter(EventCallBackPropertyName,
                EventCallback.Factory.Create<Employee>(testContext, employee => calledEmployee = employee)));


        var tbody = renderedComponent.FindAll("tbody");
        
        tbody.MarkupMatches($"<tbody>" +
                            $"  <tr>" +
                            $"      <td>{list[0].Id}</td>" +
                            $"      <td>{list[0].Name}</td>" +
                            $"      <td><button class=\"btn btn-primary\">Delete</button></td>" +
                            $"  </tr>" +
                            $"  <tr>" +
                            $"      <td>{list[1].Id}</td>" +
                            $"      <td>{list[1].Name}</td>" +
                            $"      <td><button class=\"btn btn-primary\">Delete</button></td>" +
                            $"  </tr>" +
                            $"</tbody>");
        
        
    }

    [Title(EventCallBackPropertyName + " event is called upon clicking on delete button")]
    [Description("This test verifies that upon clicking on the delete button for a row, the event is actually called")]
    public async Task GivenDeleteButton_WhenClicked_ThenEventCallBackTriggered()
    {
        TestContext testContext = new TestContext();
        Random r = new Random();
        List<Employee> list = new List<Employee>()
        {
            new(r.Next(1000), $"test_{r.Next(1000)}"),
            new(r.Next(1000), $"test_{r.Next(1000)}"),
        };

        Employee calledEmployee = null;

        var renderedComponent = testContext.RenderComponent<ComponentEvents_Ex1_EventCallBack>(
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

        
    }
}