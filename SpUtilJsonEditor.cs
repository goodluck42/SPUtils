using System.Text.Json;
using System.Text.Json.Nodes;

namespace SPUtils;

internal static class SpUtilJsonEditor
{
	public static void SetEngineDirectory(string directory)
	{
		var jsonDocument = GetJsonDocument();
		var userId = Helpers.GetUserId();
		var jsonNode = JsonNode.Parse(jsonDocument.RootElement.ToString())!;

		jsonNode[userId] = new JsonObject
		{
			[Globals.EngineDirectoryPropertyName] = directory
		};

		CommitJsonChanges(jsonNode.ToJsonString());
	}

	private static JsonDocument GetJsonDocument()
	{
		return File.Exists(Globals.JsonConfigFileName)
			? JsonDocument.Parse(File.ReadAllText(Globals.JsonConfigFileName))
			: JsonDocument.Parse("{}");
	}

	private static void CommitJsonChanges(string json)
	{
		File.WriteAllText(Globals.JsonConfigFileName, json);
	}
}