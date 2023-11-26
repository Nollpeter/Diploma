using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests.Routing;

public class ComponentTestBase<TTestComponent> : TestContext where TTestComponent : ComponentBase
{
    protected void ValidateComponentProperty(object component, string parameterName, Type parameterType )
    {
        var employee = component.GetType().GetProperties()
            .FirstOrDefault(p =>
                p.Name == parameterName && (p.PropertyType ==(parameterType)));
        if (employee == null)
        {
            throw new TestRunException($"The component Property {parameterName} with the type {parameterType.Name} is not defined in the component");
        }

        var parameterAttribute = employee.GetCustomAttribute<ParameterAttribute>();
        if (parameterAttribute == null)
        {
            throw new TestRunException(
                $"The component Property {parameterName} is defined in the component, but it is not annotated with the [Parameter] attribute");
        }
    }
    
    protected void ValidateComponentUsage(TTestComponent component, Type componentThatShouldBeUsedInMarkup)
    {
        var componentType = component.GetType();
        var manifestResourceNames =componentType.Assembly.GetManifestResourceNames();
        var key = manifestResourceNames.FirstOrDefault(x => x.Contains(componentType.Name+".razor"));
        using (var stream = componentType.Assembly.GetManifestResourceStream(key))
        using (var reader = new StreamReader(stream))
        {
            var read = reader.ReadToEnd();
            var value = $"<{componentThatShouldBeUsedInMarkup.Name}";
            if (!read.Contains(value))
            {
                throw new TestRunException($"The component {componentThatShouldBeUsedInMarkup.Name} was not used inside the component markup!");
            }

        }
        
    }
    
    protected void ValidateInjectedProperty(object component, string propertyName, Type propertyType )
    {
        var employee = component.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(p =>
                p.Name == propertyName && (p.PropertyType ==(propertyType)));
        if (employee == null)
        {
            throw new TestRunException($"The component Property {propertyName} with the type {propertyType.Name} is not defined in the component");
        }

        var parameterAttribute = employee.GetCustomAttribute<InjectAttribute>();
        if (parameterAttribute == null)
        {
            throw new TestRunException(
                $"The component Property {propertyName} is defined in the component, but it is not annotated with the [Parameter] attribute");
        }
    }
}