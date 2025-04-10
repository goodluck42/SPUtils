using System.Text.Json;

namespace SPUtils.ProjectEngineAssociationNormalizer.Handlers;

/// <summary>
/// Initializes uObject.PeanFileStream and uObject.IsNewlyCreated. Creates pean file if not exists or opens existing.
/// </summary>
internal sealed class UInitConfigFileHandler : UHandler
{
	public UInitConfigFileHandler()
	{
		Order = 1;
	}

	public override void Handle(UHandlerObject uObject)
	{
		try
		{
			if (!File.Exists(Constants.PeanFileName))
			{
				uObject.PeanFileStream = File.Create(Constants.PeanFileName);
				uObject.IsNewlyCreated = true;


				uObject.PeanFileStream.Position = 0;

				var jsonWriter = new Utf8JsonWriter(uObject.PeanFileStream, Helper.JsonWriterOptions);

				jsonWriter.WriteStartArray();
				jsonWriter.WriteEndArray();
				jsonWriter.Flush();
			}
			else
			{
				uObject.PeanFileStream =
					File.Open(Constants.PeanFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

				uObject.IsNewlyCreated = false;
			}

			Next?.Handle(uObject);
		}
		catch (Exception ex)
		{
			FireOnException(ex);
		}
	}
}