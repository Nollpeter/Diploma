using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._5_RenderFragments;
using BlazorCraft.Web.Shared._Exercises._7_DependencyInjection;
using BlazorCraft.Web.Shared._Exercises.DependencyInjection;
using BlazorCraft.Web.Tests._9_JsInterop;
using Bunit;
using FluentAssertions;

namespace BlazorCraft.Web.Tests._7_DependencyInjection;



[TestForPage(typeof(Pages._7_DependencyInjection.DependencyInjection))]
public class Test_DependencyInjection_Ex_LessonFinal : ComponentTestBase<DependencyInjection_Ex_LessonFinal>
{
    private static Type ListComponentType = typeof(RenderFragments_LessonFinal);
    public const string EmployeeServiceName = "EmployeeService";
    
    [ComponentUsedInMarkupTitle(typeof(RenderFragments_LessonFinal))]
    [ComponentUsedInMarkupDescription(typeof(RenderFragments_LessonFinal))]
    [Precondition]
    public Task GivenDependencyInjection_Ex_LessonFinal_WhenDeclared_ThenRenderFragments_LessonFinalComponentUsed()
    {
        var component = new DependencyInjection_Ex_LessonFinal();
        ValidateComponentUsage(component, ListComponentType);
		return Task.CompletedTask;
	}

    [ParameterDefinedTitle(EmployeeServiceName)]
    [ParameterDefinedDescription(EmployeeServiceName, typeof(IEmployeeService))]
    [Precondition]
    public Task GivenDependencyInjection_Ex_LessonFinal_WhenDeclared_ThenEmployeeServiceParameterDefined()
    {
        var component = new DependencyInjection_Ex_LessonFinal();
        ValidateInjectedProperty(component, EmployeeServiceName, typeof(IEmployeeService));
		return Task.CompletedTask;
	}

    [Title("EmployeeService is registered as a scoped service")]
    [Description("This test verifies that the EmployeeService is registered as a scoped service")]
    [Precondition]
    public Task GivenEmployeeService_WhenRegistered_ThenItIsAsScopedService()
    {
        var component = new DependencyInjection_Ex_LessonFinal();
        ValidateComponentUsage(component, ListComponentType);
        GetAndValidateTestContext();
		return Task.CompletedTask;
	}
    
    [Title("Employees property is bound to "+nameof(RenderFragments_LessonFinal)+".Employees")]
    [Description("This test verifies that Employees property is bound to "+nameof(RenderFragments_LessonFinal)+".Employees")]
    public async Task GivenEmployeesProperty_WhenBound_ThenItIsToRenderFragmentsLessonFinalEmployees()
    {
        var testContext = GetAndValidateTestContext();
        var employeeService = testContext.Services.GetRequiredService<IEmployeeService>();
        var employees = await employeeService.GetEmployees();
        try
        {
            var renderedComponent = testContext.RenderComponent<DependencyInjection_Ex_LessonFinal>();
            
            //Let the component initialize
            await Task.Delay(1000);
            var findComponent = renderedComponent.FindComponent<RenderFragments_LessonFinal>();
            var componentEmployees = findComponent.Instance.Employees;
            componentEmployees.Should()!.FormattedBeEquivalentTo(employees,"The list of employees bound to the list component is not equal to its expected value!");
            
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e);
            throw new TestRunException(
                "Error while binding the property Employees to the List component, you probably need to define your markup so that it only renders the list component once the component has been initialized");
            //TODO Ezt megoldani úgy, hogy specifikus exceptiont dobjunk erre és arra lehessen egy hintet adni, hogy kell az _isInitialized
        }
        
    }

    private TestContext GetAndValidateTestContext()
    {
        var testContext = new TestContext();
        testContext.Services.AddEmployeeService();
        var contains = testContext.Services.FirstOrDefault(p => p.ServiceType == typeof(IEmployeeService) && p.ImplementationType == typeof(EmployeeService) && p.Lifetime == ServiceLifetime.Scoped) != null;
        if (!contains)
        {
            throw new TestRunException(
                $"The component {nameof(IEmployeeService)} has not been registered with implementation type {nameof(EmployeeService)} with scoped lifetime");
        }

        return testContext;
    }

}
