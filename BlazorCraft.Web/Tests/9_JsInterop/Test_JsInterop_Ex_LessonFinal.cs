using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises._5_RenderFragments;
using BlazorCraft.Web.Shared._Exercises._9_JsInterop;
using BlazorCraft.Web.Tests._9_JsInterop;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using NSubstitute;

namespace BlazorCraft.Web.Tests.JsInterop;

[TestForPage(typeof(Pages._9_JsInterop.JsInterop))]
public class Test_JsInterop_Ex_LessonFinal : ComponentTestBase<JsInterop_Ex_LessonFinal>
{
    private readonly IJSRuntime _jsRuntime;

    IList<(string methodName, object[] args)> jsMethodCalls = new List<(string methodName, object[] args)>();

    public Test_JsInterop_Ex_LessonFinal(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    private static Type ListComponentType = typeof(RenderFragments_LessonFinal);

    public const string JsRuntimeName = "JsRuntime";
    public const string JsInvokableMethodName = "SetValueFromJs";
    public const string SetBlazorComponentMethodName = "JsInteropExerciseHelper.registerBlazorComponent";
    public const string SetEmployeesFromJsMethodName = "JsInteropExerciseHelper.callBlazorMethod";
    public const string GetEmployeesJsMethodName = "JsInteropExerciseHelper.getEmployees";

    [ComponentUsedInMarkupTitle(typeof(RenderFragments_LessonFinal))]
    [ComponentUsedInMarkupDescription(typeof(RenderFragments_LessonFinal))]
    [Precondition]
    public Task GivenJsInterop_Ex_LessonFinal_WhenDeclared_ThenRenderFragments_LessonFinalComponentUsed()
    {
        ValidateComponentUsage(Component, ListComponentType);
		return Task.CompletedTask;
	}

    [ParameterDefinedTitle(JsRuntimeName)]
    [ParameterDefinedDescription(JsRuntimeName, typeof(IJSRuntime))]
    [Precondition]
    public Task GivenJsInterop_Ex_LessonFinal_WhenDeclared_ThenJsRuntimeParameterDefined()
    {
        ValidateInjectedProperty(Component, JsRuntimeName, typeof(IJSRuntime));
		return Task.CompletedTask;
	}

    [PropertyOrFieldOfTypeDefinedTitle(typeof(DotNetObjectReference<JsInterop_Ex_LessonFinal>))]
    [PropertyOrFieldOfTypeDefinedDescription(typeof(DotNetObjectReference<JsInterop_Ex_LessonFinal>))]
    [Precondition]
    public Task GivenJsInterop_Ex_LessonFinal_WhenDeclared_ThenObjectReferenceFieldOrPropertyDefined()
    {

        ValidatePropertyOrFieldWithTypeExists(Component, typeof(DotNetObjectReference<JsInterop_Ex_LessonFinal>));
		return Task.CompletedTask;
	}

    [MethodWithPropertyDefinedTitle(nameof(JsInvokableMethodName), typeof(JSInvokableAttribute))]
    [MethodWithPropertyDefinedDescription(nameof(JsInvokableMethodName), typeof(JSInvokableAttribute))]
    [Precondition]
    public Task GivenJsInterop_Ex_LessonFinal_WhenDeclared_ThenJsInvokableMethodAttributeDefined()
    {
        ValidateMethodWithNameAndAttributeExists(Component, JsInvokableMethodName, typeof(JSInvokableAttribute));
		return Task.CompletedTask;
	}

    [Title("Employees are bound to " + nameof(RenderFragments_LessonFinal) + ".Employees")]
    [Description("This test verifies that Employees are bound to " + nameof(RenderFragments_LessonFinal) +
                 ".Employees")]
    public async Task GivenEmployees_WhenBound_ThenAreBoundToRenderFragmentsLessonFinalEmployees()
    {

        var testContext = new TestContext();
        await SetupMockJsRuntime(testContext);
        try
        {
            var renderedComponent = testContext.RenderComponent<JsInterop_Ex_LessonFinal>();
            await ManuallyRerunLifeCycleForValidations(renderedComponent);

            var tuple = jsMethodCalls.FirstOrDefault(tuple => tuple.methodName == SetBlazorComponentMethodName);
            if (tuple == default)
            {
                throw new TestRunException(
                    "The DotnetObjectReference instance was not passed to the javascript runtime");
            }

            tuple = jsMethodCalls.FirstOrDefault(tuple => tuple.methodName == SetEmployeesFromJsMethodName);
            if (tuple == default)
            {
                throw new TestRunException(
                    "The javascript method that sets employees from the javascript runtime has not been called");
            }

            var findComponent = renderedComponent.FindComponent<RenderFragments_LessonFinal>();
            var employees = await _jsRuntime.InvokeAsync<List<RenderFragments_LessonFinal.Employee>>(GetEmployeesJsMethodName);
            var componentEmployees = findComponent.Instance.Employees;
            componentEmployees.Should().FormattedBeEquivalentTo(employees, "The list of employees bound to the list component is not equal to its expected value!");
            
        }
        catch (JSException e)
        {
            if (e.Message.Contains("this.blazorComponentRef.invokeMethodAsync is not a function"))
            {
                throw new TestRunException(
                    $"The object passed to {SetBlazorComponentMethodName} is not a {typeof(DotNetObjectReference<JsInterop_Ex_LessonFinal>).Name}");
            }

            throw;
        }
        catch (NullReferenceException)
        {
            throw new TestRunException(
                "Error while binding the property Employees to the List component, you probably need to define your markup so that it only renders the list component once employees are set");
        }
        
    }

    private Task SetupMockJsRuntime(TestContext testContext)
    {
        IJSRuntime runtime = Substitute.For<IJSRuntime>();
        jsMethodCalls = new List<(string methodName, object[] args)>();
        runtime.InvokeAsync<IJSVoidResult>(Arg.Any<string>(), Arg.Any<object[]?>())
            .ReturnsForAnyArgs<ValueTask<IJSVoidResult>>(async ci =>
            {
                string identifier = ci.ArgAt<string>(0);
                object[] args = ci.ArgAt<object[]>(1);

                jsMethodCalls.Add((identifier, args));

                // Forward the call to the actual IJSRuntime instance
                return await _jsRuntime.InvokeAsync<IJSVoidResult>(identifier, args);
            });
        testContext.Services.AddSingleton(runtime);
		return Task.CompletedTask;
	}

    private async Task ManuallyRerunLifeCycleForValidations<TComponent>(IRenderedComponent<TComponent> renderedComponent) where TComponent:ComponentBase
    {
        bool isLifeCycleComplete = false;
        renderedComponent.OnAfterRender += (sender, args) => { isLifeCycleComplete = true; };
        await renderedComponent.Instance.CallOnParametersSetAsync();
        await renderedComponent.Instance.CallOnInitializedAsync();
        await WaitForState(() => isLifeCycleComplete);
    }
}