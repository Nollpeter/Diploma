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
            throw new TestRunException($"The component Property {parameterName} is not defined in the component");
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
        var key = manifestResourceNames.FirstOrDefault(x => x.Contains(componentType.Name));
        using (var stream = componentType.Assembly.GetManifestResourceStream(key))
        using (var reader = new StreamReader(stream))
        {
            var read = reader.ReadToEnd();
            if (!read.Contains($"<{componentThatShouldBeUsedInMarkup.Name}"))
            {
                throw new TestRunException($"The component {componentThatShouldBeUsedInMarkup.Name} was not used inside the component markup!");
            }

        }
        
    }

}