using System.Runtime.Versioning;

namespace SPUtils.ProjectEngineAssociationNormalizer;

[SupportedOSPlatform("windows")]
internal static class Constants
{
	public const string PeanFileName = "pean.sputil";

	private const string UProjectFileName1 = "ShadowsPlayground.uproject";
	private const string UProjectFileName2 = "HoldItIn.uproject";

	public static string UProjectFileName
	{
		get
		{
			if (File.Exists(UProjectFileName1))
			{
				return UProjectFileName1;
			}

			if (File.Exists(UProjectFileName2))
			{
				return UProjectFileName2;
			}

			throw new FileNotFoundException("Project file not found.");
		}
	}

	public const string UProjectPropertyName = "EngineAssociation";
	public const string UEnginesRegistryPath = @"Software\Epic Games\Unreal Engine\Builds";
}