using BlazorCraft.Web.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace BlazorCraft.Web;
public partial class App
{

	private bool isInitialized;

	[Inject]
	public IAppInitService AppInitService { get; set; } = default!;

	protected override async Task OnInitializedAsync()
	{
		await AppInitService.Initialize();
		isInitialized = true;
		StateHasChanged();
		await base.OnInitializedAsync();
		
	}
}