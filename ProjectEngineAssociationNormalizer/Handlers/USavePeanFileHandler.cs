using System.Text.Json;

namespace SPUtils.ProjectEngineAssociationNormalizer.Handlers;

internal sealed class USavePeanFileHandler : UHandler
{
	public override void Handle(UHandlerObject uObject)
	{
		if (uObject.IsAutodetect)
		{
			Log(new Log
			{
				Message = $"{nameof(USavePeanFileHandler)} skipped. {nameof(uObject.IsAutodetect)} is set to true."
			});

			FireOnSuccess();
			
			return;
		}

		if (uObject.PeanFileStream is null)
		{
			Log(new Log
			{
				Message = $"{nameof(uObject.PeanFileStream)} is null."
			});

			return;
		}

		if (uObject is { HasLocalBoundProjectInfo: true, ForceSavePeanFile: false })
		{
			Log(new Log
			{
				Message = "Skipped saving pean file. There is existing project info."
			});
			
			FireOnSuccess();
			
			return;
		}

		try
		{
			uObject.PeanFileStream.Position = 0;
			uObject.PeanFileStream.SetLength(0);

			var streamWriter = new StreamWriter(uObject.PeanFileStream);

			streamWriter.Write(JsonSerializer.Serialize(uObject.AllBoundProjectInfo, Helper.JsonSerializerOptions));
			streamWriter.Flush();

			uObject.PeanFileStream.Dispose();

			FireOnSuccess();
		}
		catch (Exception ex)
		{
			FireOnException(ex);

			return;
		}
	}
}