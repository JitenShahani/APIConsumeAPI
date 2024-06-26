﻿using Microsoft.AspNetCore.Diagnostics;

namespace APIConsumeAPI;

public class TimeOutExceptionHandler : IExceptionHandler
{
	private readonly ILogger<DefaultExceptionHandler> _logger;

	public TimeOutExceptionHandler(ILogger<DefaultExceptionHandler> logger)
	{
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		_logger.LogError(exception, "A timeout occurred");

		if (exception is TimeoutException)
		{
			httpContext.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;

			await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
			{
				Type = exception.GetType().Name,
				Title = "A timeout occurred",
				Status = (int)HttpStatusCode.RequestTimeout,
				Detail = exception.Message,
				Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
			});
			return true;
		}

		return false;
	}
}