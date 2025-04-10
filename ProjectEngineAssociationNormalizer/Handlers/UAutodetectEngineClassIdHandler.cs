using System.Collections.Immutable;
using System.Text.Json;

namespace SPUtils.ProjectEngineAssociationNormalizer.Handlers;

internal sealed class UAutodetectEngineClassIdHandler : UHandler
{
	public sealed class MultipleUnrealEngineInstancesTransferObject
	{
		public required ManualResetEvent ResetEvent { get; init; }
		public required IEnumerable<(string, string)> EngineInstances { get; init; }
		public int SelectedEngine { get; set; }
	}

	public event Action<MultipleUnrealEngineInstancesTransferObject>? OnMultipleUnrealEngineInstancesDetected;

	public override void Handle(UHandlerObject uObject)
	{
		try
		{
			if (!uObject.IsAutodetect)
			{
				Log(new Log
				{
					Message =
						$"{nameof(uObject.IsAutodetect)} is not set to true. Skipping {nameof(UAutodetectEngineClassIdHandler)}."
				});

				Next?.Handle(uObject);

				return;
			}

			if (uObject.CurrentUEngineClassId is null)
			{
				Log(new Log
				{
					Message =
						$"{nameof(uObject.CurrentUEngineClassId)} is null. .uproject file may be corrupted."
				});
			}

			if (!Helper.IsClassId(uObject.CurrentUEngineClassId))
			{
				Log(new Log
				{
					Message =
						$"{nameof(uObject.CurrentUEngineClassId)} is not CLSID. Use -pean {{12345678-9ABC-DEF1-2345-6789ABCDEF12}} to forcibly set CLSID. Trying to detect CLSID from registry..."
				});
			}

			var classIds = Helper.GetUnrealEngineClassIds().ToImmutableArray();

			if (classIds.Length == 0)
			{
				Log(new Log
				{
					Message = "There is no instances of Unreal Engine installed for current user."
				});

				FireOnCompleted();

				return;
			}

			if (classIds.Length == 1)
			{
				if (uObject.PeanFileStream is null)
				{
					Log(new Log
					{
						Message = $"{nameof(uObject.PeanFileStream)} is null."
					});

					return;
				}

				UpdateFiles(classIds[0].Item1);

				uObject.ForceSavePeanFile = true; // force update after autodetection

				Next?.Handle(uObject);

				return;
			}

			if (classIds.Length > 1)
			{
				Log(new Log
				{
					Message = "There are multiple instances of Unreal Engine detected."
				});

				var transferObject = new MultipleUnrealEngineInstancesTransferObject
				{
					ResetEvent = new ManualResetEvent(false),
					EngineInstances = classIds,
					SelectedEngine = -1
				};

				Task.Run(() =>
				{
					transferObject.ResetEvent.WaitOne();

					if (transferObject.SelectedEngine != -1)
					{
						if (transferObject.SelectedEngine >= classIds.Length)
						{
							Log(new Log
							{
								Message = "There is no such instance of engine."
							});

							FireOnCompleted();
						}
						else
						{
							UpdateFiles(classIds[transferObject.SelectedEngine].Item1);

							uObject.ForceSavePeanFile = true; // force update after autodetection

							Next?.Handle(uObject);
						}
					}
					else
					{
						Log(new Log
						{
							Message = "Invalid engine instance selected."
						});

						FireOnCompleted();
					}
				});

				OnMultipleUnrealEngineInstancesDetected?.Invoke(transferObject);
			}
		}
		catch (Exception ex)
		{
			FireOnException(ex);
		}

		return;

		void UpdateFiles(string newEngineClassId)
		{
			if (!uObject.HasLocalBoundProjectInfo)
			{
				uObject.AllBoundProjectInfo.Add(new BoundProjectInfo
				{
					EngineId = newEngineClassId,
					UserId = Helper.GetUserId()
				});
			}
			else
			{
				uObject.LocalBoundProjectInfo!.EngineId = newEngineClassId;
			}

			uObject.PeanFileStream!.Position = 0;
			uObject.PeanFileStream!.SetLength(0);

			var streamWriter = new StreamWriter(uObject.PeanFileStream);

			streamWriter.Write(JsonSerializer.Serialize(uObject.AllBoundProjectInfo, Helper.JsonSerializerOptions));
			streamWriter.Flush();

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
					jsonWriter.WriteString(Constants.UProjectPropertyName, newEngineClassId);
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
}