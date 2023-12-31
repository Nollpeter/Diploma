﻿using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._5_RenderFragments;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Employee = BlazorCraft.Web.Shared._Exercises._5_RenderFragments .RenderFragments_Ex2.Employee;


namespace BlazorCraft.Web.Tests._5_RenderFragments;

[TestForPage(typeof(Pages._5_RenderFragments.RenderFragments))]
public class Test_RenderFragments_Ex2_RenderFragment_Parameter : RenderFragmentsTestBase<RenderFragments_Ex2>
{
    public const string TitleFragmentName = "TitleContent";
    public const string DetailsFragmentName = "DetailsContent";
    public const string EmployeeParamName = "EmployeeToRender";
    public const string ContainerId = "ex1-container";
    
    
    [Title(""+TitleFragmentName+" fragment defined")]
    [Description("This test verifies that the "+TitleFragmentName+" has been defined on the component")]
    [Precondition]
    public Task GivenRenderFragments_Ex2_WhenDeclared_ThenTitleFragmentIsDefined()
    {
        ValidateRenderFragmentExists(Component, TitleFragmentName, typeof(RenderFragment));
		return Task.CompletedTask;
	}
    
    [Title(""+DetailsFragmentName+" fragment defined")]
    [Description("This test verifies that the "+DetailsFragmentName+" has been defined on the component")]
    [Precondition]
    public Task GivenRenderFragments_Ex2_WhenDeclared_ThenDetailsFragmentIsDefined()
    {
        ValidateRenderFragmentExists(Component, DetailsFragmentName, typeof(RenderFragment<Employee>));
		return Task.CompletedTask;
	}
    
    [Title(""+EmployeeParamName+" parameter defined")]
    [Description("This test verifies that the "+DetailsFragmentName+" has been defined on the component")]
    [Precondition]
    public Task GivenRenderFragments_Ex2_WhenDeclared_ThenEmployeeParameterAndDetailsFragmentDefined()
    {
        ValidateComponentProperty(Component, EmployeeParamName, typeof(Employee));
		return Task.CompletedTask;
	}
    
    [Title(TitleFragmentName + " is rendered")]
    [Description("This test verifies that the "+TitleFragmentName+" is rendered for the component without any interaction")]
    public Task GivenTitleFragment_WhenComponentRendered_ThenTitleFragmentIsRendered()
    {
        TestContext testContext = new TestContext();

        string titleContent = "title_" + new Random().Next(0, 1000);
        
        var titleFragment = CreateFragment(titleContent, "p");


        var renderedComponent = testContext.RenderComponent<RenderFragments_Ex2>(parameters:
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
        await toggleButton.ClickAsync(new MouseEventArgs());
        
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches($"<p>{detailsContent(employee)}</p>");
	}

    [Title("Second Button click hides "+DetailsFragmentName)]
    [Description("This test verifies that the "+DetailsFragmentName+" is hidden upon clicking the button a second time")]
    public async Task GivenSecondButtonClick_WhenRendered_ThenDetailsFragmentIsHidden()
    {
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
        await toggleButton.ClickAsync(new MouseEventArgs());
        
        var element = renderedComponent.Find(".title").InnerHtml;
        element.MarkupMatches($"<p>{titleContent}</p>");
        var innerHtml = renderedComponent.Find(".details").InnerHtml;
        innerHtml.MarkupMatches($"<p>{detailsContent(employee)}</p>");

        await toggleButton.ClickAsync(new MouseEventArgs());
        
        var titleResult = renderedComponent.Find(".title").InnerHtml;
        titleResult.MarkupMatches($"<p>{titleContent}</p>");
        var detailsResult = renderedComponent.Find(".details").InnerHtml;
        detailsResult.MarkupMatches(string.Empty, "The details content is not hidden after second click");
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