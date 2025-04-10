using System.Runtime.Versioning;
using System.Text.Json;

namespace SPUtils.ProjectEngineAssociationNormalizer.Handlers;

internal sealed class UNormalizeEngineClassIdHandler : UHandler
{
	public UNormalizeEngineClassIdHandler()
	{
		Order = 5;
	}

	[SupportedOSPlatform("windows")]
	public override void Handle(UHandlerObject uObject)
	{
		if (uObject.IsAutodetect)
		{
			Log(new Log
			{
				Message =
					$"{nameof(UNormalizeEngineClassIdHandler)} skipped. {nameof(uObject.IsAutodetect)} is set to true."
			});

			Next?.Handle(uObject);

			return;
		}

		string? engineClassId = null;

		if (uObject.HasLocalBoundProjectInfo)
		{
			engineClassId = uObject.LocalBoundProjectInfo!.EngineId;
		}

		if (uObject.UEngineClassIdSetterValue is not null)
		{
			engineClassId = uObject.UEngineClassIdSetterValue;
		}


		if (!Helper.IsClassId(engineClassId))
		{
			if (engineClassId is null)
			{
				Log(new Log
				{
					Message =
						"No CLSID found. Specify CLSID in cmd arguments. Example: SPUtils.exe -pean {12345678-9ABC-DEF1-2345-6789ABCDEF12}"
				});
			}
			else
			{
				Log(new Log
				{
					Message = $"{engineClassId} is not a valid CLSID."
				});
			}


			engineClassId = null;
		}

		if (engineClassId is not null)
		{
			if (uObject.UProjectFileStream is null)
			{
				Log(new Log
				{
					Message = "UProjectFileStream is null."
				});

				return;
			}


			uObject.UProjectFileStream.Position = 0;

			if (uObject.UProjectFileStream is null)
			{
				return;
			}


			uObject.UProjectFileStream.Position = 0;

			var uProjectStreamReader = new StreamReader(uObject.UProjectFileStream);
			var document = JsonDocument.Parse(uProjectStreamReader.ReadToEnd());
			var memoryStream = new MemoryStream();
			var jsonWriter = new Utf8JsonWriter(memoryStream, Helper.JsonWriterOptions);

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
			uObject.UProjectFileStream.Dispose();

			if (!uObject.HasLocalBoundProjectInfo)
			{
				uObject.AllBoundProjectInfo.Add(new BoundProjectInfo
				{
					EngineId = engineClassId,
					UserId = Helper.GetUserId()
				});
			}
			else
			{
				if (engineClassId != uObject.LocalBoundProjectInfo!.EngineId)
				{
					Log(new Log
					{
						Message = $"Set CLSID and local CLSID are different. Forced to save pean file."
					});

					uObject.LocalBoundProjectInfo!.EngineId = engineClassId;
					uObject.ForceSavePeanFile = true;
				}
			}


			Next?.Handle(uObject);
		}
		else
		{
			Log(new Log
			{
				Message = $"Failed to normalize {Constants.UProjectPropertyName}."
			});

			FireOnCompleted();
		}
	}
}