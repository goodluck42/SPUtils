namespace SPUtils.RunBatGenerator;

internal static class Constants
{
	public const string BatRelativePath = @"Binaries\Win64";
	public static string RunServerBatCommand { get; } = $"ShadowsPlaygroundServer.exe -log{Environment.NewLine}exit";

	public static string RunClientBatCommand { get; } =
		$"ShadowsPlaygroundClient.exe 127.0.0.1 -log -windowed -resx=800 -resy=600{Environment.NewLine}exit";

	public const string RunServerBatFileName = "!x_run_server.bat";
	public const string RunClientBatFileName = "!x_run_client.bat";
}