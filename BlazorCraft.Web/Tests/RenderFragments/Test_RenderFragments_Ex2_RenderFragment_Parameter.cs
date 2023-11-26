using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._1_Components._5_RenderFragments;
using BlazorCraft.Web.Shared._Exercises._1_Components._5_RenderFragments;
using BlazorCraft.Web.Tests.Introduction;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Employee = BlazorCraft.Web.Shared._Exercises._1_Components._5_RenderFragments .RenderFragments_Ex2.Employee;


namespace BlazorCraft.Web.Tests.RenderFragments;

[TestForPage(typeof(Pages._4_RenderFragments.RenderFragments))]
public class Test_RenderFragments_Ex2_RenderFragment_Parameter : RenderFragmentsTestBase<RenderFragments_Ex2>
{
    public const string TitleFragmentName = "TitleContent";
    public const string DetailsFragmentName = "DetailsContent";
    public const string EmployeeParamName = "EmployeeToRender";
    public const string ContainerId = "ex1-container";
    
    
    [Title(""+DetailsFragmentName+" fragment defined")]
    [Description("This test verifies that the "+TitleFragmentName+" has been defined on the component")]
    public async Task<TestRunResult> Test1()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex2();
        ValidateRenderFragmentExists(renderFragmentsEx2, TitleFragmentName, typeof(RenderFragment));
        return TestRunResult.Success;
    }
    
    [Title(""+DetailsFragmentName+" fragment defined")]
    [Description("This test verifies that the "+DetailsFragmentName+" has been defined on the component")]
    public async Task<TestRunResult> Test2()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex2();
        ValidateRenderFragmentExists(renderFragmentsEx2, DetailsFragmentName, typeof(RenderFragment<Employee>));
        return TestRunResult.Success;
    }
    
    [Title(""+EmployeeParamName+" parameter defined")]
    [Description("This test verifies that the "+DetailsFragmentName+" has been defined on the component")]
    public async Task<TestRunResult> Test2_1()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex2();
        ValidateComponentProperty(renderFragmentsEx2, EmployeeParamName, typeof(Employee));
        return TestRunResult.Success;
    }
    
    [Title(TitleFragmentName + " is rendered")]
    [Description("This test verifies that the "+TitleFragmentName+" is rendered for the component without any interaction")]
    public async Task<TestRunResult> Test3()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex2();
        ValidateRenderFragmentExists(renderFragmentsEx2, TitleFragmentName, typeof(RenderFragment));
        ValidateRenderFragmentExists(renderFragmentsEx2, DetailsFragmentName, typeof(RenderFragment<Employee>));

        TestContext testContext = new TestContext();

        string titleContent = "title_" + new Random().Next(0, 1000);
        
        var titleFragment = CreateFragment(titleContent, "p");


        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex2>(parameters:
            new[] { ComponentParameter.CreateParameter(TitleFragmentName, titleFragment) });
        
        titleFragment.Invoke(new RenderTreeBuilder());
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");

        return TestRunResult.Success;
    }
    
    [Title("Button click renders "+DetailsFragmentName)]
    [Description("This test verifies that the "+DetailsFragmentName+" is rendered upon clicking the button")]
    public async Task<TestRunResult> Test4()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex2();
        ValidateRenderFragmentExists(renderFragmentsEx2, TitleFragmentName, typeof(RenderFragment));
        ValidateRenderFragmentExists(renderFragmentsEx2, DetailsFragmentName, typeof(RenderFragment<Employee>));

        TestContext testContext = new TestContext();
        
        string titleContent = "title_" + new Random().Next(0, 1000);
        
        var employee = new Employee(new Random().Next(0, 1000), "test", "test", "test");
        Func<Employee,string> detailsContent = (employee) => $"id: {employee.Id}, name: {employee.FirstName} {employee.LastName}" ;

        var titleFragment = CreateFragment(titleContent, "p");
        var detailsFragment = CreateDetailsFragment(detailsContent);
            
        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex2>(parameters:
            new[]
            {
                ComponentParameter.CreateParameter(TitleFragmentName, titleFragment),
                ComponentParameter.CreateParameter(DetailsFragmentName, detailsFragment),
                ComponentParameter.CreateParameter(EmployeeParamName, employee),
            });

        var toggleButton = renderedComponent.Find($".toggle");
        toggleButton.Click();
        
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches($"<p>{detailsContent(employee)}</p>");


        return TestRunResult.Success;
    }

    [Title("Second Button click hides "+DetailsFragmentName)]
    [Description("This test verifies that the "+DetailsFragmentName+" is hidden upon clicking the button a second time")]
    public async Task<TestRunResult> Test5()
    {
        var renderFragmentsEx2 = new RenderFragments_Ex2();
        ValidateRenderFragmentExists(renderFragmentsEx2, TitleFragmentName, typeof(RenderFragment));
        ValidateRenderFragmentExists(renderFragmentsEx2, DetailsFragmentName, typeof(RenderFragment<Employee>));

        TestContext testContext = new TestContext();
        
        string titleContent = "title_" + new Random().Next(0, 1000);
        var employee = new Employee(new Random().Next(0, 1000), "test", "test", "test");
        Func<Employee,string> detailsContent = (employee) => $"id: {employee.Id}, name: {employee.FirstName} {employee.LastName}" ;

        var titleFragment = CreateFragment(titleContent, "p");
        var detailsFragment = CreateDetailsFragment(detailsContent);
            
        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex2>(parameters:
            new[]
            {
                ComponentParameter.CreateParameter(TitleFragmentName, titleFragment),
                ComponentParameter.CreateParameter(DetailsFragmentName, detailsFragment),
                ComponentParameter.CreateParameter(EmployeeParamName, employee),
            });

        var toggleButton = renderedComponent.Find($".toggle");
        toggleButton.Click();
        
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches($"<p>{detailsContent(employee)}</p>");

        toggleButton.Click();
        
        var titleResult = renderedComponent.Find(".title").InnerHtml;
        titleResult.MarkupMatches($"<p>{titleContent}</p>");
        var detailsResult = renderedComponent.Find(".details").InnerHtml;
        detailsResult.MarkupMatches(string.Empty, "The details content is not hidden after second click");
        

        return TestRunResult.Success;
    }

    private static RenderFragment<Employee> CreateDetailsFragment(Func<Employee, string> detailsContent)
    {
        return new RenderFragment<Employee>(value => builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, detailsContent(value));
            builder.CloseElement();
        });
    }
}