namespace APIConsumeAPI.Health;

public class JokeHealthCheck : IHealthCheck
{
	private readonly HttpClient _client;
	private readonly string _endpoint;

	public JokeHealthCheck(IHttpClientFactory httpClientFactory)
	{
		_client = httpClientFactory.CreateClient("jokes");
		_endpoint = "random_joke";
	}

	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
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