using System.Text;

namespace SPUtils.BatGenerator.Generators;

internal sealed class RunClientNoConsoleBatGenerator : SimpleBatGenerator
{
	public override void GenerateBat()
	{
		var stringBuilder = new StringBuilder();

		stringBuilder.AppendLine("@echo off");
		stringBuilder.AppendLine(
			@$"start {Globals.ExePath}\ShadowsPlaygroundClient.exe -windowed -resx=800 -resy=600");
		stringBuilder.AppendLine("exit");

		GenerateBat("!XRunClient_NoConsole.bat", stringBuilder.ToString());
	}
}