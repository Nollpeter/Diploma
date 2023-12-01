using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using MudBlazor.Services;
using NSubstitute;

namespace BlazorCraft.Web.Tests._11_Exam;

public abstract class ExamTestBase<TComponent> : ComponentTestBase<TComponent> where TComponent : ComponentBase, new()
{
    protected List<(string methodName, object[] args)> jsMethodCalls = null!;

    protected virtual async Task<TestContext> SetupTestContext()
    {
        TestContext ctx = new TestContext();
        ctx.Services.AddMudServices();
        ctx.JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        ctx.JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        await SetupMockJsRuntime(ctx);
        return ctx;
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
                return await new ValueTask<IJSVoidResult>(new DummyJSVoidResult());
            });
        testContext.Services.AddSingleton(runtime);
        return Task.CompletedTask;
    }

    private class DummyJSVoidResult : IJSVoidResult
    {
        // No members are needed, as IJSVoidResult is a marker interface
    }
}