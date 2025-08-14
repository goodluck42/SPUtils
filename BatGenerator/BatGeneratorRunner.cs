namespace SPUtils.BatGenerator;

internal sealed class BatGeneratorRunner
{
	private readonly IList<IBatGenerator> _batGenerators = [];

	public void AddBatGenerator(IBatGenerator batGenerator)
	{
		batGenerator.OnException += OnException;
		batGenerator.OnOk += OnOk;

		_batGenerators.Add(batGenerator);
	}

	public void Run()
	{
		foreach (var batGenerator in _batGenerators)
		{
			batGenerator.GenerateBat();
		}
	}

	public event Action<Exception>? OnException;
	public event Action<Type>? OnOk;
}