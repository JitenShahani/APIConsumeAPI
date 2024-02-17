namespace APIConsumeAPI.Filters;

public class UniversityFilter : IEndpointFilter
{
	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		try
		{
            var _argument = context.GetArgument<Country>(0);
            var _errors = new Dictionary<string, string[]>();

            if (_argument.Name!.ToLower() == "pakistan" || _argument.Name!.ToLower() == "maldives" || _argument.Name!.ToLower() == "china")
            {
                _errors.Add(nameof(_argument.Name), [$"{_argument.Name} is an invalid country!"]);
            }

            if (_errors.Count > 0)
            {
                return TypedResults.ValidationProblem(_errors);
            }

        }
		catch (Exception)
		{
            var _argument = context.GetArgument<string>(0);
            var _errors = new Dictionary<string, string[]>();

            if (_argument.ToLower() == "pakistan" || _argument.ToLower() == "maldives" || _argument.ToLower() == "china")
            {
                _errors.Add(nameof(_argument), [$"{_argument} is an invalid country!"]);
            }

            if (_errors.Count > 0)
            {
                return TypedResults.ValidationProblem(_errors);
            }
        }

		return await next(context);
	}
}