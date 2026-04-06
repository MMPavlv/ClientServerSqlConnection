using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client.Models
{
    public static class ConnectionService
    {
        public static readonly string ConnectAction = "Connect";
        public static readonly string CloseAction = "Close";
        public static readonly string GetVersionAction = "GetVersion";

        public static readonly HttpClient Client = new()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        public static async Task<string> SendRequestAsync(string url, string action)
        {
            using var response = await Client.GetAsync(BuildEndpointUri(url, action));

            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return responseBody;
            }

            var errorDetails = ReadStringPayload(responseBody);
            var statusCode = (int)response.StatusCode;

            throw new HttpRequestException(
                string.IsNullOrWhiteSpace(errorDetails)
                    ? $"Server returned error {statusCode}."
                    : $"Server returned error {statusCode}: {errorDetails}");
        }

        private static Uri BuildEndpointUri(string url, string action)
        {
            action = action.Trim();
            url = url.Trim();

            Regex rg = new(@"^localhost:(?<port>\d+)$",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            if (rg.IsMatch(url))
            {
                var a = new Uri($"http://{url}/Version/{action}");
                return new Uri($"http://{url}/Version/{action}");
            }

            if (!Uri.TryCreate($"{url}/Version/{action}", UriKind.Absolute, out var baseUri))
            {
                throw new InvalidOperationException("Invalid server address.");
            }

            return baseUri;
        }

        public static string ReadStringPayload(string responseBody)
        {
            var trimmedBody = responseBody.Trim();
            if (string.IsNullOrWhiteSpace(trimmedBody))
            {
                return string.Empty;
            }

            if (trimmedBody.StartsWith('\"'))
            {
                return JsonSerializer.Deserialize<string>(trimmedBody) ?? string.Empty;
            }

            return trimmedBody;
        }
    }
}
