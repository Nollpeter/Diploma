using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.Routing;
using BlazorCraft.Web.Tests.Introduction;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests.Routing;

[TestForPage(typeof(Pages._5_Routing.Routing))]
public class Test_Routing_Ex2_RouteParams : RoutingTestBase<Routing_Ex2>
{
    public const string RouteParamName = "MyParameter";
    public const string Route = "route-with-parameter";

    [Title("Route defined")]
    [Description("This test verifies if the component has the /" + Route + " route defined")]
    public async Task GivenRouting_Ex2_WhenDefined_ThenIsAccessibleViaRoute()
    {
        var routingEx2 = new Routing_Ex2();
        var routeAttributes = CheckRouteAttributesExist(routingEx2);

        CheckRouteByTemplate(routeAttributes, Route);
    }

    [Title("Route parameter name")]
    [Description("This test verifies if the route parameter " + RouteParamName + " is defined with the correct name")]
    public async Task GivenRouteParameter_WhenDefined_ThenHasCorrectName()
    {
        var routingEx2 = new Routing_Ex2();
        var routeAttributes = CheckRouteAttributesExist(routingEx2);

        CheckRouteByTemplate(routeAttributes, Route);
        FindRouteParameterByName(routingEx2, RouteParamName);
    }

    [Title("Route parameter type")]
    [Description("This test verifies if the route parameter " + RouteParamName +
                 " is defined with the correct type: string")]
    public async Task GivenRouteParameter_WhenDefined_ThenHasCorrectType()
    {
        var routingEx2 = new Routing_Ex2();
        var routeAttributes = CheckRouteAttributesExist(routingEx2);

        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(routingEx2, RouteParamName);
        ValidateRouteParameterType(parameterByName, typeof(string));
    }

    [Title("Rendered output")]
    [Description("This test verifies if the component actually renders the route parameter in a <p> tag")]
    public async Task GivenRouteParameter_WhenRendered_ThenIsDisplayedInPTag()
    {
        var routingEx2 = new Routing_Ex2();
        var routeAttributes = CheckRouteAttributesExist(routingEx2);

        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(routingEx2, RouteParamName);
        ValidateRouteParameterType(parameterByName, typeof(string));

        TestContext testContext = new TestContext();
        object? paramValue = $"RandomString_{new Random().Next(0, 1000)}";
        var renderedComponent = testContext.RenderComponent<Routing_Ex2>(new[]
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