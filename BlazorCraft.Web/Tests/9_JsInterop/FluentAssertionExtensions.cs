using BlazorCraft.Web.Infrastructure.Attributes;
using FluentAssertions.Collections;
using FluentAssertions.Execution;

namespace BlazorCraft.Web.Tests._9_JsInterop;

public static class FluentAssertionExtensions
{
    public static void FormattedBeEquivalentTo<T>(this GenericCollectionAssertions<T> assertions, IEnumerable<T> expected, string message)
    {
        try
        {
            assertions.BeEquivalentTo(expected);
        }
        catch (AssertionFailedException)
        {
            throw new CollectionsNotEquivalentException((IEnumerable<object>)expected,
                (IEnumerable<object>)assertions.Subject, message);
        }
        
    }
}