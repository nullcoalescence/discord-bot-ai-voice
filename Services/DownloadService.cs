using System.Net;

namespace discord_bot_ai_voice.Services
{
    internal class DownloadService : IDownloadService
    {
        private readonly string url;
        private readonly string path;
        private readonly string fileName;

        private readonly string downloadPath;

        public DownloadService(string url, string path, string fileName) 
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(path);
            }

            this.url = url;
            this.path = path;
            this.fileName = fileName;

            this.downloadPath = Path.Combine(this.path, this.fileName);
        }

        public async Task<string> DownloadFile()
        {
            Console.WriteLine($"Downloading: {this.url}...");

            var downloadFile = Path.Combine(this.path, this.fileName);

            using (var httpClient = new HttpClient())
            {
                await DownloadFile(httpClient, new Uri(this.url));
            }

            Console.WriteLine($"Downloaded '{this.fileName}' to: {this.path}.");

            return this.downloadPath;
        }

        private async Task DownloadFile(HttpClient client, Uri uri)
        {
            byte[] fileBytes = await client.GetByteArrayAsync(uri);
            File.WriteAllBytes(this.downloadPath, fileBytes);
        }
    }
}
