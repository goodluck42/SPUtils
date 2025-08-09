namespace SPUtils.RunBatGenerator;

internal static class Constants
{
	public const string ExePath = @"Binaries\Win64";
	public const string BatRelativePath = @"";

	public static string RunServerBatCommand { get; } =
		$@"start {ExePath}\ShadowsPlaygroundServer.exe -log{Environment.NewLine}exit";

	public static string RunClientBatCommand { get; } =
		$@"start {ExePath}\ShadowsPlaygroundClient.exe -log -windowed -resx=800 -resy=600{Environment.NewLine}exit";

	public static string RunClientNoConsoleBatCommand { get; } =
		$@"start {ExePath}\ShadowsPlaygroundClient.exe -windowed -resx=800 -resy=600{Environment.NewLine}exit";

	public const string RunServerBatFileName = "!XRunServer.bat";
	public const string RunClientBatFileName = "!XRunClient.bat";
	public const string RunClientNoConsoleBatFileName = "!XRunClient_NoConsole.bat";
}