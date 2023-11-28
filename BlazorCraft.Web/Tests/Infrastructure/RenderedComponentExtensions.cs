using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests._9_JsInterop;

public static class RenderedComponentExtensions
{
    public static async Task CallOnInitializedAsync<TComponent>(this TComponent component) where TComponent : class
    {
        var method = typeof(TComponent).GetMethod("OnInitializedAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        if (method == null)
        {
            throw new InvalidOperationException("The OnInitializedAsync method could not be found on the component.");
        }

        // Ensure that we're calling a method that returns a Task, as expected for OnInitializedAsync
        if (method.ReturnType != typeof(Task))
        {
            throw new InvalidOperationException("The OnInitializedAsync method does not return a Task.");
        }

        var task = (Task)method.Invoke(component, null)!;
        await task;
    }

    public static async Task CallOnParametersSetAsync<TComponent>(this TComponent component)
        where TComponent : ComponentBase
    {
        var method =
            typeof(TComponent).GetMethod("OnParametersSetAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        if (method == null)
        {
            throw new InvalidOperationException("The OnParametersSetAsync method could not be found on the component.");
        }

        // Ensure that we're calling a method that returns a Task, as expected for OnParametersSetAsync
        if (method.ReturnType != typeof(Task))
        {
            throw new InvalidOperationException("The OnParametersSetAsync method does not return a Task.");
        }

        var task = (Task)method.Invoke(component, null)!;
        await task;
    }
}