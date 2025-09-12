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

		if (File.Exists(ProjectFileName1))
		{
			CurrentProject = CurrentProject.Project1;
		}

		if (File.Exists(ProjectFileName2))
		{
			CurrentProject = CurrentProject.Project2;
		}
	}

	public const string JsonConfigFileName = "sputil.json";
	public const string UserIdRegistryPath = @"SOFTWARE\Microsoft\Cryptography";
	public const string UserIdRegistryKey = "MachineGuid";

	private const string ProjectFileName1 = "ShadowsPlayground.uproject";
	private const string ProjectFileName2 = "HoldItIn.uproject";

	public static CurrentProject CurrentProject { get; }

	public static string ProjectFileName
	{
		get
		{
			switch (CurrentProject)
			{
				case CurrentProject.Project1:
					return ProjectFileName1;
				case CurrentProject.Project2:
					return ProjectFileName2;
				default:
					throw new InvalidOperationException("Unspecified project type.");
			}
		}
	}

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