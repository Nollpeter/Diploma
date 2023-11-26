using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.Routing;
using BlazorCraft.Web.Tests.Introduction;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests.Routing;

[TestForPage(typeof(Pages._5_Routing.Routing))]
public class Test_Routing_Ex4_OptionalRouteParams : RoutingTestBase<Routing_Ex4>
{
    public const string RouteParamName = "MyParameter";
    public const string Route = "route-with-optional-parameter";

    [Title("Route defined")]
    [Description("This test verifies if the component has the /" + Route + " route defined")]
    public async Task<TestRunResult> Ex1_Route_Defined()
    {
        var routingEx4 = new Routing_Ex4();
        var routeAttributes = CheckRouteAttributesExist(routingEx4);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        return TestRunResult.Success;
    }

    [Title("Route parameter name")]
    [Description("This test verifies if the route parameter " + RouteParamName + " is defined with the correct name")]
    public async Task<TestRunResult> Ex2_Route_Parameter_Name()
    {
        var routingEx4 = new Routing_Ex4();
        var routeAttributes = CheckRouteAttributesExist(routingEx4);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        FindRouteParameterByName(routingEx4, RouteParamName);
        return TestRunResult.Success;
    }

    [Title("Route parameter type")]
    [Description("This test verifies if the route parameter " + RouteParamName +
                 " is defined with the correct type: int")]
    public async Task<TestRunResult> Ex3_Route_Parameter_Type()
    {
        var routingEx4 = new Routing_Ex4();
        var routeAttributes = CheckRouteAttributesExist(routingEx4);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(routingEx4, RouteParamName);
        ValidateRouteParameterType(parameterByName, typeof(int?));
        ValidateRouteParameterConstraint(routeAttributes.First(), RouteParamName, "int?");
        return TestRunResult.Success;
    }

    [Title("Empty parameter output")]
    [Description("This test verifies if the parameter is left empty, the component renders accordingly")]
    public async Task<TestRunResult> Ex4_Rendered_Output_For_Empty_Value()
    {
        var routingEx4 = new Routing_Ex4();
        var routeAttributes = CheckRouteAttributesExist(routingEx4);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(routingEx4, RouteParamName);
        ValidateRouteParameterType(parameterByName, typeof(int?));
        ValidateRouteParameterConstraint(routeAttributes.First(), RouteParamName, "int?");
        TestContext testContext = new TestContext();
        int? paramValue = null;
        var renderedComponent = testContext.RenderComponent<Routing_Ex4>(new[]
            { ComponentParameter.CreateParameter(RouteParamName, paramValue) });
        var renderedComponentMarkup = renderedComponent.Markup;
        var expectedOutput = $"<p>The parameter value is empty</p>";
        if (renderedComponentMarkup != expectedOutput)
        {
            return new TestRunResult(false,
                $"The component does not render the parameter correctly. Expected markup: {expectedOutput}, Actual markup: {renderedComponentMarkup}");
        }
        else
        {
            return TestRunResult.Success;
        }
    }

    [Title("Int parameter output")]
    [Description("This test verifies if the parameter is left empty, the component renders accordingly")]
    public async Task<TestRunResult> Ex5_Rendered_Output_For_Int_Value()
    {
        var routingEx4 = new Routing_Ex4();
        var routeAttributes = CheckRouteAttributesExist(routingEx4);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(routingEx4, RouteParamName);
        ValidateRouteParameterType(parameterByName, typeof(int?));
        ValidateRouteParameterConstraint(routeAttributes.First(), RouteParamName, "int?");
        TestContext testContext = new TestContext();
        int? paramValue = new Random().Next(0, 1000);
        var renderedComponent = testContext.RenderComponent<Routing_Ex4>(new[]
            { ComponentParameter.CreateParameter(RouteParamName, paramValue) });
        var renderedComponentMarkup = renderedComponent.Markup;
        var expectedOutput = $"<p>{paramValue}</p>";
        if (renderedComponentMarkup != expectedOutput)
        {
            return new TestRunResult(false,
                $"The component does not render the parameter correctly. Expected markup: {expectedOutput}, Actual markup: {renderedComponentMarkup}");
        }
        else
        {
            return TestRunResult.Success;
        }
    }
}