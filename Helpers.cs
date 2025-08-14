using System.Diagnostics.CodeAnalysis;
using Microsoft.Win32;

namespace SPUtils;

public class Helpers
{
	private const string FallbackUserIdFileName = "FallbackUserId";

	public static string GetUserId()
	{
		string? machineGuid = string.Empty;

		try
		{
			if (File.Exists(FallbackUserIdFileName))
			{
				machineGuid = File.ReadAllText(FallbackUserIdFileName);
			}
			else
			{
				using var registryKey = Registry.LocalMachine.OpenSubKey(Globals.UserIdRegistryPath);

				if (registryKey is not null)
				{
					machineGuid = registryKey.GetValue(Globals.UserIdRegistryKey)?.ToString();
				}

				if (machineGuid is null)
				{
					machineGuid = Guid.NewGuid().ToString();

					try
					{
						File.WriteAllText(FallbackUserIdFileName, machineGuid);
					}
					catch
					{
						// ignored
					}
				}
			}
		}
		catch
		{
			// ignored
		}

		return $@"{machineGuid}:{Environment.UserDomainName}\{Environment.UserName}";
	}

	/// <summary>
	/// Check if the project file exists.
	/// </summary>
	/// <returns></returns>
	public static bool IsInProjectDir()
	{
		return File.Exists($@"{Environment.CurrentDirectory}\{Globals.ProjectFileName}");
	}

	public static bool TryGetProjectPath([NotNullWhen(true)] out string? value)
	{
		value = null;

		if (IsInProjectDir())
		{
			value = $@"{Environment.CurrentDirectory}\{Globals.ProjectFileName}";

			return true;
		}

		return false;
	}
}