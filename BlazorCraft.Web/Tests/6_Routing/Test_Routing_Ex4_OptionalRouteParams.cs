using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._6_Routing;
using Bunit;

namespace BlazorCraft.Web.Tests._6_Routing;

[TestForPage(typeof(Pages._6_Routing.Routing))]
public class Test_Routing_Ex4_OptionalRouteParams : RoutingTestBase<Routing_Ex4>
{
    public const string RouteParamName = "MyParameter";
    public const string Route = "route-with-optional-parameter";

    [Title("Route defined")]
    [Description("This test verifies if the component has the /" + Route + " route defined")]
    public Task GivenRouting_Ex4_WhenDefined_ThenIsAccessibleViaRoute()
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
        FindRouteParameterByName(Component, RouteParamName);
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
        ValidateRouteParameterType(parameterByName, typeof(int?));
        ValidateRouteParameterConstraint(routeAttributes.First(), RouteParamName, "int?");
		return Task.CompletedTask;
	}

    [Title("Empty parameter output")]
    [Description("This test verifies if the parameter is left empty, the component renders accordingly")]
    public Task GivenEmptyParameter_WhenRendered_ThenComponentRendersAccordingly()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(Component, RouteParamName);
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

		return Task.CompletedTask;
	}

    [Title("Int parameter output")]
    [Description("This test verifies if the parameter is left empty, the component renders accordingly")]
    public Task GivenIntParameter_WhenRendered_ThenComponentRendersAccordingly()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(Component, RouteParamName);
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

		return Task.CompletedTask;
	}
}