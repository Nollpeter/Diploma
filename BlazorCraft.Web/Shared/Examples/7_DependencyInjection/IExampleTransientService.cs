namespace BlazorCraft.Web.Shared.Examples._7_DependencyInjection;

public interface IExampleTransientService
{
    public int GetValue();    
}

public class ExampleTransientService : IExampleTransientService
{
    private static int _value = 10;
    public ExampleTransientService()
    {
        ++_value;
    }

    public int GetValue() => _value;
}

