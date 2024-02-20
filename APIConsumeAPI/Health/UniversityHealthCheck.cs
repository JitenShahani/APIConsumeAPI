namespace APIConsumeAPI.Health;

public class UniversityHealthCheck : IHealthCheck
{
	private readonly HttpClient _client;
	private readonly string _endpoint;

	public UniversityHealthCheck(IHttpClientFactory httpClientFactory)
	{
		_client = httpClientFactory.CreateClient("universities");
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