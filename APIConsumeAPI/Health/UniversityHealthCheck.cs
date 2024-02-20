namespace APIConsumeAPI.Health;

public class UniversityHealthCheck : IHealthCheck
{
	private readonly HttpClient _client;
	private readonly IConfiguration _config;
	private readonly string _endpoint;

	public UniversityHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration config)
	{
		_config = config;

		try
		{
			_client = httpClientFactory.CreateClient("universities");
		}
		catch (System.Exception)
		{
			_client = httpClientFactory.CreateClient();
			_client.BaseAddress = new Uri(_config["UniversityBaseAddress"]!);
		}

		_endpoint = "search?country=Bhutan";
	}

	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
	{
		try
		{
			var response = await _client.GetAsync(_endpoint, cancellationToken);

			if (response.IsSuccessStatusCode)
			{
				return HealthCheckResult.Healthy();
			}

			return HealthCheckResult.Unhealthy();
		}
		catch (Exception ex)
		{
			return HealthCheckResult.Unhealthy(ex.Message);
		}
	}
}