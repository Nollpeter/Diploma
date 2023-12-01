using System.Diagnostics.CodeAnalysis;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Pages._1_Components_Introduction;
using BlazorCraft.Web.Shared._Exercises._1_Components_Introduction;
using Bunit;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorCraft.Web.Tests._1_Components_Introduction;

[TestForPage(typeof(ComponentsIntroduction))]
public class Test_Components_Introduction_Ex_LessonFinal : ComponentTestBase<Components_Introduction_Ex_LessonFinal>
{
    public const string NumbersParameterName = "Numbers";
    
    [StringSyntax("html")]
    public const string ListIsHiddenMarkup = "<p>The list is hidden</p>";
    
    [ParameterDefinedTitle(NumbersParameterName)]
    [ParameterDefinedDescription(NumbersParameterName, typeof(List<int>))]
    [Precondition]
    public Task GivenComponentsIntroductionExercise_WhenRendered_ThenNumbersParamterDefined()
    {
        var component = new Components_Introduction_Ex_LessonFinal();
        ValidateComponentProperty(component, NumbersParameterName, typeof(List<int>));
		return Task.CompletedTask;
	}

    [Title("List is hidden by default")]
    [Description("This test verifies that by default, the List is hidden in its initial state")]
    public Task GivenInitialComponentState_WhenRendered_ThenListIsHidden()
    {
        TestContext testContext = new TestContext();
        Random r = new Random();
        List<int> list = new List<int>(){ r.Next(1000), r.Next(1000), r.Next(1000)};
        var renderedComponent = testContext.RenderComponent<Components_Introduction_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(NumbersParameterName, list));

        var innerHtml = renderedComponent.Find(".ex-container").InnerHtml;
        innerHtml.MarkupMatches("<p>The list is hidden</p>");
		return Task.CompletedTask;
	}
    
    [Title("Button click renders list")]
    [Description("This test verifies that clicking on the \"Click me\" button, the list is rendered")]
    public async Task GivenComponentsIntroductionExercise_WhenButtonClicked_ThenListRenders()
    {
        var component = new Components_Introduction_Ex_LessonFinal();
        ValidateComponentProperty(component, NumbersParameterName, typeof(List<int>));

        TestContext testContext = new TestContext();
        Random r = new Random();
        List<int> list = new List<int>(){ r.Next(1000), r.Next(1000), r.Next(1000)};
        var renderedComponent = testContext.RenderComponent<Components_Introduction_Ex_LessonFinal>(
            ComponentParameter.CreateParameter(NumbersParameterName, list));

        await renderedComponent.Find(".btn").ClickAsync(new MouseEventArgs());
        
        var innerHtml = renderedComponent.Find(".ex-container").InnerHtml;
        innerHtml.MarkupMatches($"<p>parameter value: {list[0]}</p>"+
                                $"<p>parameter value: {list[1]}</p>"+
                                $"<p>parameter value: {list[2]}</p>");

	}
}