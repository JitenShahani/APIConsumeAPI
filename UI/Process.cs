namespace UI;

public class Process
{
	private readonly IChoice _choice;
	private readonly HttpClient _httpClient;
	private const string _baseAddress = $@"https://localhost:44314/api/university/v4?country=";

	public Process(IChoice choice, HttpClient httpClient)
	{
		_choice = choice;
		_httpClient = httpClient;
	}

	public async Task<IEnumerable<UniversityResponse>> CallApi()
	{
		// User Choice
		string _country = _choice.GetChoice();

		string _endpointAddress = _baseAddress + _country;

		IEnumerable<UniversityResponse> _apiResponse = [];

		try
		{
			var _response = await _httpClient.GetFromJsonAsync<IEnumerable<UniversityResponse>>(_endpointAddress);
			_apiResponse = _response!.ToList();
		}
		catch (Exception ex)
		{
			WriteLine(ex.Message);
		}

		return _apiResponse.ToList();
	}
}