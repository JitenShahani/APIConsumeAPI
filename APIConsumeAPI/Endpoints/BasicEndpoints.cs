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

		group
			.MapGet("hello", () => $"Hello, World! {DateTime.Now.ToLongDateString()}")
			.WithTags("Basic Endpoints")
			.WithGroupName("v1")
			.Produces<string>(200)
			.WithMetadata(new SwaggerOperationAttribute("Hello, World! - Current Date as long string"));

		group
			.MapPost("/user", () =>
			{
				IEnumerable<string> users = [ "James", "Jon", "Steve", "Mads" ];

				return TypedResults.Ok(users);
			})
			.WithTags("Basic Endpoints")
			.WithGroupName("v1")
			.Produces<IEnumerable<string>>(200)
			.WithMetadata(new SwaggerOperationAttribute("List of users"));

		group
			.MapGet("/reverseString", (string input) => new string(input.Reverse().ToArray()))
			.WithTags("Basic Endpoints")
			.WithGroupName("v1")
			.Produces<string>(200)
			.WithMetadata(new SwaggerOperationAttribute("Reverse a string"));
	}
}