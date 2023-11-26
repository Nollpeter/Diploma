using System.Diagnostics.CodeAnalysis;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Pages._3_Components;
using BlazorCraft.Web.Shared._Exercises._1_Components._1_Introduction;
using BlazorCraft.Web.Tests.Routing;
using Bunit;

namespace BlazorCraft.Web.Tests.Introduction;

[TestForPage(typeof(ComponentsIntroduction))]
public class Test_Components_Introduction_Exercise : ComponentTestBase<ComponentsIntroductionExercise>
{
    public const string NumbersParameterName = "Numbers";
    
    [StringSyntax("html")]
    public const string ListIsHiddenMarkup = "<p>The list is hidden</p>";
    
    [Title(NumbersParameterName+" parameter defined")]
    [Description("This test verifies that you have defined the "+NumbersParameterName+" Parameter property and annotated it with the [Paramaeter] attribute")]
    public async Task<TestRunResult> Test1()
    {
        var component = new ComponentsIntroductionExercise();
        ValidateComponentProperty(component, NumbersParameterName, typeof(List<int>));
        return TestRunResult.Success;
        
    }

    [Title("List is hidden by default")]
    [Description("This test verifies that by default, the List is hidden in its initial state")]
    public async Task<TestRunResult> Test2()
    {
        var component = new ComponentsIntroductionExercise();
        ValidateComponentProperty(component, NumbersParameterName, typeof(List<int>));

        TestContext testContext = new TestContext();
        Random r = new Random();
        List<int> list = new List<int>(){ r.Next(1000), r.Next(1000), r.Next(1000)};
        var renderedComponent = testContext.RenderComponent<ComponentsIntroductionExercise>(
            ComponentParameter.CreateParameter(NumbersParameterName, list));

        var innerHtml = renderedComponent.Find(".ex-container").InnerHtml;
        innerHtml.MarkupMatches("<p>The list is hidden</p>");

        return TestRunResult.Success;
    }
    
    [Title("Button click renders list")]
    [Description("This test verifies that clicking on the \"Click me\" button, the list is rendered")]
    public async Task<TestRunResult> Test3()
    {
        var component = new ComponentsIntroductionExercise();
        ValidateComponentProperty(component, NumbersParameterName, typeof(List<int>));

        TestContext testContext = new TestContext();
        Random r = new Random();
        List<int> list = new List<int>(){ r.Next(1000), r.Next(1000), r.Next(1000)};
        var renderedComponent = testContext.RenderComponent<ComponentsIntroductionExercise>(
            ComponentParameter.CreateParameter(NumbersParameterName, list));

        renderedComponent.Find(".btn").Click();
        
        var innerHtml = renderedComponent.Find(".ex-container").InnerHtml;
        innerHtml.MarkupMatches($"<p>parameter value: {list[0]}</p>"+
                                $"<p>parameter value: {list[1]}</p>"+
                                $"<p>parameter value: {list[2]}</p>");

        return TestRunResult.Success;
    }
}