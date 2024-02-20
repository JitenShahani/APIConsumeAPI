namespace APIConsumeAPI.Endpoints;

public class ConsumeUniversityEndpoints
{
	private readonly HttpClient _client;
	private readonly IConfiguration _config;
	private readonly ILogger _logger;

	public ConsumeUniversityEndpoints(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory, IConfiguration config)
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

		_logger = loggerFactory.CreateLogger<UniversityResponse>();
	}

	public void ConfigureUniversityEndpoints(IEndpointRouteBuilder endpoint)
	{
		var group = endpoint.MapGroup("/api/")
			.WithTags("University Endpoints")
			.WithGroupName("v1")
			.RequireRateLimiting("token")
			.RequireRateLimiting("concurrency")
			.WithOpenApi();

		group
			.MapGet("/v1/university", GetAsyncV1)
			.Produces<UniversityResponse>(200)
			.Produces(400)
			.WithMetadata(new SwaggerOperationAttribute("Old fashioned - Country name hardcoded - String output"));

		group
			.MapGet("/v2/university", GetAsyncV2)
			.Produces<UniversityResponse>(200)
			.Produces(400)
			.WithMetadata(new SwaggerOperationAttribute("Old fashioned - Country name hardcoded - Json output"));

		group
			.MapGet("/v3/university/{country}", GetAsyncV3)
			.Produces<UniversityResponse>(200)
			.Produces(400)
			.WithMetadata(new SwaggerOperationAttribute("Old fashioned - Country parameter from route - Json output"))
			.AddEndpointFilter<UniversityFilter>();

		group
			.MapGet("/v4/university/", GetAsyncV4)
			.Produces<UniversityResponse>(200)
			.Produces(400)
			.WithMetadata(new SwaggerOperationAttribute("Single line - Country parameter from query - Json output"))
			.AddEndpointFilter<UniversityFilter>();

		group
			.MapPost("/v5/university", GetAsyncV5)
			.Produces<UniversityResponse>(200)
			.Produces(400)
			.WithMetadata(new SwaggerOperationAttribute("POST - Single line - Country parameter from body - Json output"))
			.AddEndpointFilter<UniversityFilter>();
	}

	private async Task<Results<Ok<string>, BadRequest>> GetAsyncV1()
	{
		var response = await _client.GetAsync("search?country=Japan");

		if (response.IsSuccessStatusCode)
		{
			var stringResult = await response.Content.ReadAsStringAsync();
			return TypedResults.Ok(stringResult);
		}

		_logger.LogCritical("Unable to access Universities endpoint");
		return TypedResults.BadRequest();
	}

	private async Task<Results<Ok<IEnumerable<UniversityResponse>>, BadRequest>> GetAsyncV2()
	{
		var response = await _client.GetAsync("search?country=Japan");

		if (response.IsSuccessStatusCode)
		{
			var stringResult = await response.Content.ReadAsStringAsync();

			var jsonData = JsonSerializer.Deserialize<IEnumerable<UniversityResponse>>(stringResult);

			return TypedResults.Ok(jsonData);
		}

		_logger.LogCritical("Unable to access Universities endpoint");
		return TypedResults.BadRequest();
	}

	private async Task<Results<Ok<IEnumerable<UniversityResponse>>, BadRequest>> GetAsyncV3([FromRoute] string country)
	{
		var response = await _client.GetAsync($"search?country={country}");

		if (response.IsSuccessStatusCode)
		{
			var stringResult = await response.Content.ReadAsStringAsync();

			var jsonData = JsonSerializer.Deserialize<IEnumerable<UniversityResponse>>(stringResult);

			return TypedResults.Ok(jsonData);
		}

		_logger.LogCritical("Unable to access Universities endpoint");
		return TypedResults.BadRequest();
	}

	private async Task<Results<Ok<IEnumerable<UniversityResponse>>, BadRequest>> GetAsyncV4([FromQuery] string country)
	{
		var result = await _client.GetFromJsonAsync<IEnumerable<UniversityResponse>>($"search?country={country}");

		if (result!.Any())
		{
			return TypedResults.Ok(result);
		}

		_logger.LogCritical("Unable to access Universities endpoint");
		return TypedResults.BadRequest();
	}

	private async Task<Results<Ok<IEnumerable<UniversityResponse>>, BadRequest>> GetAsyncV5([FromBody] Country country)
	{
		var result = await _client.GetFromJsonAsync<IEnumerable<UniversityResponse>>($"search?country={country.Name}");

		if (result!.Any())
		{
			return TypedResults.Ok(result);
		}

		_logger.LogCritical("Unable to access Universities endpoint");
		return TypedResults.BadRequest();
	}
}