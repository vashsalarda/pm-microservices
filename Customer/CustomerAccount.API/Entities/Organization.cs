using Newtonsoft.Json;

namespace CustomerAccount.API.Entities;

public class Organization
{

	[JsonProperty("id")]
	public string? Id { get; set; }

	[JsonProperty("customerId")]
	public string? CustomerId { get; set; }

	[JsonProperty("companyName")]
	public string? CompanyName { get; set; }

	[JsonProperty("country")]
	public string? Country { get; set; }

	[JsonProperty("state")]
	public string? State { get; set; }

	[JsonProperty("city")]
	public string? City { get; set; }

[	JsonProperty("address")]
	public string? Address { get; set; }

	[JsonProperty("mobileNumber")]
	public string? MobileNumber { get; set; }

	[JsonProperty("landline")]
	public string? Landline { get; set; }

	[JsonProperty("industry")]
	public string? Industry { get; set; }

	[JsonProperty("language")]
	public string? Language { get; set; }

	[JsonProperty("companySize")]	
	public int CompanySize { get; set; }

}