using Newtonsoft.Json;

namespace discord_bot_ai_voice.Models
{
    internal class UberDuckApiCredentials
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("api_secret")]
        public string ApiSecret { get; set; }
    }
}
