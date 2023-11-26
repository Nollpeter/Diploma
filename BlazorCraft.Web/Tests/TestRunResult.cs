namespace BlazorCraft.Web.Tests;

public record TestRunResult(bool IsSuccessful, string? ErrorMessage)
{
    public static TestRunResult Success => new(true, null);
}