using System.Runtime.Versioning;

namespace SPUtils.ProjectEngineAssociationNormalizer;

[SupportedOSPlatform("windows")]
internal static class Constants
{
	public const string PeanFileName = "pean.sputil";
	
	public static string UProjectFileName => Globals.ProjectFileName;

	public const string UProjectPropertyName = "EngineAssociation";
	public const string UEnginesRegistryPath = @"Software\Epic Games\Unreal Engine\Builds";
}