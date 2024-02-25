namespace APIConsumeAPI.Endpoints;

public class ConsumeJokeEndpoints
{
	private readonly HttpClient _client;
	private readonly IConfiguration _config;
	private readonly ILogger _logger;

	public ConsumeJokeEndpoints(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory, IConfiguration config)
	{
		_config = config;

		try
		{
			_client = httpClientFactory.CreateClient("jokes");
		}
		catch
		{
			_client = httpClientFactory.CreateClient();
			_client.BaseAddress = new Uri(_config["JokeBaseAddress"]!);
		}

		_logger = loggerFactory.CreateLogger<JokeResponse>();
	}

	public void ConfigureJokeEndpoints(IEndpointRouteBuilder endpoint)
	{
		var group = endpoint.MapGroup("/api/joke")
			.WithTags("Joke Endpoint")
			.WithGroupName("v1")
			.RequireRateLimiting("token")
			.RequireRateLimiting("concurrency")
			.WithOpenApi();

		group
			.MapGet("/v1", GetJokeV1)
			.Produces<JokeResponse>(200)
			.Produces(400)
			.WithMetadata(new SwaggerOperationAttribute("Single line - Json output"));

		group
			.MapGet("/v2", GetJokeV2)
			.Produces<JokeResponse>(200)
			.Produces(400)
			.WithMetadata(new SwaggerOperationAttribute("Single line - Programming Jokes - Json output"));
	}

	private async Task<Results<Ok<JokeResponse>, BadRequest>> GetJokeV1()
	{
		var response = await _client.GetAsync("random_joke");

		if (response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<JokeResponse>();
			return TypedResults.Ok(result);
		}

		_logger.LogCritical("Unable to access Jokes endpoint");
		return TypedResults.BadRequest();
	}

	private async Task<Results<Ok<JokeResponse>, BadRequest>> GetJokeV2()
	{
		var response = await _client.GetAsync("jokes/programming/random");

		if (response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<IEnumerable<JokeResponse>>();
			return TypedResults.Ok(result!.First());
		}

		_logger.LogCritical("Unable to access Programming Jokes endpoint");
		return TypedResults.BadRequest();
	}
}