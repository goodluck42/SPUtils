using System.Text;

namespace SPUtils.BatGenerator.Generators;

internal sealed class RunEditorBatGenerator : SimpleBatGenerator
{
	public override void GenerateBat()
	{
		var engineFolder = SPUtils.Globals.Configuration[$"{Helpers.GetUserId()}:EngineFolder"];

		if (!Helpers.TryGetProjectPath(out var projectPath))
		{
			FireOnException(new InvalidOperationException("Not in project directory."));

			return;
		}

		if (engineFolder is null)
		{
			FireOnException(new InvalidOperationException("Engine directory is not set."));

			return;
		}

		var stringBuilder = new StringBuilder();

		stringBuilder.AppendLine("@echo off");
		stringBuilder.AppendLine(
			$@"start """" /B ""{engineFolder}\Engine\Binaries\Win64\UnrealEditor.exe"" ""{projectPath}""");
		stringBuilder.AppendLine($@"exit");

		GenerateBat("!XRunEditor.bat", stringBuilder.ToString());
	}
}