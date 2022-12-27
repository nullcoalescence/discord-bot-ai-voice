using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discord_bot_ai_voice.Models
{
    internal class UberDuckApiSpeakStatusResponse
    {
        [JsonProperty("failed_at")]
        public string FailedAt { get; set; }
        [JsonProperty("finished_at")]
        public string FinishedAt { get; set; }
        [JsonProperty("meta")]
        public string Meta { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("started_at")]
        public string StartedAt { get; set; }
    }
}
