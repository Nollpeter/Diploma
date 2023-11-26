using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.Routing;
using BlazorCraft.Web.Tests.Introduction;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests.Routing;

[TestForPage(typeof(Pages._5_Routing.Routing))]
public class Test_Routing_Ex1_Basics : RoutingTestBase<Routing_Ex1>
{
    public const string Route1 = "my-first-route";
    public const string Route2 = "my-resource/my-second-route";


    [Title("First route")]
    [Description("Your task is to define a route for the " + nameof(Routing_Ex1) +
                 " component, so that it can be reached in the /" + Route1 + " url")]
    public async Task<TestRunResult> Ex1_First_Route_Defined()
    {
        var routingEx1 = new Routing_Ex1();

        var routeAttributes = CheckRouteAttributesExist(routingEx1);

        CheckRouteByTemplate(routeAttributes, Route1);

        return TestRunResult.Success;
    }

    [Title("Second route")]
    [Description("Your task is to define a second route for the " + nameof(Routing_Ex1) +
                 " component, so that it can be reached in the /" + Route2 + " url as well")]
    public async Task<TestRunResult> Ex2_Second_Route_Defined()
    {
        var routingEx1 = new Routing_Ex1();
        var routeAttributes = CheckRouteAttributesExist(routingEx1);

        CheckRouteByTemplate(routeAttributes, Route2);

        return TestRunResult.Success;
    }

    [Title("Both routes defined")]
    [Description("This test verifies if " + nameof(Routing_Ex1) + " component can be reached on both /" + Route1 +
                 " and /" + Route2)]
    public async Task<TestRunResult> Ex3_All_Routes_Defined()
    {
        var routingEx1 = new Routing_Ex1();
        var routeAttributes = CheckRouteAttributesExist(routingEx1);

        CheckRouteByTemplate(routeAttributes, Route1);
        CheckRouteByTemplate(routeAttributes, Route2);

        return TestRunResult.Success;
    }
}