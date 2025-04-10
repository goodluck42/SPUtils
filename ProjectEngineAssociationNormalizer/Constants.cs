using System.Runtime.Versioning;

namespace SPUtils.ProjectEngineAssociationNormalizer;

[SupportedOSPlatform("windows")]
internal static class Constants
{
	public const string PeanFileName = "pean.sputil";
	public const string UProjectFileName = "ShadowsPlayground.uproject";
	public const string UProjectPropertyName = "EngineAssociation";
}