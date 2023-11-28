using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._5_RenderFragments;
using Bunit;

namespace BlazorCraft.Web.Tests._5_RenderFragments;

[TestForPage(typeof(Pages._5_RenderFragments.RenderFragments))]
public class Test_RenderFragments_Ex4_Table : RenderFragmentsTestBase<RenderFragments_Ex4>
{
    [Title("Component renders properly")]
    [Description("This test verifies that the component is rendered properly")]
    public Task GivenRenderFragments_Ex4_WhenRendered_ThenRenderedProperly()
    {
        TestContext testContext = new TestContext();
        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex4>();
        renderedComponent.MarkupMatches("<h2>Items table</h2>" +
                                        "<table class=\"table\">" +
                                        "    <thead>" +
                                        "    <tr>" +
                                        "        <th>List items</th>" +
                                        "    </tr>" +
                                        "    </thead>" +
                                        "    <tbody>" +
                                        "    <tr><td>1</td></tr>" +
                                        "    <tr><td>2</td></tr>" +
                                        "    <tr><td>3</td></tr>" +
                                        "    </tbody>" +
                                        "</table>");

		return Task.CompletedTask;
	}
}