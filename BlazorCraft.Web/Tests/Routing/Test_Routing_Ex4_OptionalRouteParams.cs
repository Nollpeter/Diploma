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
    public async Task GivenRouting_Ex4_WhenDefined_ThenIsAccessibleViaRoute()
    {
        var routingEx4 = new Routing_Ex4();
        var routeAttributes = CheckRouteAttributesExist(routingEx4);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        
    }

    [Title("Route parameter name")]
    [Description("This test verifies if the route parameter " + RouteParamName + " is defined with the correct name")]
    public async Task GivenRouteParameter_WhenDefined_ThenHasCorrectName()
    {
        var routingEx4 = new Routing_Ex4();
        var routeAttributes = CheckRouteAttributesExist(routingEx4);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        FindRouteParameterByName(routingEx4, RouteParamName);
        
    }

    [Title("Route parameter type")]
    [Description("This test verifies if the route parameter " + RouteParamName +
                 " is defined with the correct type: int")]
    public async Task GivenRouteParameter_WhenDefined_ThenHasCorrectType()
    {
        var routingEx4 = new Routing_Ex4();
        var routeAttributes = CheckRouteAttributesExist(routingEx4);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(routingEx4, RouteParamName);
        ValidateRouteParameterType(parameterByName, typeof(int?));
        ValidateRouteParameterConstraint(routeAttributes.First(), RouteParamName, "int?");
        
    }

    [Title("Empty parameter output")]
    [Description("This test verifies if the parameter is left empty, the component renders accordingly")]
    public async Task GivenEmptyParameter_WhenRendered_ThenComponentRendersAccordingly()
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
            throw new TestRunException(
                $"The component does not render the parameter correctly. Expected markup: {expectedOutput}, Actual markup: {renderedComponentMarkup}");
        }
    }

    [Title("Int parameter output")]
    [Description("This test verifies if the parameter is left empty, the component renders accordingly")]
    public async Task GivenIntParameter_WhenRendered_ThenComponentRendersAccordingly()
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
            throw new TestRunException(
                $"The component does not render the parameter correctly. Expected markup: {expectedOutput}, Actual markup: {renderedComponentMarkup}");
        }
    }
}