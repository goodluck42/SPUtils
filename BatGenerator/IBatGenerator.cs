namespace SPUtils.BatGenerator;

internal interface IBatGenerator
{
	void GenerateBat();

	event Action<Exception> OnException;
	event Action<Type>? OnOk;
}