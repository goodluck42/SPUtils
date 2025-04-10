namespace SPUtils;

internal static class UtilsDetection
{
	private static string[]? _args;
	
	public static void SetArgs(string[] args) => _args ??= args;
	
	public static bool IsProjectEngineAssociationNormalizer(out string? engineClassId, out bool autodetect)
	{
		engineClassId = null;
		autodetect = false;

		if (_args is null)
		{
			return false;
		}
		
		switch (_args.Length)
		{
			case 1 when _args[0].ToLower() == "-pean":
				return true;
			case 2:
				if (_args[1].ToLower() == "-autodetect")
				{
					autodetect = true;
				}
				else
				{
					engineClassId = _args[1];
				}

				return true;
			default:
				return false;
		}
	}
}