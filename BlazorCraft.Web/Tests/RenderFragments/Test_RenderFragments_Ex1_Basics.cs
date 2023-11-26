using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.RehderFragments;
using BlazorCraft.Web.Tests.Introduction;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace BlazorCraft.Web.Tests.RenderFragments;

[TestForPage(typeof(Pages._4_RenderFragments.RenderFragments))]
public class Test_RenderFragments_Ex1_Basics : RenderFragmentsTestBase<RenderFragments_Ex1>
{
    public const string TitleFragmentName = "TitleContent";
    public const string DetailsFragmentName = "DetailsContent";
    public const string ContainerId = "ex1-container";
    
    
    [Title(""+DetailsFragmentName+" fragment defined")]
    [Infrastructure.Attributes.Description("This test verifies that the "+TitleFragmentName+" has been defined on the component")]
    public async Task<TestRunResult> Test1()
    {
        var renderFragmentsEx1 = new RenderFragments_Ex1();
        ValidateRenderFragmentExists(renderFragmentsEx1, TitleFragmentName, typeof(RenderFragment));
        return TestRunResult.Success;
    }
    
    [Title(""+DetailsFragmentName+" fragment defined")]
    [Infrastructure.Attributes.Description("This test verifies that the "+DetailsFragmentName+" has been defined on the component")]
    public async Task<TestRunResult> Test2()
    {
        var renderFragmentsEx1 = new RenderFragments_Ex1();
        ValidateRenderFragmentExists(renderFragmentsEx1, DetailsFragmentName, typeof(RenderFragment));
        return TestRunResult.Success;
    }
    
    [Title(TitleFragmentName + " is rendered")]
    [Infrastructure.Attributes.Description("This test verifies that the "+TitleFragmentName+" is rendered for the component without any interaction")]
    public async Task<TestRunResult> Test3()
    {
        var renderFragmentsEx1 = new RenderFragments_Ex1();
        ValidateRenderFragmentExists(renderFragmentsEx1, TitleFragmentName, typeof(RenderFragment));
        ValidateRenderFragmentExists(renderFragmentsEx1, DetailsFragmentName, typeof(RenderFragment));

        TestContext testContext = new TestContext();

        string titleContent = "title_" + new Random().Next(0, 1000);
        
        var titleFragment = CreateFragment(titleContent, "p");


        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex1>(parameters:
            new[] { ComponentParameter.CreateParameter(TitleFragmentName, titleFragment) });
        
        titleFragment.Invoke(new RenderTreeBuilder());
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");

        return TestRunResult.Success;
    }
    
    [Title("Button click renders "+DetailsFragmentName)]
    [Infrastructure.Attributes.Description("This test verifies that the "+DetailsFragmentName+" is rendered upon clicking the button")]
    public async Task<TestRunResult> Test4()
    {
        var renderFragmentsEx1 = new RenderFragments_Ex1();
        ValidateRenderFragmentExists(renderFragmentsEx1, TitleFragmentName, typeof(RenderFragment));
        ValidateRenderFragmentExists(renderFragmentsEx1, DetailsFragmentName, typeof(RenderFragment));

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
        
        renderedComponent.Find($".toggle").Click();
        
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches($"<p>{detailsContent}</p>");


        return TestRunResult.Success;
    }


    [Title("Second Button click hides "+DetailsFragmentName)]
    [Infrastructure.Attributes.Description("This test verifies that the "+DetailsFragmentName+" is hidden upon clicking the button a second time")]
    public async Task<TestRunResult> Test5()
    {
        var renderFragmentsEx1 = new RenderFragments_Ex1();
        ValidateRenderFragmentExists(renderFragmentsEx1, TitleFragmentName, typeof(RenderFragment));
        ValidateRenderFragmentExists(renderFragmentsEx1, DetailsFragmentName, typeof(RenderFragment));

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
        toggleButton.Click();
        
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches($"<p>{detailsContent}</p>");

        toggleButton.Click();
        
        var titleResult = renderedComponent.Find(".title").InnerHtml;
        titleResult.MarkupMatches($"<p>{titleContent}</p>");
        var detailsResult = renderedComponent.Find(".details").InnerHtml;
        detailsResult.MarkupMatches(string.Empty);
        

        return TestRunResult.Success;
    }
    
}