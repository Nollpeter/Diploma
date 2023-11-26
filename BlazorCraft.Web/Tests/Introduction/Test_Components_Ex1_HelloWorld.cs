using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using BlazorCraft.Web.Pages._3_Components;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

namespace BlazorCraft.Web.Tests.Introduction;

public record TestRunResult(bool IsSuccessful, string? ErrorMessage)
{
    public static TestRunResult Success => new(true, null);
}

[TestForPage(typeof(ComponentsIntroduction))]
public class Test_Components_Ex1_HelloWorld : TestContext
{
    [Title("Existing component")]
    [Description("This test verifies that you have actually created the component with the correct name")]
    public async Task<TestRunResult> RunTest()
    {
        string componentName = "HelloWorld";
        // Megkeresi a komponens típust a név alapján.
        
        Type componentType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(p => p.Name == componentName);
        if (componentType == null)
        {
            throw new ArgumentException($"Component with name {componentName} could not be found", nameof(componentName));
        }

        await Task.Delay(3000);
        return new (true, null);
    }

    [Title("Component content valid")]
    [Description("This test verifies that the actual content of the created component is valid")]
    public async Task<TestRunResult> RunTest2()
    {
        await Task.CompletedTask;
        try
        {
            string componentName = "HelloWorld";
            Type componentType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(p => p.Name == componentName);
            if (componentType == null)
            {
                throw new ArgumentException($"Component with name {componentName} could not be found", nameof(componentName));
            }

            MethodInfo method = GetType().GetMethods().FirstOrDefault(p => p.Name == nameof(RenderComponent) && p.ContainsGenericParameters && p.GetParameters().Length == 1 && p.GetParameters().First().ParameterType == typeof(ComponentParameter[]));
            MethodInfo genericMethod = method.MakeGenericMethod(componentType);
            IRenderedComponent<IComponent> invoke =
                (IRenderedComponent<IComponent>)genericMethod.Invoke(this, new object[] { new ComponentParameter[0] });

            var invokeMarkup = invoke.Markup;
            if (invokeMarkup == "<h6>Hello World!</h6>")
            {
                return TestRunResult.Success;
            }
            else
            {
                return new(false, $"Invalid markup. Expected: <h6>Hello World!</h6>, Found: {invokeMarkup}");
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new(false, "Hiba történt a teszt végrehajtása közben");
        }
        
    }
}