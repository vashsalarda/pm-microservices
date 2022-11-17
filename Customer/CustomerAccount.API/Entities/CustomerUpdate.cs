using Newtonsoft.Json;

namespace CustomerAccount.API.Entities;

public class CustomerUpdate
{
	
	[JsonProperty("email")]
	public string? Email { get; set; }

	[JsonProperty("firstName")]
	public string? FirstName { get; set; }

	[JsonProperty("lastName")]
	public string? LastName { get; set; }

	[JsonProperty("mobileNumber")]
	public string? MobileNumber { get; set; }

	[JsonProperty("country")]
	public string? Country { get; set; }

	[JsonProperty("state")]
	public string? State { get; set; }

	[JsonProperty("city")]
	public string? City { get; set; }

}