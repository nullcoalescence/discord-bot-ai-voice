
using Newtonsoft.Json;

namespace discord_bot_ai_voice.Models
{
    internal class UberDuckApiSpeakResponse
    {
        [JsonProperty("uuid")]
        public string? Uuid { get; set; }
    }
}
