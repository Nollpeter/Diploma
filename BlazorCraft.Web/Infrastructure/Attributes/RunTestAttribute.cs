namespace BlazorCraft.Web.Infrastructure.Attributes;

public class RunTestAttribute : Attribute
{
    
}

public class DescriptionAttribute : Attribute
{
    public DescriptionAttribute(string description)
    {
        Description = description;
    }

    public string Description { get; }
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