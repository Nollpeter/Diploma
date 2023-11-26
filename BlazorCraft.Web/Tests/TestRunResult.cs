using BlazorCraft.Web.Infrastructure.Attributes;

namespace BlazorCraft.Web.Tests;

public record TestRunResult(bool IsSuccessful, string? ErrorMessage)
{
    public static TestRunResult Success => new(true, null);
}

public record HtmlMarkupMismatchTestRunResult(string ErrorMessage, string ExpectedMarkup, string ActualMarkup) : TestRunResult(false, ErrorMessage);

public record PreconditionsNotMetTestRunResult(PreconditionsFailedException Exception) : TestRunResult(false, "The following exercise precondition test(s) failed: ");

public record CollectionsNotEquivalentTestRunResult(IEnumerable<object> Expected, IEnumerable<object> Actual, string Message) : TestRunResult(false, Message);