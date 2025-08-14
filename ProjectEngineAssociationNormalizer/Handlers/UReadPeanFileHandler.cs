using System.Runtime.Versioning;
using System.Text.Json;

namespace SPUtils.ProjectEngineAssociationNormalizer.Handlers;

/// <summary>
/// Reads config file and sets uObject.AllBoundProjectInfo and uObject.LocalBoundProjectInfo if possible
/// </summary>
internal sealed class UReadPeanFileHandler : UHandler
{
	public override void Handle(UHandlerObject uObject)
	{
		try
		{
			if (uObject.PeanFileStream is null)
			{
				Log(new Log
				{
					Message = $"{nameof(uObject.PeanFileStream)} is null.",
				});

				FireOnCompleted();

				return;
			}

			if (!uObject.IsNewlyCreated)
			{
				uObject.PeanFileStream.Position = 0;

				var streamReader = new StreamReader(uObject.PeanFileStream);

				try
				{
					uObject.AllBoundProjectInfo =
						JsonSerializer.Deserialize<List<BoundProjectInfo>>(streamReader.ReadToEnd()) ?? [];
				}
				catch (JsonException)
				{
					uObject.AllBoundProjectInfo = [];

					uObject.PeanFileStream.Position = 0;
					uObject.PeanFileStream.SetLength(0);

					var jsonWriter = new Utf8JsonWriter(uObject.PeanFileStream, Helpers.JsonWriterOptions);

					jsonWriter.WriteStartArray();
					jsonWriter.WriteEndArray();
					jsonWriter.Flush();

					Log(new Log
					{
						Message = $"{nameof(uObject.PeanFileStream)} contained invalid JSON. File was truncated.",
					});

					Next?.Handle(uObject);

					return;
				}

				var localUserId = Helpers.GetUserId();

				foreach (var boundProjectInfo in uObject.AllBoundProjectInfo)
				{
					if (boundProjectInfo.UserId == localUserId)
					{
						uObject.LocalBoundProjectInfo = boundProjectInfo;

						break;
					}
				}
			}
			else
			{
				uObject.AllBoundProjectInfo = [];
			}

			Next?.Handle(uObject);
		}
		catch (Exception ex)
		{
			FireOnException(ex);
		}
	}
}