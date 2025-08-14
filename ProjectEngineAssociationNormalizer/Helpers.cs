using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace SPUtils.ProjectEngineAssociationNormalizer;

internal static partial class Helpers
{
	[GeneratedRegex(@"^{[A-F0-9]{8}\-[A-F0-9]{4}\-[A-F0-9]{4}\-[A-F0-9]{4}\-[A-F0-9]{12}\}")]
	private static partial Regex ClassIdRegex();

	public static bool IsClassId(string? classId)
	{
		return classId is not null && ClassIdRegex().IsMatch(classId);
	}

	public static string GetBackupFileName()
	{
		var now = DateTime.Now;

		return
			$"BACKUP_{now.Year}.{now.Month}.{now.Day} - {now.Hour}.{now.Minute}.{now.Second}.{now.Millisecond} {Constants.UProjectFileName}";
	}

	public static string GetUserId()
	{
		return SPUtils.Helpers.GetUserId();
	}

	public static IEnumerable<(string, string)> GetUnrealEngineClassIds()
	{
		try
		{
			var builds = Registry.CurrentUser.OpenSubKey(Constants.UEnginesRegistryPath);

			if (builds is not null)
			{
				var names = builds.GetValueNames();

				if (names.Length == 0)
				{
					return [];
				}

				var result = new List<(string, string)>();

				foreach (var name in names)
				{
					var value = builds.GetValue(name);

					var strValue = value?.ToString();

					if (strValue is not null)
					{
						result.Add((name, strValue));
					}
				}

				return result;
			}

			return [];
		}
		catch
		{
			return [];
		}
	}

	public static JsonSerializerOptions JsonSerializerOptions => Globals.JsonSerializerOptions;
	public static JsonWriterOptions JsonWriterOptions => Globals.JsonWriterOptions;
}