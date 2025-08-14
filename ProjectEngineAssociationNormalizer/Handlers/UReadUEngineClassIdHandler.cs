using System.Text.Json;

namespace SPUtils.ProjectEngineAssociationNormalizer.Handlers;

internal sealed class UReadUEngineClassIdHandler : UHandler
{
	public override void Handle(UHandlerObject uObject)
	{
		try
		{
			uObject.UProjectFileStream =
				new FileStream(Constants.UProjectFileName, FileMode.Open, FileAccess.ReadWrite);
		
			var streamReader = new StreamReader(uObject.UProjectFileStream);
			var json = streamReader.ReadToEnd();
			var document = JsonDocument.Parse(json);
		
			foreach (var property in document.RootElement.EnumerateObject())
			{
				if (property is { Name: Constants.UProjectPropertyName, Value.ValueKind: JsonValueKind.String })
				{
					var engineClassId = property.Value.GetString();
		
					if (Helpers.IsClassId(engineClassId))
					{
						uObject.CurrentUEngineClassId = engineClassId;
					}
		
					break;
				}
			}
		
			Next?.Handle(uObject);
		}
		catch (Exception ex)
		{
			FireOnException(ex);
		}
	}
}