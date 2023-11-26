using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCraft.Web.Tests.Routing;

public abstract class ComponentTestBase : TestContext
{
    
    public async Task CheckPreconditions()
    {
        var preconditionMethods = GetType().GetMethods().Where(p => p.GetCustomAttribute<PreconditionAttribute>() != null);
        var exceptions = new List<PreconditionException>();
        foreach (var preconditionMethod in preconditionMethods)
        {
            var func = (Func<Task<TestRunResult>>)Delegate.CreateDelegate(typeof(Func<Task<TestRunResult>>),
                this, preconditionMethod);
            var title = preconditionMethod.GetCustomAttribute<TitleAttribute>().Title;
            try
            {
                var task = await func();
            }
            catch (Exception e)
            {
                exceptions.Add(new PreconditionException(e.Message, title));
            }
        }

        if (exceptions.Any())
        {
            throw new PreconditionsFailedException(exceptions);
        }
    }
}

public class ComponentTestBase<TTestComponent> : ComponentTestBase where TTestComponent : ComponentBase
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

    protected string ValidatePropertyOrFieldWithTypeExists(object component, Type fieldOrPropertyType)
    {
        var property = component.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(p =>
                (p.PropertyType ==(fieldOrPropertyType)));
        
        var field = component.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(p =>
                (p.FieldType ==(fieldOrPropertyType)));

        return property?.Name ?? field?.Name ?? throw new TestRunException(
            $"There is no field or property with the type {fieldOrPropertyType} defined inside the component." +
            $"Without defining it, the exercise cannot be successful!");
    }

    protected void ValidateMethodWithNameAndAttributeExists(object component, string methodName, Type attributeType)
    {
        var methodInfos = component.GetType().GetMethods();
        var method = methodInfos
            .FirstOrDefault(p => p.Name == methodName && p.GetCustomAttribute<JSInvokableAttribute>() != null);

        if (method == null)
        {
            throw new TestRunException($"The component does not have a public method with name {methodName} attributed with the [{attributeType}] attribute annotated");
        }
    }

    protected async Task WaitForState(Func<bool> condition)
    {
        while (condition() != true)
        {
            await Task.Delay(50);
        }
    }
}