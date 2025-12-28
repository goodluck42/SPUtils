using System.Text;

namespace SPUtils.BatGenerator.Generators;

internal sealed class RunServerBatGenerator : SimpleBatGenerator
{
	public override void GenerateBat()
	{
		var stringBuilder = new StringBuilder();

		stringBuilder.AppendLine("@echo off");
		switch (SPUtils.Globals.CurrentProject)
		{
			case CurrentProject.Project1:
				stringBuilder.AppendLine(@$"start {Globals.ExePath}\ShadowsPlaygroundServer.exe -NOSTEAM -log");
				break;
			case CurrentProject.Project2:
				stringBuilder.AppendLine(@$"start {Globals.ExePath}\HoldItInServer.exe -NOSTEAM -log");

				break;
		}

		stringBuilder.AppendLine("exit");

		GenerateBat("!XRunServer.bat", stringBuilder.ToString());
	}
}