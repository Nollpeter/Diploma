namespace BlazorCraft.Web.Infrastructure;

public interface IAsyncLockProvider
{
    Task<AsyncLock> AcquireLockAsync();
}

public class AsyncLockProvider : IAsyncLockProvider, IAsyncDisposable
{
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

    public async Task<AsyncLock> AcquireLockAsync()
    {
        var asyncLock = new AsyncLock(_semaphoreSlim);
        await _semaphoreSlim.WaitAsync();
        return asyncLock;
    }

    public ValueTask DisposeAsync()
	{
		_semaphoreSlim.Dispose();
		return ValueTask.CompletedTask;
	}
}

public class AsyncLock : IAsyncDisposable
{
    private readonly SemaphoreSlim _semaphoreSlim;

    public AsyncLock(SemaphoreSlim semaphoreSlim)
    {
        _semaphoreSlim = semaphoreSlim;
    }

    public ValueTask DisposeAsync()
	{
		_semaphoreSlim.Release();
		return ValueTask.CompletedTask;
	}
}