using Microsoft.AspNetCore.Diagnostics;

namespace APIConsumeAPI;

public class DefaultExceptionHandler : IExceptionHandler
{
	private readonly ILogger<DefaultExceptionHandler> _logger;

	public DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger)
	{
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		_logger.LogError(exception, "An unexpected error ocurred");

		ProblemDetails problemDetails = new()
		{
			Type = exception.GetType().Name,
			Title = "An unexpected error ocurred",
			Status = StatusCodes.Status500InternalServerError,
			Detail = exception.Message,
			Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
		};

		httpContext.Response.StatusCode = problemDetails.Status.Value;

		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

		return true;
	}
}