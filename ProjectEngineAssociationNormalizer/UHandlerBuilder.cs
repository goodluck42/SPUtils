namespace SPUtils.ProjectEngineAssociationNormalizer;

internal sealed class UHandlerBuilder
{
	private readonly List<UHandler> _uHandlers;

	public UHandlerBuilder()
	{
		_uHandlers = [];
	}

	public void AddHandler(UHandler handler)
	{
		_uHandlers.Add(handler);
	}

	public UHandler Build()
	{
		if (_uHandlers.Count == 0)
		{
			throw new InvalidOperationException("Handlers are empty.");
		}

		if (_uHandlers.Count == 1)
		{
			return _uHandlers[0];
		}

		var previous = _uHandlers[0];
		var first = previous;

		foreach (var uHandler in _uHandlers.Skip(1))
		{
			previous.Next = uHandler;
			previous = uHandler;
		}

		return first;
	}
}