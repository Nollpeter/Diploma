namespace BlazorCraft.Web.Infrastructure.Attributes;

public class TestRunException : Exception
{
    public TestRunException(string? message) : base(message)
    {
    }
}

public class PreconditionException : Exception
{
    public string Title { get; }
    public PreconditionException(string? message, string title) : base(message)
    {
        Title = title;
    }
}

public class PreconditionsFailedException : Exception
{
    public PreconditionsFailedException(List<PreconditionException> preconditions)
    {
        Preconditions = preconditions;
    }

    public List<PreconditionException> Preconditions { get; }
    
}

public class CollectionsNotEquivalentException : Exception
{
    public IEnumerable<object> Expected { get; }
    public IEnumerable<object> Actual { get; }

    public CollectionsNotEquivalentException(IEnumerable<object> expected, IEnumerable<object> actual, string message) : base(message)
    {
        Expected = expected;
        Actual = actual;
    }
}