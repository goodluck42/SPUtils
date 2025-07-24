namespace SPUtils.RunBatGenerator;

internal static class Constants
{
	public const string BatRelativePath = @"Binaries\Win64";

	public static string RunServerBatCommand { get; } =
		$"start ShadowsPlaygroundServer.exe -log{Environment.NewLine}exit";

	public static string RunClientBatCommand { get; } =
		$"start ShadowsPlaygroundClient.exe -log -windowed -resx=800 -resy=600{Environment.NewLine}exit";

	public static string RunClientNoConsoleBatCommand { get; } =
		$"start ShadowsPlaygroundClient.exe -windowed -resx=800 -resy=600{Environment.NewLine}exit";

	public const string RunServerBatFileName = "!XRunServer.bat";
	public const string RunClientBatFileName = "!XRunClient.bat";
	public const string RunClientNoConsoleBatFileName = "!XRunClient_NoConsole.bat";
}