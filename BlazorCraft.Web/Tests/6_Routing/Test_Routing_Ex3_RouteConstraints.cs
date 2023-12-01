using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._6_Routing;
using Bunit;

namespace BlazorCraft.Web.Tests._6_Routing;

[TestForPage(typeof(Pages._6_Routing.Routing))]
public class Test_Routing_Ex3_RouteConstraints : RoutingTestBase<Routing_Ex3>
{
    public const string RouteParamName = "MyParameter";
    public const string Route = "route-with-parameter";

    [Title("Route defined")]
    [Description("This test verifies if the component has the /" + Route + " route defined")]
    public Task GivenRouting_Ex3_WhenDefined_ThenIsAccessibleViaRoute()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
		return Task.CompletedTask;
	}

    [Title("Route parameter name")]
    [Description("This test verifies if the route parameter " + RouteParamName + " is defined with the correct name")]
    public Task GivenRouteParameter_WhenDefined_ThenHasCorrectName()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(Component, RouteParamName);
		return Task.CompletedTask;
	}

    [Title("Route parameter type")]
    [Description("This test verifies if the route parameter " + RouteParamName +
                 " is defined with the correct type: int")]
    public Task GivenRouteParameter_WhenDefined_ThenHasCorrectType()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(Component, RouteParamName);
        ValidateRouteParameterType(parameterByName, typeof(int));
        ValidateRouteParameterConstraint(routeAttributes.First(), RouteParamName, "int");
		return Task.CompletedTask;
	}

    [Title("Rendered output")]
    [Description("This test verifies if the component actually renders the route parameter in a <p> tag")]
    public Task GivenRouteParameter_WhenRendered_ThenIsDisplayedInPTag()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(Component, RouteParamName);
        ValidateRouteParameterType(parameterByName, typeof(int));
        ValidateRouteParameterConstraint(routeAttributes.First(), RouteParamName, "int");
        {
            TestContext testContext = new TestContext();
            int paramValue = new Random().Next(0, 1000);
            var renderedComponent = testContext.RenderComponent<Routing_Ex3>(new[]
                { ComponentParameter.CreateParameter(RouteParamName, paramValue) });
            var renderedComponentMarkup = renderedComponent.Markup;
            var expectedOutput = $"<p>{paramValue}</p>";
            if (renderedComponentMarkup != expectedOutput)
            {
                throw new TestRunException(
                    $"The component does not render the parameter correctly. Expected markup: {expectedOutput}, Actual markup: {renderedComponentMarkup}");
            }
        }

		return Task.CompletedTask;
	}
}