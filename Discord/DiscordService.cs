using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace discord_bot_ai_voice.Discord
{
    internal class DiscordService
    {
        private DiscordSocketClient discord;
        private readonly CommandService commands;

        public DiscordService(CommandService commands)
        {
            this.commands = commands;
        }

        // Initialize bot with token
        public async Task InitBot()
        {
            // TODO seperate file
            string tokenFile = @"D:\Keystore\discord-bot-ai-voice\discord.txt";
            var token = File.ReadAllText(tokenFile);

            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            discord = new DiscordSocketClient(config);
            discord.Log += Log;

            await discord.LoginAsync(TokenType.Bot, token);
            await discord.StartAsync();

            await InstallCommandsAsync();

            // Block task
            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            discord.MessageReceived += HandleCommandAsync;

            await commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process command if system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            if (message.Author.Id == this.discord.CurrentUser.Id || message.Author.IsBot) return;

            // Determine if message was command based on prefix and make sure no bots trigger this
            int argPos = 0;

            if (message.HasCharPrefix('!', ref argPos))
            {
                // Create a websocket-based command context
                var context = new SocketCommandContext(discord, message);

                // Execute command
                await commands.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: null);
            }
            else
            {
                Console.WriteLine("no");
            }


        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }


}
