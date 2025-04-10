namespace SPUtils.ProjectEngineAssociationNormalizer.Handlers;

internal sealed class UBackupProjectFileHandler : UHandler
{
	public UBackupProjectFileHandler()
	{
		Order = 4;
	}

	public override void Handle(UHandlerObject uObject)
	{
		if (uObject is { IsAutodetect: true })
		{
			Log(new Log
			{
				Message = $"{nameof(UBackupProjectFileHandler)} skipped. {nameof(uObject.IsAutodetect)} is set to true."
			});

			Next?.Handle(uObject);

			return;
		}

		try
		{
			// Check if we actually need a backup.
			if (uObject.HasLocalBoundProjectInfo &&
			    uObject.LocalBoundProjectInfo!.EngineId == uObject.CurrentUEngineClassId)
			{
				Log(new Log
				{
					Message = "Skipping backup."
				});

				Next?.Handle(uObject);

				return;
			}

			if (uObject.UProjectFileStream is null)
			{
				Log(new Log
				{
					Message = "UProjectFileStream is null."
				});

				FireOnCompleted();

				return;
			}

			if (uObject is { HasLocalBoundProjectInfo: false, UEngineClassIdSetterValue: null })
			{
				Log(new Log
				{
					Message = "Nothing to set, skipping backup."
				});

				Next?.Handle(uObject);

				return;
			}


			uObject.UProjectFileStream.Position = 0;

			var uProjectStreamReader = new StreamReader(uObject.UProjectFileStream);
			var uProjectJson = uProjectStreamReader.ReadToEnd();
			using var backupFileStream = File.Create(Helper.GetBackupFileName());
			using var backupStreamWriter = new StreamWriter(backupFileStream);

			backupStreamWriter.Write(uProjectJson);
			
			Next?.Handle(uObject);
		}
		catch (Exception ex)
		{
			FireOnException(ex);
		}
	}
}