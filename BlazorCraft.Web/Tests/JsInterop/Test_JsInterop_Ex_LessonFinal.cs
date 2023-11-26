using System.Collections.ObjectModel;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Shared._Exercises.DependencyInjection;
using BlazorCraft.Web.Shared._Exercises.RehderFragments;
using BlazorCraft.Web.Tests;
using BlazorCraft.Web.Tests.Routing;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using NSubstitute;
using NSubstitute.Core;

namespace BlazorCraft.Web.Shared._Exercises.JsInterop;

[TestForPage(typeof(Pages._8_JsInterop.JsInterop))]
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
    public async Task<TestRunResult> Test1()
    {
        var component = new JsInterop_Ex_LessonFinal();
        ValidateComponentUsage(component, ListComponentType);
        return TestRunResult.Success;
    }

    [ParameterDefinedTitle(JsRuntimeName)]
    [ParameterDefinedDescription(JsRuntimeName, typeof(IJSRuntime))]
    [Precondition]
    public async Task<TestRunResult> Test2()
    {
        var component = new JsInterop_Ex_LessonFinal();
        ValidateInjectedProperty(component, JsRuntimeName, typeof(IJSRuntime));
        return TestRunResult.Success;
    }

    [PropertyOrFieldOfTypeDefinedTitle(typeof(DotNetObjectReference<JsInterop_Ex_LessonFinal>))]
    [PropertyOrFieldOfTypeDefinedDescription(typeof(DotNetObjectReference<JsInterop_Ex_LessonFinal>))]
    [Precondition]
    public async Task<TestRunResult> Test3()
    {
        var component = new JsInterop_Ex_LessonFinal();

        ValidatePropertyOrFieldWithTypeExists(component, typeof(DotNetObjectReference<JsInterop_Ex_LessonFinal>));
        return TestRunResult.Success;
    }

    [MethodWithPropertyDefinedTitle(nameof(JsInvokableMethodName), typeof(JSInvokableAttribute))]
    [MethodWithPropertyDefinedDescription(nameof(JsInvokableMethodName), typeof(JSInvokableAttribute))]
    [Precondition]
    public async Task<TestRunResult> Test4()
    {
        var component = new JsInterop_Ex_LessonFinal();
        ValidateMethodWithNameAndAttributeExists(component, JsInvokableMethodName, typeof(JSInvokableAttribute));
        return TestRunResult.Success;
    }

    [Title("Employees are bound to " + nameof(RenderFragments_LessonFinal) + ".Employees")]
    [Description("This test verifies that Employees are bound to " + nameof(RenderFragments_LessonFinal) +
                 ".Employees")]
    public async Task<TestRunResult> Test6()
    {
        var component = new JsInterop_Ex_LessonFinal();

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
            return TestRunResult.Success;
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
        catch (NullReferenceException e)
        {
            return new TestRunResult(false,
                "Error while binding the property Employees to the List component, you probably need to define your markup so that it only renders the list component once employees are set");
            //TODO Ezt megoldani úgy, hogy specifikus exceptiont dobjunk erre és arra lehessen egy hintet adni, hogy kell az _isInitialized
        }
        
    }

    private async Task SetupMockJsRuntime(TestContext testContext)
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