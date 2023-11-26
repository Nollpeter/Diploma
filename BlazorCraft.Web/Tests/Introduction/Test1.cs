using System.Reflection;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

namespace BlazorCraft.Web.Tests.Introduction;

public class Test1 : TestContext
{
    public (bool, string?) RunTest()
    {
        string componentName = "HelloWorld";
        // Megkeresi a komponens típust a név alapján.
        
        Type componentType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(p => p.Name == componentName);
        if (componentType == null)
        {
            throw new ArgumentException($"A {componentName} komponens nem található.", nameof(componentName));
        }

        return (true, null);
    }

    public (bool, string?) RunTest2()
    {
        try
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
                (IRenderedComponent<IComponent>)genericMethod.Invoke(this, new object[] { new ComponentParameter[0] });

            var invokeMarkup = invoke.Markup;
            if (invokeMarkup == "<h6>Hello World!</h6>")
            {
                return (true, null);
            }
            else
            {
                return (false, $"Nem megfelelő markup. Elvárt: <h6>Hello World!</h6>, Kapott: {invokeMarkup}");
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (false, "Hiba történt a teszt végrehajtása közben");
        }
        
    }
}