namespace BlazorCraft.Web.Shared.Examples._7_DependencyInjection;

public interface IExampleScopedService
{
    public int GetValue();    
}

public class ExampleScopedService : IExampleScopedService
{
    private static int _value = 10;
    public ExampleScopedService()
    {
        ++_value;
    }

    public int GetValue() => _value;
}