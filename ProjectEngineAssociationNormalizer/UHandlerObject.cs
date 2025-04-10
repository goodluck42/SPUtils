namespace SPUtils.ProjectEngineAssociationNormalizer;

internal class UHandlerObject
{
	private bool? _isNewlyCreated;
	public FileStream? PeanFileStream { get; set; }
	public FileStream? UProjectFileStream { get; set; }
	public List<BoundProjectInfo> AllBoundProjectInfo { get; set; } = [];
	public BoundProjectInfo? LocalBoundProjectInfo { get; set; }
	public bool ForceSavePeanFile { get; set; }
	public bool IsAutodetect { get; init; }

	public bool IsNewlyCreated
	{
		get => _isNewlyCreated ?? throw new InvalidOperationException($"Member {IsNewlyCreated} is unset.");
		set => _isNewlyCreated = value;
	}

	// Current property in .uproject file
	public string? CurrentUEngineClassId { get; set; }

	public bool HasLocalBoundProjectInfo => LocalBoundProjectInfo is not null &&
	                                        !string.IsNullOrEmpty(LocalBoundProjectInfo.UserId) &&
	                                        !string.IsNullOrEmpty(LocalBoundProjectInfo.EngineId);

	public string? UEngineClassIdSetterValue { get; init; }
}