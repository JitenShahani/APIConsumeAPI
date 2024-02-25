namespace APIConsumeAPI.Exceptions;

public class GlobalExceptionHandling : IMiddleware
{
	private readonly ILogger _logger;

	public GlobalExceptionHandling(ILogger<ApiResponse> logger) => _logger = logger;

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception e)
		{
			_logger.LogError(e, e.Message);

			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			ApiResponse response = new()
			{
				IsSuccess = false,
				StatusCode = HttpStatusCode.InternalServerError,
				Result = null,
				ErrorMessages = new()
				{
					e.Message
				}
			};

			var json = JsonSerializer.Serialize(response);

			context.Response.ContentType = "application/json";

			await context.Response.WriteAsync(json);
		}
	}
}