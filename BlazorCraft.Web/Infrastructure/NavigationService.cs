namespace BlazorCraft.Web.Infrastructure;

public interface INavigationService
{
    void RegisterPage(Type pageType, (Type? previousPage, Type? nextPage) nextAndPreviousPage);
    (Type? previousPage, Type? nextPage) GetNextAndPreviousPages(Type pageType);
}

public class NavigationService : INavigationService
{
    private readonly IDictionary<Type, (Type? previousPage, Type? nextPage)> _pagesMap = new Dictionary<Type, (Type? previousPage, Type? nextPage)>();


    public void RegisterPage(Type pageType, (Type? previousPage, Type? nextPage) nextAndPreviousPage)
    {
        _pagesMap[pageType] = nextAndPreviousPage;
    }

    public (Type? previousPage, Type? nextPage) GetNextAndPreviousPages(Type pageType)
    {
        var tryGetValue = _pagesMap.TryGetValue(pageType, out var page);
        return tryGetValue ? page : (null, null);
    }
}