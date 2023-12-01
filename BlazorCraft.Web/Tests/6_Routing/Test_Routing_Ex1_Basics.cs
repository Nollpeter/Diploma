using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._6_Routing;

namespace BlazorCraft.Web.Tests._6_Routing;

[TestForPage(typeof(Pages._6_Routing.Routing))]
public class Test_Routing_Ex1_Basics : RoutingTestBase<Routing_Ex1>
{
    public const string Route1 = "my-first-route";
    public const string Route2 = "my-resource/my-second-route";


    [Title("First route")]
    [Description("This test verifies that you defined the first route: " + nameof(Routing_Ex1) +
                 " component, so that it can be reached in the /" + Route1 + " url")]
    public Task GivenRouting_Ex1_WhenDefined_ThenIsAccessibleViaFirstRoute()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);

        CheckRouteByTemplate(routeAttributes, Route1);
        return Task.CompletedTask;
    }

    [Title("Second route")]
    [Description("This test verifies that you defined the second route: " + nameof(Routing_Ex1) +
                 " component, so that it can be reached in the /" + Route2 + " url as well")]
    public Task GivenRouting_Ex1_WhenDefined_ThenIsAccessibleViaSecondRoute()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);

        CheckRouteByTemplate(routeAttributes, Route2);
        return Task.CompletedTask;
    }

    [Title("Both routes defined")]
    [Description("This test verifies if " + nameof(Routing_Ex1) + " component can be reached on both /" + Route1 +
                 " and /" + Route2)]
    public Task GivenRouting_Ex1_WhenDefined_ThenIsAccessibleViaBothRoutes()
    {
        var routeAttributes = CheckRouteAttributesExist(Component);

        CheckRouteByTemplate(routeAttributes, Route1);
        CheckRouteByTemplate(routeAttributes, Route2);
        return Task.CompletedTask;
    }
}