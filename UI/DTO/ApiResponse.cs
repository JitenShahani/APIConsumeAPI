using System.Net;
using System.Text.Json.Serialization;

namespace UI.DTO;

public class ApiResponse
{
	[JsonPropertyName("isSuccess")]
	public bool IsSuccess { get; set; }
	[JsonPropertyName("result")]
	public Object? Result { get; set; }
	[JsonPropertyName("statusCode")]
	public HttpStatusCode StatusCode { get; set; }
	[JsonPropertyName("errorMessages")]
	public List<string> ErrorMessages { get; set; } = new();
}
