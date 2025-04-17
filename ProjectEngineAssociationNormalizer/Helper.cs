using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace SPUtils.ProjectEngineAssociationNormalizer;

internal static partial class Helper
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
		try
		{
			using var registryKey = Registry.LocalMachine.OpenSubKey(Constants.UserIdRegistryPath);

			string? machineGuid;

			if (registryKey is null ||
			    (machineGuid = registryKey.GetValue(Constants.UserIdRegistryKey)?.ToString()) is null)
			{
				machineGuid = Guid.NewGuid().ToString();
			}

			return $@"{machineGuid}:{Environment.UserDomainName}\{Environment.UserName}";
		}
		catch
		{
			return $@"{Guid.Empty.ToString()}:{Environment.UserDomainName}\{Environment.UserName}";
		}
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

	public static JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
	{
		WriteIndented = true,
		IndentSize = 1,
		IndentCharacter = '\t'
	};

	public static JsonWriterOptions JsonWriterOptions { get; } = new JsonWriterOptions
	{
		Indented = true,
		IndentSize = 1,
		IndentCharacter = '\t'
	};

	public static void ChangeEngineAssociation(UHandlerObject uObject, string engineClassId)
	{
		if (uObject.UProjectFileStream is null)
		{
			return;
		}


		uObject.UProjectFileStream.Position = 0;

		var uProjectStreamReader = new StreamReader(uObject.UProjectFileStream);
		var document = JsonDocument.Parse(uProjectStreamReader.ReadToEnd());
		var memoryStream = new MemoryStream();
		var jsonWriter = new Utf8JsonWriter(memoryStream, JsonWriterOptions);

		jsonWriter.WriteStartObject();

		foreach (var property in document.RootElement.EnumerateObject())
		{
			if (property.Name == Constants.UProjectPropertyName)
			{
				jsonWriter.WriteString(Constants.UProjectPropertyName, engineClassId);
			}
			else
			{
				property.WriteTo(jsonWriter);
			}
		}

		jsonWriter.WriteEndObject();
		jsonWriter.Flush();

		uObject.UProjectFileStream.Position = 0;

		memoryStream.WriteTo(uObject.UProjectFileStream);

		uObject.UProjectFileStream.Flush();
	}
}