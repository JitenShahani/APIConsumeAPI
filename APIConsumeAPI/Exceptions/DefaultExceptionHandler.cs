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

		await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
		{
			Type = exception.GetType().Name,
			Title = "An unexpected error ocurred",
			Status = (int)HttpStatusCode.InternalServerError,
			Detail = exception.Message,
			Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
		});

		return true;
	}
}
