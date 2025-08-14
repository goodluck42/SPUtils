using System.Text;

namespace SPUtils.BatGenerator.Generators;

internal sealed class RunBuildServerAndClientBatGenerator : SimpleBatGenerator
{
	public override void GenerateBat()
	{
		var engineFolder = SPUtils.Globals.Configuration[$"{Helpers.GetUserId()}:EngineFolder"];

		Console.WriteLine(SPUtils.Globals.Configuration[Helpers.GetUserId()]);
		
		if (!Helpers.TryGetProjectPath(out var projectPath))
		{
			FireOnException(new InvalidOperationException("Not in project directory."));

			return;
		}

		if (engineFolder is null)
		{
			FireOnException(new InvalidOperationException("Engine folder not detected."));

			return;
		}

		var stringBuilder = new StringBuilder();

		stringBuilder.AppendLine("@echo off");
		stringBuilder.AppendLine(@"echo ""[------------------------------ Client ------------------------------]""");
		stringBuilder.AppendLine(
			$@"call ""{engineFolder}\Engine\Build\BatchFiles\Build.bat"" ShadowsPlaygroundClient Win64 Development -Project=""{projectPath}"" -WaitMutex  -FromMSBuild");
		stringBuilder.AppendLine(@"echo ""[------------------------------ Server ------------------------------]""");
		stringBuilder.AppendLine(
			$@"call ""{engineFolder}\Engine\Build\BatchFiles\Build.bat"" ShadowsPlaygroundServer Win64 Development -Project=""{projectPath}"" -WaitMutex  -FromMSBuild");
		stringBuilder.AppendLine(@"echo ""[----------------------------- Completed ----------------------------]""");
		stringBuilder.AppendLine(@"pause");

		GenerateBat("!XBuildServerAndClient.bat", stringBuilder.ToString());
	}
}