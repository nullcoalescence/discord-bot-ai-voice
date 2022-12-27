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

        var url = await uberduckAiService.GetVoiceURL("I put brown sauce on my sausage supper!");

        // Download
        var downloadService = new DownloadService(url, @"c:\users\btov1\Downloads", "drake.wav");
        await downloadService.Download();


        // args
        // -local --path ~/downloads --name drake.mp4 --prompt 'haha prompt' 
        // -discord --discord_creds --prompt 'haha'
    }
}