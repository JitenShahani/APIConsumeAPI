namespace APIConsumeAPI.DTO;

public class UniversityResponse
{
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("alpha_two_code")]
	public string? Code { get; set; }

	[JsonPropertyName("country")]
	public string? Country { get; set; }

	[JsonPropertyName("state-province")]
	public string? State { get; set; }

	[JsonPropertyName("domains")]
	public IEnumerable<string>? Domains { get; set; }

	[JsonPropertyName("web_pages")]
	public IEnumerable<string>? Sites { get; set; }
}