using System.Collections.Generic;
using Newtonsoft.Json;

namespace Newsletter.EmailOctopus
{
    internal class SubscribeRequestModel
    {
        [JsonProperty("api_key")] public string ApiKey { get; set; }
        [JsonProperty("email_address")] public string EmailAddress { get; set; }
        [JsonProperty("fields")] public Dictionary<string, string> Fields { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("tags")] public string[] Tags { get; set; }
    }
}