namespace SPUtils;

internal interface ILoggable
{
	void Log(Log log);

	event Action<Log>? OnLog;
}