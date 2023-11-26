using System.Security.Cryptography;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._1_Components._5_RenderFragments;
using BlazorCraft.Web.Shared._Exercises._1_Components._5_RenderFragments;
using BlazorCraft.Web.Tests.Introduction;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Employee = BlazorCraft.Web.Shared._Exercises._1_Components._5_RenderFragments.RenderFragments_Ex2.Employee;

namespace BlazorCraft.Web.Tests.RenderFragments;

[TestForPage(typeof(Pages._4_RenderFragments.RenderFragments))]
public class Test_RenderFragments_Ex3_Generic : RenderFragmentsTestBase<RenderFragments_Ex3<Employee>>
{
    public const string TitleFragmentName = "TitleContent";
    public const string DetailsFragmentName = "DetailsContent";
    public const string ObjectToRenderParamName = "ObjectToRender";
    public const string ContainerId = "ex1-container";


    [Title("" + DetailsFragmentName + " fragment defined")]
    [Description("This test verifies that the " + TitleFragmentName + " has been defined on the component")]
    public async Task<TestRunResult> Test1()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex3<Employee>();
        ValidateRenderFragmentExists(renderFragmentsEx2, TitleFragmentName, typeof(RenderFragment));
        return TestRunResult.Success;
    }

    [Title("" + ObjectToRenderParamName + " parameter defined")]
    [Description("This test verifies that the " + ObjectToRenderParamName + " has been defined on the component")]
    public async Task<TestRunResult> Test2_1()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex3<Employee>();
        ValidateComponentProperty(renderFragmentsEx2, ObjectToRenderParamName, typeof(Employee));
        return TestRunResult.Success;
    }

    [Title(TitleFragmentName + " is rendered")]
    [Description("This test verifies that the " + TitleFragmentName +
                 " is rendered for the component without any interaction")]
    public async Task<TestRunResult> Test3()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex3<Employee>();
        ValidateRenderFragmentExists(renderFragmentsEx2, TitleFragmentName, typeof(RenderFragment));

        TestContext testContext = new TestContext();

        string titleContent = "title_" + new Random().Next(0, 1000);

        var titleFragment = CreateFragment(titleContent, "p");
        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex3<Employee>>(parameters:
            new[] { ComponentParameter.CreateParameter(TitleFragmentName, titleFragment) });

        titleFragment.Invoke(new RenderTreeBuilder());
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");

        return TestRunResult.Success;
    }

    [Title("Button click renders " + ObjectToRenderParamName)]
    [Description("This test verifies that the " + ObjectToRenderParamName + " is rendered upon clicking the button")]
    public async Task<TestRunResult> Test4()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex3<Employee>();
        ValidateRenderFragmentExists(renderFragmentsEx2, TitleFragmentName, typeof(RenderFragment));

        TestContext testContext = new TestContext();

        string titleContent = "title_" + new Random().Next(0, 1000);

        var employee = new Employee(new Random().Next(0, 1000), "test", "test", "test");
        var objectToRenderContent = "<ul>" +
                                    $"<li>Id: {employee.Id}</li>" +
                                    $"<li>FirstName: {employee.FirstName}</li>" +
                                    $"<li>LastName: {employee.LastName}</li>" +
                                    $"<li>Position: {employee.LastName}</li>" +
                                    "</ul>";

        var titleFragment = CreateFragment(titleContent, "p");

        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex3<Employee>>(parameters:
            new[]
            {
                ComponentParameter.CreateParameter(TitleFragmentName, titleFragment),
                ComponentParameter.CreateParameter(ObjectToRenderParamName, employee),
            });

        var toggleButton = renderedComponent.Find($".toggle");
        toggleButton.Click();

        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches($"{objectToRenderContent}");


        return TestRunResult.Success;
    }

    [Title("Second Button click hides " + DetailsFragmentName)]
    [Description("This test verifies that the " + DetailsFragmentName +
                 " is hidden upon clicking the button a second time")]
    public async Task<TestRunResult> Test5()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex3<Employee>();
        ValidateRenderFragmentExists(renderFragmentsEx2, TitleFragmentName, typeof(RenderFragment));

        TestContext testContext = new TestContext();

        string titleContent = "title_" + new Random().Next(0, 1000);
        var employee = new Employee(new Random().Next(0, 1000), "test", "test", "test");
        var objectToRenderContent = "<ul>" +
                                    $"<li>Id: {employee.Id}</li>" +
                                    $"<li>FirstName: {employee.FirstName}</li>" +
                                    $"<li>LastName: {employee.LastName}</li>" +
                                    $"<li>Position: {employee.LastName}</li>" +
                                    "</ul>";

        var titleFragment = CreateFragment(titleContent, "p");

        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex3<Employee>>(parameters:
            new[]
            {
                ComponentParameter.CreateParameter(TitleFragmentName, titleFragment),
                ComponentParameter.CreateParameter(ObjectToRenderParamName, employee),
            });

        var toggleButton = renderedComponent.Find($".toggle");
        toggleButton.Click();

        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches(objectToRenderContent);

        toggleButton.Click();

        var titleResult = renderedComponent.Find(".title").InnerHtml;
        titleResult.MarkupMatches($"<p>{titleContent}</p>");
        var detailsResult = renderedComponent.Find(".details").InnerHtml;
        detailsResult.MarkupMatches(string.Empty, "The details content is not hidden after second click");


        return TestRunResult.Success;
    }

    private record SecondType(int Id, string Value);

    [Title("Rendering with different type")]
    [Description("This test verifies that the " + ObjectToRenderParamName + " is rendered even if it is not of the type Employee")]
    public async Task<TestRunResult> Test6()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex3<SecondType>();
        ValidateRenderFragmentExists(renderFragmentsEx2, TitleFragmentName, typeof(RenderFragment));

        TestContext testContext = new TestContext();

        string titleContent = "title_" + new Random().Next(0, 1000);

        var secondType = new SecondType(new Random().Next(0, 1000), "test");
        var objectToRenderContent = "<ul>" +
                                    $"<li>Id: {secondType.Id}</li>" +
                                    $"<li>Value: {secondType.Value}</li>" +
                                    "</ul>";

        var titleFragment = CreateFragment(titleContent, "p");

        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex3<SecondType>>(parameters:
            new[]
            {
                ComponentParameter.CreateParameter(TitleFragmentName, titleFragment),
                ComponentParameter.CreateParameter(ObjectToRenderParamName, secondType),
            });

        var toggleButton = renderedComponent.Find($".toggle");
        toggleButton.Click();

        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches($"{objectToRenderContent}");


        return TestRunResult.Success;
    }

    private static RenderFragment<Employee> CreateDetailsFragment(Func<Employee, string> objectToRenderContent)
    {
        return new RenderFragment<Employee>(value => builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, objectToRenderContent(value));
            builder.CloseElement();
        });
    }
}