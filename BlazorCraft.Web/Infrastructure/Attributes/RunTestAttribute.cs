namespace BlazorCraft.Web.Infrastructure.Attributes;

public class RunTestAttribute : Attribute
{
}

public class ComponentUsedInMarkupDescriptionAttribute : DescriptionAttribute
{
    public ComponentUsedInMarkupDescriptionAttribute(Type componentType)
        : base($"This test verifies that the component {componentType} is used inside the razor markup")
    {
        ComponentType = componentType;
    }

    public Type ComponentType { get; }
}

public class ParameterDefinedDescriptionAttribute : DescriptionAttribute
{
    public string ParameterName { get; }
    public Type ParameterType { get; }

    public ParameterDefinedDescriptionAttribute(string parameterName, Type parameterType) : base(
        "This test verifies that you have defined the " + parameterName +
        " Parameter property with type " + parameterType.Name + " and annotated it with the [Paramaeter] attribute"
    )
    {
        ParameterName = parameterName;
        ParameterType = parameterType;
    }
}

public class PropertyOrFieldOfTypeDefinedDescriptionAttribute : DescriptionAttribute
{
    public PropertyOrFieldOfTypeDefinedDescriptionAttribute(Type parameterOrPropertyType)
        : base(
            $"This test verifies that a property or field with type {parameterOrPropertyType.Name} has been defined inside the component")
    {
        ParameterOrPropertyType = parameterOrPropertyType;
    }

    public Type ParameterOrPropertyType { get; }
}

public class MethodWithPropertyDefinedDescriptionAttribute : DescriptionAttribute
{
    public string MethodName { get; }
    public Type AttributeType { get; }

    public MethodWithPropertyDefinedDescriptionAttribute(string methodName, Type attributeType) : base(
        $"This test verifies that a method with name {methodName} attributed with the [{attributeType.Name}] has been defined inside the component")
    {
        MethodName = methodName;
        AttributeType = attributeType;
    }
}

public class DescriptionAttribute : Attribute
{
    public DescriptionAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
}

public class ComponentUsedInMarkupTitleAttribute : TitleAttribute
{
    public ComponentUsedInMarkupTitleAttribute(Type componentType) : base(
        $"{componentType} component is used in markup")
    {
        ComponentName = componentType;
    }

    public Type ComponentName { get; }
}

public class ParameterDefinedTitleAttribute : TitleAttribute
{
    public ParameterDefinedTitleAttribute(string parameterName) : base($"{parameterName} defined")
    {
        ParameterName = parameterName;
    }

    public string ParameterName { get; }
}

public class PropertyOrFieldOfTypeDefinedTitleAttribute : TitleAttribute
{
    public PropertyOrFieldOfTypeDefinedTitleAttribute(Type parameterOrPropertyType) : base(
        $"Property or field with type {parameterOrPropertyType.Name} defined")
    {
        ParameterOrPropertyType = parameterOrPropertyType;
    }

    public Type ParameterOrPropertyType { get; }
}

public class MethodWithPropertyDefinedTitleAttribute : TitleAttribute
{
    public MethodWithPropertyDefinedTitleAttribute(string methodName, Type attributeType) : base(
        $"Method {methodName} attributed with the [{attributeType.Name}] defined")
    {
    }
}

public class TitleAttribute : Attribute
{
    public TitleAttribute(string title)
    {
        Title = title;
    }

    public string Title { get; }
}

public class HintAttribute : Attribute
{
    public HintAttribute(string hint)
    {
        Hint = hint;
    }

    public string Hint { get; }
}

public class TestForPageAttribute : Attribute
{
    public TestForPageAttribute(Type page)
    {
        Page = page;
    }

    public Type Page { get; set; }
}