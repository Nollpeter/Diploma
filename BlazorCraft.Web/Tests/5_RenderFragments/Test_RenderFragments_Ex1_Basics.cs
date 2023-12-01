using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._5_RenderFragments;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using TestContext = Bunit.TestContext;

namespace BlazorCraft.Web.Tests._5_RenderFragments;

[TestForPage(typeof(Pages._5_RenderFragments.RenderFragments))]
public class Test_RenderFragments_Ex1_Basics : RenderFragmentsTestBase<RenderFragments_Ex1>
{
    public const string TitleFragmentName = "TitleContent";
    public const string DetailsFragmentName = "DetailsContent";
    public const string ContainerId = "ex1-container";
    
    
    [Title(TitleFragmentName+" fragment defined")]
    [Description("This test verifies that the "+TitleFragmentName+" has been defined on the component")]
    [Precondition]
    public Task GivenRenderFragments_Ex1_WhenDeclared_ThenTitleFragmentIsDefined()
    {
        ValidateRenderFragmentExists(Component, TitleFragmentName, typeof(RenderFragment));
		return Task.CompletedTask;
	}
    
    [Title(DetailsFragmentName+" fragment defined")]
    [Description("This test verifies that the "+DetailsFragmentName+" has been defined on the component")]
    [Precondition]
    public Task GivenRenderFragments_Ex1_WhenDeclared_ThenDetailsFragmentIsDefined()
    {
        ValidateRenderFragmentExists(Component, DetailsFragmentName, typeof(RenderFragment));
		return Task.CompletedTask;
	}
    
    [Title(TitleFragmentName + " is rendered")]
    [Description("This test verifies that the "+TitleFragmentName+" is rendered for the component without any interaction")]
    public Task GivenTitleFragment_WhenComponentRendered_ThenTitleFragmentIsRendered()
    {
        TestContext testContext = new TestContext();

        string titleContent = "title_" + new Random().Next(0, 1000);
        
        var titleFragment = CreateFragment(titleContent, "p");


        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex1>(parameters:
            new[] { ComponentParameter.CreateParameter(TitleFragmentName, titleFragment) });
        
        titleFragment.Invoke(new RenderTreeBuilder());
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
		return Task.CompletedTask;
	}
    
    [Title("Button click renders "+DetailsFragmentName)]
    [Description("This test verifies that the "+DetailsFragmentName+" is rendered upon clicking the button")]
    public async Task GivenButtonClick_WhenRendered_ThenDetailsFragmentIsRendered()
    {
        TestContext testContext = new TestContext();
        
        string titleContent = "title_" + new Random().Next(0, 1000);
        string detailsContent = "details_" + new Random().Next(0, 1000);

        var titleFragment = CreateFragment(titleContent, "p");
        var detailsFragment =CreateFragment(detailsContent, "p");

        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex1>(parameters:
            new[]
            {
                ComponentParameter.CreateParameter(TitleFragmentName, titleFragment),
                ComponentParameter.CreateParameter(DetailsFragmentName, detailsFragment),
            });
        
        await renderedComponent.Find($".toggle").ClickAsync(new MouseEventArgs());
        
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches($"<p>{detailsContent}</p>");
	}


    [Title("Second Button click hides "+DetailsFragmentName)]
    [Description("This test verifies that the "+DetailsFragmentName+" is hidden upon clicking the button a second time")]
    public async Task GivenSecondButtonClick_WhenRendered_ThenDetailsFragmentIsHidden()
    {
        TestContext testContext = new TestContext();
        
        string titleContent = "title_" + new Random().Next(0, 1000);
        string detailsContent = "details_" + new Random().Next(0, 1000);

        var titleFragment = CreateFragment(titleContent, "p");
        var detailsFragment =CreateFragment(detailsContent, "p");


        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex1>(parameters:
            new[]
            {
                ComponentParameter.CreateParameter(TitleFragmentName, titleFragment),
                ComponentParameter.CreateParameter(DetailsFragmentName, detailsFragment),
            });

        var toggleButton = renderedComponent.Find($".toggle");
        await toggleButton.ClickAsync(new MouseEventArgs());
        
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches($"<p>{detailsContent}</p>");

        await toggleButton.ClickAsync(new MouseEventArgs());
        
        var titleResult = renderedComponent.Find(".title").InnerHtml;
        titleResult.MarkupMatches($"<p>{titleContent}</p>");
        var detailsResult = renderedComponent.Find(".details").InnerHtml;
        detailsResult.MarkupMatches(string.Empty);
	}
    
}