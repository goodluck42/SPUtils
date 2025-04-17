using SPUtils;
using SPUtils.ProjectEngineAssociationNormalizer;
using SPUtils.ProjectEngineAssociationNormalizer.Handlers;
using SPUtils.RunBatGenerator;

UtilsDetection.SetArgs(args);

if (UtilsDetection.IsProjectEngineAssociationNormalizer(out var engineClassId, out var autodetect))
{
	var globalResetEvent = new ManualResetEvent(false);
	var uBuilder = new UHandlerBuilder();
	var first = new UInitConfigFileHandler(); // 1

	first.OnLog += log => Console.WriteLine($"[{log.TimeStamp}] {log.Message}");
	first.OnCompleted += () =>
	{
		globalResetEvent.Set();

		Console.WriteLine("Completed!");
	};
	first.OnSuccess += () =>
	{
		globalResetEvent.Set();

		Console.WriteLine("Success!");
	};

	first.OnException += ex =>
	{
		first.Log(new Log
		{
			Message = ex.ToString(),
		});
	};

	var autodetectHandler = new UAutodetectEngineClassIdHandler();

	autodetectHandler.OnMultipleUnrealEngineInstancesDetected += transferObject =>
	{
		int i = 0;

		foreach (var instance in transferObject.EngineInstances)
		{
			Console.WriteLine($"[{i++}]{instance.Item1} (path: {instance.Item2})");
		}

		try
		{
			transferObject.SelectedEngine = int.Parse(Console.ReadLine() ?? "0");
		}
		catch
		{
			Console.WriteLine("Invalid index. 0 selected by default.");

			transferObject.SelectedEngine = 0;
		}

		transferObject.ResetEvent.Set();
	};

	uBuilder.AddHandler(first);
	uBuilder.AddHandler(new UReadPeanFileHandler()); // 2
	uBuilder.AddHandler(new UReadUEngineClassIdHandler()); // 3
	uBuilder.AddHandler(new UBackupProjectFileHandler()); // 4 (skipped if autodetect is true)
	uBuilder.AddHandler(new UNormalizeEngineClassIdHandler()); // 5 (skipped if autodetect is true)
	uBuilder.AddHandler(autodetectHandler); // 6
	uBuilder.AddHandler(new USavePeanFileHandler()); // 7

	var result = uBuilder.Build();

	result.Handle(new UHandlerObject
	{
		UEngineClassIdSetterValue = engineClassId,
		IsAutodetect = autodetect
	});

	globalResetEvent.WaitOne();
}
else if (UtilsDetection.IsRunBatGenerator(out var force))
{
	GenerateRunBats.OnException += ex => Console.WriteLine(ex.Message);
	GenerateRunBats.OnOk += () => Console.WriteLine("Completed");

	GenerateRunBats.Generate(force);
}
else
{
	Console.WriteLine("Invalid command.");
}