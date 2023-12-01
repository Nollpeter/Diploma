namespace BlazorCraft.Web.Infrastructure;

public interface IAppInitService
{
	Task Initialize();
}

class AppInitService : IAppInitService
{
	private readonly ITestRunnerService _testRunnerService;

	public AppInitService(ITestRunnerService testRunnerService)
	{
		_testRunnerService = testRunnerService;
	}

	public async Task Initialize()
	{
		await _testRunnerService.Initialize();
		
	}
}