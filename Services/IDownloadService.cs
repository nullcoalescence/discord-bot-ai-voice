namespace discord_bot_ai_voice.Services
{
    internal interface IDownloadService
    {
        public Task<string> DownloadFile();
    }
}
