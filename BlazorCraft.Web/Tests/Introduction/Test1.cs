using System.Reflection;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

namespace BlazorCraft.Web.Tests.Introduction;

public class Test1 : TestContext
{
    public bool RunTest()
    {
        string componentName = "HelloWorld";
        // Megkeresi a komponens típust a név alapján.
        
        Type componentType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(p => p.Name == componentName);
        if (componentType == null)
        {
            throw new ArgumentException($"A {componentName} komponens nem található.", nameof(componentName));
        }

        return true;
    }

    public bool RunTest2()
    {
        string componentName = "HelloWorld";
        // Megkeresi a komponens típust a név alapján.
        Type componentType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(p => p.Name == componentName);
        if (componentType == null)
        {
            throw new ArgumentException($"A {componentName} komponens nem található.", nameof(componentName));
        }

        // Meghívja a generikus RenderComponent függvényt a megfelelő típussal.
        MethodInfo method = GetType().GetMethods().FirstOrDefault(p => p.Name == nameof(RenderComponent) && p.ContainsGenericParameters && p.GetParameters().Length == 1 && p.GetParameters().First().ParameterType == typeof(ComponentParameter[]));
        MethodInfo genericMethod = method.MakeGenericMethod(componentType);
        IRenderedComponent<IComponent> invoke =
            (IRenderedComponent<IComponent>)genericMethod.Invoke(this, new object[0]);
        
        invoke.MarkupMatches("<h6>Hello World!</h6>");

        return true;
    }
}