using System.Text;

namespace SPUtils.BatGenerator.Generators;

internal sealed class RunClientBatGenerator : SimpleBatGenerator
{
	public override void GenerateBat()
	{
		var stringBuilder = new StringBuilder();

		stringBuilder.AppendLine("@echo off");

		switch (SPUtils.Globals.CurrentProject)
		{
			case CurrentProject.Project1:
				stringBuilder.AppendLine(
					@$"start {Globals.ExePath}\ShadowsPlaygroundClient.exe -log -windowed -resx=800 -resy=600");
				break;
			case CurrentProject.Project2:
				stringBuilder.AppendLine(
					@$"start {Globals.ExePath}\HoldItInClient.exe -log -windowed -resx=800 -resy=600");

				break;
		}


		stringBuilder.AppendLine("exit");

		GenerateBat("!XRunClient.bat", stringBuilder.ToString());
	}
}