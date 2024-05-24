using UI;

using HttpClient _client = new();
Process _process = new(new UserChoice(), _client);

do
{
	var _universities = await _process.CallApi();

	if (!_universities.Any())
	{
		WriteLine("No universities found.");
	}

	foreach (var item in _universities!)
	{
		WriteLine(item);
	}

	WriteLine();

	Write("Continue? (Y/N)? ");

} while (ReadLine()!.ToUpper() == "Y");