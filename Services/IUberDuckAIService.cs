namespace discord_bot_ai_voice.Services
{
    internal interface IUberDuckAIService
    {
        public Task<string> GetVoiceURL(string prompt);
    }
}
