using Blazored.LocalStorage;

namespace BlazorCraft.Web.Infrastructure;

public interface IPanelStateService
{
    Task<bool> IsExpanded(string id);
    Task SaveExpansionState(string id, bool isExpanded);
}

public class PanelStateService : IPanelStateService
{
    private readonly ILocalStorageService _localStorage;

    public PanelStateService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<bool> IsExpanded(string id)
    {
        var panelStates = await _localStorage.GetItemAsync<Dictionary<string, bool>>(StorageKeys.PanelStatesKey) ?? new Dictionary<string, bool>();
        var containsKey = panelStates.ContainsKey(id);
        return containsKey && panelStates[id];
    }

    public async Task SaveExpansionState(string id, bool isExpanded)
    {
        var panelStates = await _localStorage.GetItemAsync<Dictionary<string, bool>>(StorageKeys.PanelStatesKey) ?? new Dictionary<string, bool>();
        panelStates[id] = isExpanded;
        await _localStorage.SetItemAsync(StorageKeys.PanelStatesKey, panelStates);
    }
}