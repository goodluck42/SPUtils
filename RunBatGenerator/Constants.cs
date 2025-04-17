namespace SPUtils.RunBatGenerator;

internal static class Constants
{
	public const string BatRelativePath = @"Binaries\Win64";
	public const string RunServerBatCommand = "ShadowsPlaygroundServer.exe -log";

	public const string RunClientBatCommand =
		"ShadowsPlaygroundClient.exe 127.0.0.1 -log -windowed -resx=800 -resy=600";

	public const string RunServerBatFileName = "!x_run_server.bat";
	public const string RunClientBatFileName = "!x_run_client.bat";
}