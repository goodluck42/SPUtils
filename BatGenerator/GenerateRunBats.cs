namespace SPUtils.BatGenerator;

internal static class GenerateRunBats
{
	public static void Generate(bool force = false)
	{
		try
		{
			var mainDir = Path.Combine(Directory.GetCurrentDirectory(), Constants.BatRelativePath);
			var serverRunBatPath = Path.Combine(mainDir, Constants.RunServerBatFileName);
			var clientRunBatPath = Path.Combine(mainDir, Constants.RunClientBatFileName);
			var clientRunNoConsoleBatPath = Path.Combine(mainDir, Constants.RunClientNoConsoleBatFileName);

			if (!Directory.Exists(mainDir))
			{
				Directory.CreateDirectory(mainDir);
			}

			if (force)
			{
				using var serverStreamWriter =
					new StreamWriter(File.Open(serverRunBatPath, FileMode.OpenOrCreate, FileAccess.Write));
				using var clientStreamWriter =
					new StreamWriter(File.Open(clientRunBatPath, FileMode.OpenOrCreate, FileAccess.Write));
				using var clientNoConsoleStreamWriter =
					new StreamWriter(File.Open(clientRunNoConsoleBatPath, FileMode.OpenOrCreate, FileAccess.Write));

				serverStreamWriter.BaseStream.SetLength(0);
				clientStreamWriter.BaseStream.SetLength(0);
				clientNoConsoleStreamWriter.BaseStream.SetLength(0);

				serverStreamWriter.Write(Constants.RunServerBatCommand);
				clientStreamWriter.Write(Constants.RunClientBatCommand);
				clientNoConsoleStreamWriter.Write(Constants.RunClientNoConsoleBatCommand);
			}
			else
			{
				if (!File.Exists(serverRunBatPath))
				{
					using var streamWriter = new StreamWriter(File.Create(serverRunBatPath));

					streamWriter.Write(Constants.RunServerBatCommand);
				}

				if (!File.Exists(clientRunBatPath))
				{
					using var streamWriter = new StreamWriter(File.Create(clientRunBatPath));

					streamWriter.Write(Constants.RunClientBatCommand);
				}

				if (!File.Exists(clientRunNoConsoleBatPath))
				{
					using var streamWriter = new StreamWriter(File.Create(clientRunNoConsoleBatPath));

					streamWriter.Write(Constants.RunClientNoConsoleBatCommand);
				}
			}

			OnOk?.Invoke();
		}
		catch (Exception ex)
		{
			OnException?.Invoke(ex);
		}
	}

	public static event Action<Exception>? OnException;
	public static event Action? OnOk;
}