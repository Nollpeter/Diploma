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
    public Task GivenComponentEvents_Ex2_EventCallBack_WhenDeclared_ThenEmployeesParameterDefined()
    {
        ValidateComponentProperty(Component, EmployeesParameterName, typeof(List<Employee>));
        return Task.CompletedTask;
    }

    [ParameterDefinedTitle(EventCallBackPropertyName)]
    [ParameterDefinedDescription(EventCallBackPropertyName, typeof(EventCallback<Employee>))]
    [Precondition]
    public Task GivenComponentEvents_Ex2_EventCallBack_WhenDeclared_ThenEventCallBackParameterDefined()
    {
        ValidateComponentProperty(Component, EventCallBackPropertyName, typeof(EventCallback<Employee>));
        return Task.CompletedTask;
    }

    [Title(EmployeesParameterName + " are rendered properly")]
    [Description("This test verifies that you render the values of " + EmployeesParameterName + " properly")]
    public Task GivenComponentEvents_Ex2_EventCallBack_WhenRendered_ThenEmployeesRenderedCorrectly()
    {
        TestContext testContext = new TestContext();
        Random r = new Random();
        List<Employee> list = new()
        {
            new(r.Next(1000), $"test_{r.Next(1000)}"),
            new(r.Next(1000), $"test_{r.Next(1000)}"),
        };


        var renderedComponent = testContext.RenderComponent<ComponentEvents_Ex1_EventCallBack>(
            ComponentParameter.CreateParameter(EmployeesParameterName, list));


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

        return Task.CompletedTask;
    }

    [Title(EventCallBackPropertyName + " event is called upon clicking on delete button")]
    [Description("This test verifies that upon clicking on the delete button for a row, the event is actually called")]
    public Task GivenDeleteButton_WhenClicked_ThenEventCallBackTriggered()
    {
        TestContext testContext = new TestContext();
        Random r = new Random();
        List<Employee> list = new()
        {
            new(r.Next(1000), $"test_{r.Next(1000)}"),
            new(r.Next(1000), $"test_{r.Next(1000)}"),
        };

        Employee? calledEmployee = null;

        var renderedComponent = testContext.RenderComponent<ComponentEvents_Ex1_EventCallBack>(
            ComponentParameter.CreateParameter(EmployeesParameterName, list),
            ComponentParameter.CreateParameter(EventCallBackPropertyName,
                EventCallback.Factory.Create<Employee>(testContext, employee => calledEmployee = employee)));


        var buttons = renderedComponent.FindAll(".btn");
        if (!buttons.Any())
        {
            throw new TestRunException("There is no delete button!");
        }

        buttons.First().Click();

        if (calledEmployee == null)
        {
            throw new TestRunException(EventCallBackPropertyName + " was not called upon clicking the delete button");
        }

        if (calledEmployee != list[0])
        {
            throw new TestRunException(EventCallBackPropertyName + " was called but not with the correct employee");
        }

        return Task.CompletedTask;
    }
}