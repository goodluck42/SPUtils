using System.Diagnostics.CodeAnalysis;
using SPUtils.BatGenerator;
using SPUtils.BatGenerator.Generators;

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
			case 1 when _args[0] == nameof(ProjectEngineAssociationNormalizer):
				return true;
			case 2 when _args[0] == nameof(ProjectEngineAssociationNormalizer):
				if (_args[1] == "Autodetect")
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

	public static bool IsBatGenerator()
	{
		if (_args is [nameof(BatGeneratorRunner), ..])
		{
			return true;
		}

		return false;
	}

	public static bool IsSpUtilJsonEditor([NotNullWhen(true)] out string? method, [NotNullWhen(true)] out string? value)
	{
		method = null;
		value = null;

		if (_args is [nameof(SpUtilJsonEditor), _, _, ..])
		{
			method = _args[1];
			value = _args[2];

			return true;
		}

		return false;
	}
}