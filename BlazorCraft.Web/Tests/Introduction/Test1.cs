using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

namespace BlazorCraft.Web.Tests.Introduction;

public record TestRunResult(bool IsSuccessful, string? ErrorMessage)
{
    public static TestRunResult Success => new(true, null);
}

public class Test1 : TestContext
{
    [Title("Létező komponens")]
    [Description("Ez a teszt ellenőrzni, hogy létrehozta-e a komponenst")]
    public TestRunResult RunTest()
    {
        string componentName = "HelloWorld";
        // Megkeresi a komponens típust a név alapján.
        
        Type componentType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(p => p.Name == componentName);
        if (componentType == null)
        {
            throw new ArgumentException($"A {componentName} komponens nem található.", nameof(componentName));
        }

        return new (true, null);
    }

    [Title("komponens tartalma helyes")]
    [Description("Ez a teszt ellenőrzni, hogy valóban azt tartalmazza-e a komponens, ami az elvárt")]
    public TestRunResult RunTest2()
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
                return TestRunResult.Success;
            }
            else
            {
                return new(false, $"Nem megfelelő markup. Elvárt: <h6>Hello World!</h6>, Kapott: {invokeMarkup}");
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new(false, "Hiba történt a teszt végrehajtása közben");
        }
        
    }
}