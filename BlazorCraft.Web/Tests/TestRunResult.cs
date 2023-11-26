namespace BlazorCraft.Web.Tests;

public record TestRunResult(bool IsSuccessful, string? ErrorMessage)
{
    public static TestRunResult Success => new(true, null);
}

public record HtmlMarkupMismatchTestRunResult(string ErrorMessage, string ExpectedMarkup, string ActualMarkup) : TestRunResult(false, ErrorMessage);