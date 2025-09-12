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

		switch (SPUtils.Globals.CurrentProject)
		{
			case CurrentProject.Project1:
				stringBuilder.AppendLine(
					$@"call ""{engineFolder}\Engine\Build\BatchFiles\Build.bat"" ShadowsPlaygroundClient Win64 Development -Project=""{projectPath}"" -WaitMutex  -FromMSBuild");
				break;
			case CurrentProject.Project2:
				stringBuilder.AppendLine(
					$@"call ""{engineFolder}\Engine\Build\BatchFiles\Build.bat"" HoldItInClient Win64 Development -Project=""{projectPath}"" -WaitMutex  -FromMSBuild");
				break;
		}

		stringBuilder.AppendLine(@"echo ""[------------------------------ Server ------------------------------]""");

		switch (SPUtils.Globals.CurrentProject)
		{
			case CurrentProject.Project1:
				stringBuilder.AppendLine(
					$@"call ""{engineFolder}\Engine\Build\BatchFiles\Build.bat"" ShadowsPlaygroundServer Win64 Development -Project=""{projectPath}"" -WaitMutex  -FromMSBuild");
				break;
			case CurrentProject.Project2:
				stringBuilder.AppendLine(
					$@"call ""{engineFolder}\Engine\Build\BatchFiles\Build.bat"" HoldItInServer Win64 Development -Project=""{projectPath}"" -WaitMutex  -FromMSBuild");
				break;
		}


		stringBuilder.AppendLine(@"echo ""[----------------------------- Completed ----------------------------]""");
		stringBuilder.AppendLine(@"pause");

		GenerateBat("!XBuildServerAndClient.bat", stringBuilder.ToString());
	}
}