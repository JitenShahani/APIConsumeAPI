namespace APIConsumeAPI.DTO;

public class ApiResponse
{
	public bool IsSuccess { get; set; }
	public Object? Result { get; set; }
	public HttpStatusCode StatusCode { get; set; }
	public List<string> ErrorMessages { get; set; } = new();
}
