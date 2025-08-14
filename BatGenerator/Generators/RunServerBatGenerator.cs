using System.Text;

namespace SPUtils.BatGenerator.Generators;

internal sealed class RunServerBatGenerator : SimpleBatGenerator
{
	public override void GenerateBat()
	{
		var stringBuilder = new StringBuilder();

		stringBuilder.AppendLine("@echo off");
		stringBuilder.AppendLine(@$"start {Globals.ExePath}\ShadowsPlaygroundServer.exe -log");
		stringBuilder.AppendLine("exit");

		GenerateBat("!XRunServer.bat", stringBuilder.ToString());
	}
}