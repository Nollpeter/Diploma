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
    [Description("This test verifies that you defined the first route: " + nameof(Routing_Ex1) +
                 " component, so that it can be reached in the /" + Route1 + " url")]
    public async Task Ex1_First_Route_Defined()
    {
        var routingEx1 = new Routing_Ex1();

        var routeAttributes = CheckRouteAttributesExist(routingEx1);

        CheckRouteByTemplate(routeAttributes, Route1);

        
    }

    [Title("Second route")]
    [Description("This test verifies that you defined the second route: " + nameof(Routing_Ex1) +
                 " component, so that it can be reached in the /" + Route2 + " url as well")]
    public async Task Ex2_Second_Route_Defined()
    {
        var routingEx1 = new Routing_Ex1();
        var routeAttributes = CheckRouteAttributesExist(routingEx1);

        CheckRouteByTemplate(routeAttributes, Route2);

        
    }

    [Title("Both routes defined")]
    [Description("This test verifies if " + nameof(Routing_Ex1) + " component can be reached on both /" + Route1 +
                 " and /" + Route2)]
    public async Task Ex3_All_Routes_Defined()
    {
        var routingEx1 = new Routing_Ex1();
        var routeAttributes = CheckRouteAttributesExist(routingEx1);

        CheckRouteByTemplate(routeAttributes, Route1);
        CheckRouteByTemplate(routeAttributes, Route2);

        
    }
}