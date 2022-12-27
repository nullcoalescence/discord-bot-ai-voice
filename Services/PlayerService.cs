using System.Diagnostics;

namespace discord_bot_ai_voice.Services
{
    internal class PlayerService : IPlayerService
    {
        public PlayerService() { }

        // Just open in default media player to keep it simple
        public void PlayWavFile(string path)
        {
            Console.WriteLine($"Opening '{path}' in default system media player...");

            using (Process proc = new Process())
            {
                proc.StartInfo.FileName = "explorer";
                proc.StartInfo.Arguments = "\"" + path + "\"";

                proc.Start();
            }

            Console.WriteLine("Done");
        }
    }
}
