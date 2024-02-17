namespace APIConsumeAPI.DTO;

public class JokeResponse
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("type")]
	public string? Type { get; set; }

	[JsonPropertyName("setup")]
	public string? Setup { get; set; }

	[JsonPropertyName("punchline")]
	public string? PunchLine { get; set; }
}
