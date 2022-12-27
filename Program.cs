using discord_bot_ai_voice.Models;
using discord_bot_ai_voice.Services;
using Newtonsoft.Json;
using System.Net;

public class Program
{
    /*
     *  Args:
     *      1) path to uberduck.ai credentials json file
     *      // TODO
     *      2) prompt
     *      3) method (discord or download or autoplay)
     *      4) if download -> directory
     */

    internal static async Task Main(string[] args)
    {
        // Generate voice from uberduck.ai
        var uberduckAiCreds = JsonConvert.DeserializeObject<UberDuckApiCredentials>(File.ReadAllText(args[0])?? string.Empty);
        var uberduckAiService = new UberDuckAIService(uberduckAiCreds.ApiKey, uberduckAiCreds.ApiSecret);

        var url = await uberduckAiService.GetVoiceURL("I put brown sauce on my sausage supper! Please don't fart gordon I beg of you you");

        // Download
        var downloadService = new DownloadService(url, @"c:\users\btov1\Downloads", "drake.wav");
        var dl = await downloadService.DownloadFile();

        // Play
        var playerService = new PlayerService();
        playerService.PlayWavFile(dl);
    }
}