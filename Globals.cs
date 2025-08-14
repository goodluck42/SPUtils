using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace SPUtils;

internal static class Globals
{
	static Globals()
	{
		var builder = new ConfigurationBuilder();

		builder.SetBasePath(Directory.GetCurrentDirectory());

		Configuration = builder.AddJsonFile(JsonConfigFileName, true).Build();
	}

	public const string JsonConfigFileName = "sputil.json";
	public const string UserIdRegistryPath = @"SOFTWARE\Microsoft\Cryptography";
	public const string UserIdRegistryKey = "MachineGuid";
	public const string ProjectFileName = "ShadowsPlayground.uproject";
	public const string EngineDirectoryPropertyName = "EngineFolder";

	public static IConfiguration Configuration { get; }

	public static JsonWriterOptions JsonWriterOptions { get; } = new()
	{
		Indented = true,
		IndentSize = 1,
		IndentCharacter = '\t'
	};
	
	public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		WriteIndented = true,
		IndentSize = 1,
		IndentCharacter = '\t'
	};
}