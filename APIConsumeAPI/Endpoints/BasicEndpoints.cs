namespace APIConsumeAPI.Endpoints;

public class BasicEndpoints
{
	public void ConfigureBasicEndpoints(IEndpointRouteBuilder endpoint)
	{
		var group = endpoint.MapGroup("/api")
			.WithTags("Basic Endpoints")
			.WithGroupName("v1")
			.DisableRateLimiting()
			.WithOpenApi();

		endpoint
			.MapGet("hello", () => $"Hello, World! {DateTime.Now.ToLongDateString()}")
			.Produces<string>(200)
			.WithMetadata(new SwaggerOperationAttribute("Hello, World! - Current Date as long string"));

		endpoint
			.MapPost("/user", () =>
			{
				IEnumerable<string> users = [ "James", "Jon", "Steve", "Mads" ];

				return TypedResults.Ok(users);
			})
			.Produces<IEnumerable<string>>(200)
			.WithMetadata(new SwaggerOperationAttribute("List of users"));
	}
}