using Newtonsoft.Json;

namespace CustomerAccount.API.Entities;

public class Role
{

	[JsonProperty("id")]
	public string Id { get; set; } = null!;

	[JsonProperty("name")]
	public string Name { get; set; } = null!;

	[JsonProperty("key")]
	public string Key { get; set; } = null!;

}