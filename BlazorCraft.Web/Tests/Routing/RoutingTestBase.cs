using System.Reflection;
using BlazorCraft.Web.Infrastructure.Attributes;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web.Tests.Routing;

public class RoutingTestBase<TTestComponent> : ComponentTestBase<TTestComponent> where TTestComponent : ComponentBase
{
    protected IList<RouteAttribute> CheckRouteAttributesExist(TTestComponent component)
    {
        var customAttributes = component.GetType().GetCustomAttributes<RouteAttribute>().ToArray();
        if (!customAttributes.Any())
        {
            throw new TestRunException("There is no routing applied to the component");
        }

        return customAttributes;
    }

    protected void ExactlyOneRouteAttributeExists(IEnumerable<RouteAttribute> routeAttributes)
    {
        if (routeAttributes.Count() > 1)
        {
            throw new Exception("Multiple routes defined, this exercise requires exactly one");
        }
    }

    protected RouteAttribute CheckRouteByTemplate(IEnumerable<RouteAttribute> routeAttributes, string routeTemplate,
        string? errorMessage = null)
    {
        var route = routeAttributes.FirstOrDefault(p => p.Template.Contains(routeTemplate));
        if (route == null)
        {
            throw new TestRunException(errorMessage ?? $"Could not find route with template: /{routeTemplate}");
        }

        return route;
    }

    protected PropertyInfo FindRouteParameterByName(TTestComponent component, string parameterName)
    {
        var propertyInfos = component.GetType().GetProperties();
        var routeParamProperty = propertyInfos.FirstOrDefault(p => p.Name == parameterName);
        if (routeParamProperty == null)
        {
            throw new TestRunException($"Could not find route parameter with name: /{parameterName}");
        }

        return routeParamProperty;
    }

    protected PropertyInfo FindRouteParameterByNameInRoute(TTestComponent component, string parameterName, RouteAttribute routeAttribute)
    {
        var propertyInfos = component.GetType().GetProperties();
        var routeParamProperty = propertyInfos.FirstOrDefault(p => p.Name == parameterName);
        if (routeParamProperty == null)
        {
            throw new TestRunException($"Could not find route parameter with name: /{parameterName}");
        }

        if (!routeAttribute.Template.Contains(parameterName))
        {
            throw new TestRunException($"The route parameter is not defined in route: {routeAttribute.Template}");
        }

        return routeParamProperty;
    }
    
    protected void ValidateRouteParameterType(PropertyInfo routeParamProperty, Type expectedType)
    {
        if (routeParamProperty.PropertyType != expectedType)
        {
            throw new TestRunException(
                $"Defined type for property is incorrect: Expected: {expectedType}, Found: {routeParamProperty.PropertyType}");
        }
    }

    protected void ValidateRouteParameterConstraint(RouteAttribute routeAttribute, string routeParamName, string constraint, bool isString = false)
    {
        
        if (!routeAttribute.Template.Contains( isString ? $"{routeParamName}?" : $"{routeParamName}:{constraint}"))
        {
            Console.WriteLine(routeAttribute.Template);
            throw new TestRunException(
                $"The route parameter is not constrained to the type: {constraint}");
        }
    }

    public void AssertCorrectMarkup(string expected, string actual)
    {
        if (actual != expected)
        {
            throw new TestRunException($"The component does not render the parameter correctly. Expected markup: {expected}, Actual markup: {actual}");
        }
    }
}