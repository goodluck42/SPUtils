namespace SPUtils.ProjectEngineAssociationNormalizer;

internal abstract class UHandler : ILoggable
{
	private UHandler? _next;

	public UHandler? Next
	{
		get => _next;
		set
		{
			// propagate events to next handler
			if (value is not null)
			{
				_next = value;
				_next.OnException = OnException;
				_next.OnCompleted = OnCompleted;
				_next.OnSuccess = OnSuccess;
				_next.OnLog = OnLog;
			}
		}
	}
	
	public abstract void Handle(UHandlerObject uObject);

	public event Action<Exception>? OnException;
	public event Action? OnSuccess;
	public event Action? OnCompleted;

	protected void FireOnException(Exception ex)
	{
		OnException?.Invoke(ex);
		FireOnCompleted();
	}

	protected void FireOnSuccess()
	{
		OnSuccess?.Invoke();
	}

	protected void FireOnCompleted()
	{
		OnCompleted?.Invoke();
	}

	public void Log(Log log)
	{
		OnLog?.Invoke(log);
	}

	public event Action<Log>? OnLog;
}