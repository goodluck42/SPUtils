namespace SPUtils.ProjectEngineAssociationNormalizer;

internal sealed class BoundProjectInfo
{
	// Combination of MachineGuid, Environment.UserDomainName, and Environment.UserName
	public string? UserId { get; set; }

	// CLSID
	public string? EngineId { get; set; }
}