using Discord.Commands;

using discord_bot_ai_voice.Models;
using discord_bot_ai_voice.Services;

using Newtonsoft.Json;

namespace discord_bot_ai_voice.Discord
{
    /*
     *      This class runs the 'drake' command
     *      Usage: [in discord]:   !drake I had brown sauce on my messsage supper!
     *             [bot]            the bot then runs 'I had brown sauce on my message supper!' thru the UberDuckAiService to generate the ai voice.
     *             [bot]            the bot then recieves the url to the voice recording on an s3 bucket from the UberDuckAiService.
     *             [bot]            it then responds with the url received from uberduck.ai. I'd like to make it trigger the server's music bot bot it looks like
     *                              at least JMusicBot does not respond to '!play' requests from other bots
     */

    public class DrakeCommand : ModuleBase<SocketCommandContext>
    {
        [Command("drake")]
        [Summary("Takes a prompt and generates a text-to-speech recording using UberDuck.ai's Drake voice model")]
        public async Task DrakeAsync([Remainder][Summary("Voice Prompt")] string prompt)
        {
            var loadingMessage = await ReplyAsync("Contacting Drake‼️ (Please be patient, he might be 😴💤🥱)");
            // TODO This is where dependency injection would be nice....
            // Get the voice URL from the UberDuckAiService
            var uberduckCredsPath = @"D:\Keystore\discord-bot-ai-voice\uberduck_ai_credentials.json";

            var uberduckAiCreds = JsonConvert.DeserializeObject<UberDuckApiCredentials>(File.ReadAllText(uberduckCredsPath));
            var uberduckAiService = new UberDuckAIService(uberduckAiCreds.ApiKey, uberduckAiCreds.ApiSecret);

            var voiceUrl = await uberduckAiService.GetVoiceURL(prompt);

            // delete 'loading' message
            await Context.Channel.DeleteMessageAsync(loadingMessage);

            await ReplyAsync($"Drake voice generated:\n{voiceUrl}");

        }
    }
}
