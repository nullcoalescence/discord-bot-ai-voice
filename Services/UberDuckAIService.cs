using discord_bot_ai_voice.Models;

using Newtonsoft.Json;

using System.Net.Http.Headers;
using System.Text;


namespace discord_bot_ai_voice.Services
{
    /*
     * Service for interacting with https://uberduck.ai
     * 
     *  This abstracts the uberduck.ai api a little.
     *  The API takes 2 calls:
     *         1) the first call takes 2 params, the prompt and voice model to use. It returns a UUID. (I'm hardcoding 'drake' as the AI voice for now).
     *         2) the UUID is used in the second call which generates the voice and returns a response containing a url to the recording. This call may return some nulls and a failed_at date if 
     *            rate-limited.
     *  This service simplifies that a lot and hardcodes some stuff. Eventually maybe it can be modified to not exclusively use the Drake voice.
     * 
     */

    internal class UberDuckAIService : IUberDuckAIService
    {
        private readonly string apiKey;
        private readonly string apiSecret;

        private HttpClient httpClient;

        private readonly string baseUrl = "https://api.uberduck.ai";

        public UberDuckAIService(string apiKey, string apiSecret)
        {
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;

            this.httpClient = new HttpClient();
        }

        public async Task<string> GetVoiceURL(string prompt)
        {
            // Build the first request
            Console.WriteLine("Building http request to 'api.uberduck.ai/speak'...");
            var speakEndpointResponse = await this.MakeCallToSpeakEndpoint(prompt);

            // Sleep between calls to give the API time to generate the voice
            Console.WriteLine("Sleeping (10s)...");
            Thread.Sleep(10000);

            // Make second call to uberduck.ai to get the url for the recording
            Console.WriteLine("Building http request to 'api.uberduck.ai/speak-status'...");
            var speakStatusResponse = await this.MakeCallToSpeakStatusEndpoint(speakEndpointResponse);

            // Check if we were rate-limited. If so, sleep for 30 seconds and try again
            var reqFailed = RequestFailedOrRateLimited(speakStatusResponse);

            while (reqFailed)
            {
                Console.WriteLine("Sleeping for 30s then trying again.(press Ctrl+C to cancel process)");

                Thread.Sleep(30000);
                speakStatusResponse = await this.MakeCallToSpeakStatusEndpoint(speakEndpointResponse);

                reqFailed = RequestFailedOrRateLimited(speakStatusResponse);
            }

            return speakStatusResponse.Path;
        }

        // Make a request to api.uberduck.ai/speak
        private async Task<UberDuckApiSpeakResponse> MakeCallToSpeakEndpoint(string prompt)
        {
            this.httpClient.DefaultRequestHeaders.Authorization = BuildAuthHeader();

            var url = $"{this.baseUrl}/speak";

            var json = "{\"speech\":\"" + prompt + "\",\"voice\":\"drake\"}";
            var postBody = new StringContent(json);

            var result = await this.httpClient.PostAsync(url, postBody);

            if (result.StatusCode == System.Net.HttpStatusCode.OK) 
            {
                Console.WriteLine("'/speak' endpoint returned HttpStatusCode 200 [ok]");
            }

            var responseBody = await result.Content.ReadAsStringAsync();

            var speakEndpointResponse = JsonConvert.DeserializeObject<UberDuckApiSpeakResponse>(responseBody);
            return speakEndpointResponse;
        }

        // Make a request to api.uberduck.ai/speakstatus
        private async Task<UberDuckApiSpeakStatusResponse> MakeCallToSpeakStatusEndpoint(UberDuckApiSpeakResponse speakEndpointResp)
        {
            var url = $"{this.baseUrl}/speak-status?uuid={speakEndpointResp.Uuid}";

            var result = await this.httpClient.GetAsync(url);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("'/speak-status' endpoint returned HttpStatusCode 200 [ok]");
            }

            var responseBody = await result.Content.ReadAsStringAsync();

            var speakStatusEndpointResponse = JsonConvert.DeserializeObject<UberDuckApiSpeakStatusResponse>(responseBody);
            return speakStatusEndpointResponse;
        }

        // Generate http request header
        private AuthenticationHeaderValue BuildAuthHeader()
        {
            var credentials = Encoding.ASCII.GetBytes($"{this.apiKey}:{this.apiSecret}");
            var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));

            return header;
        }

        // Check 'UberDuckApiSpeakStatusResponse' model, if 'failed_at' is null, rate-limiting is a likely cause.
        // If all values (other than start_at') then it is probably still generating
        // Could also maybe be malformed voice prompt input that the api didn't like
        private bool RequestFailedOrRateLimited(UberDuckApiSpeakStatusResponse response)
        {
            if (response.FailedAt is null)
            {
                return false;
            }
            else
            {
                if (response.FailedAt is not null)
                {
                    Console.WriteLine("Error: api.uberduck.ai: '/speak-status' endpoint. Likely you are being rate-limited, try again later. Also check your prompt for any odd inputs.");
                    Console.WriteLine("Response data:");
                    Console.WriteLine($"- meta: {response.Meta}\n- failed_at: {response.FailedAt}");
                }

                if (response.Meta is null && response.FinishedAt is null)
                {
                    Console.WriteLine("Error: api.uberduck.ai: '/speak-status' endpoint. It looks like it is not finished processing your request.");
                }

                return true;
            }
        }
    }
}
