using Microsoft.Extensions.Configuration;

namespace SPUtils.BatGenerator.Generators;

internal abstract class SimpleBatGenerator : IBatGenerator
{
	public abstract void GenerateBat();

	protected void GenerateBat(string batName, string batCmd)
	{
		try
		{
			if (!Helpers.IsInProjectDir())
			{
				throw new InvalidOperationException("Not in project directory.");
			}

			File.WriteAllText(batName, batCmd);

			OnOk?.Invoke(GetType());
		}
		catch (Exception ex)
		{
			OnException?.Invoke(ex);
		}
	}

	public event Action<Exception>? OnException;
	public event Action<Type>? OnOk;

	protected void FireOnException(Exception ex)
	{
		OnException?.Invoke(ex);
	}
}