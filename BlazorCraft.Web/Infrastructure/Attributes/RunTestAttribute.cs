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
        " Parameter property with type "+ parameterType.Name+" and annotated it with the [Paramaeter] attribute"
        )
    {
        ParameterName = parameterName;
        ParameterType = parameterType;
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
    public ComponentUsedInMarkupTitleAttribute(Type componentType) : base($"{componentType} component is used in markup")
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