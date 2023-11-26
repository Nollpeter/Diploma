namespace BlazorCraft.Web.Infrastructure.Attributes;

public class TestRunException : Exception
{
    public TestRunException(string? message) : base(message)
    {
    }
}