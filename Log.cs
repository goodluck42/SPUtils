namespace SPUtils;

internal sealed class Log
{
	public DateTime TimeStamp => DateTime.Now;
	public required string Message { get; init; }
}