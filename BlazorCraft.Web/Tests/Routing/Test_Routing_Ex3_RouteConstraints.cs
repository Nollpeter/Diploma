using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.Routing;
using BlazorCraft.Web.Tests.Introduction;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests.Routing;

[TestForPage(typeof(Pages._5_Routing.Routing))]
public class Test_Routing_Ex3_RouteConstraints : RoutingTestBase<Routing_Ex3>
{
    public const string RouteParamName = "MyParameter";
    public const string Route = "route-with-parameter";

    [Title("Route defined")]
    [Description("This test verifies if the component has the /" + Route + " route defined")]
    public async Task Ex1_Route_Defined()
    {
        var routingEx3 = new Routing_Ex3();
        var routeAttributes = CheckRouteAttributesExist(routingEx3);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        
    }

    [Title("Route parameter name")]
    [Description("This test verifies if the route parameter " + RouteParamName + " is defined with the correct name")]
    public async Task Ex2_Route_Parameter_Name()
    {
        var routingEx3 = new Routing_Ex3();
        var routeAttributes = CheckRouteAttributesExist(routingEx3);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(routingEx3, RouteParamName);
        
    }

    [Title("Route parameter type")]
    [Description("This test verifies if the route parameter " + RouteParamName +
                 " is defined with the correct type: int")]
    public async Task Ex3_Route_Parameter_Type()
    {
        var routingEx3 = new Routing_Ex3();
        var routeAttributes = CheckRouteAttributesExist(routingEx3);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(routingEx3, RouteParamName);
        ValidateRouteParameterType(parameterByName, typeof(int));
        ValidateRouteParameterConstraint(routeAttributes.First(), RouteParamName, "int");
        
    }

    [Title("Rendered output")]
    [Description("This test verifies if the component actually renders the route parameter in a <p> tag")]
    public async Task Ex4_Rendered_Output()
    {
        var routingEx3 = new Routing_Ex3();
        var routeAttributes = CheckRouteAttributesExist(routingEx3);
        ExactlyOneRouteAttributeExists(routeAttributes);
        CheckRouteByTemplate(routeAttributes, Route);
        var parameterByName = FindRouteParameterByName(routingEx3, RouteParamName);
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
            else
            {
                
            }
        }
    }
}