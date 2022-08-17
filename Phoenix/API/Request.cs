using System.Net.Http.Headers;
using System.Text;

namespace Phoenix
{
    public class Request
    {
        public static void Send(string endpoint, string method, string? auth, string? json = null, bool XAuditLogReason = false)
        {
            HttpClient client = new();
            if (Config.IsBot == true)
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bot {auth}");
            }
            else
            {
                client.DefaultRequestHeaders.Add("Authorization", auth);
            }

            if (XAuditLogReason == true)
            {
                client.DefaultRequestHeaders.Add("X-Audit-Log-Reason", "Phoenix Nuker");
            }

            HttpRequestMessage request = new(new HttpMethod(method), $"https://discord.com/api/v{Config.APIVersion}{endpoint}");
            if (json != null)
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            else
            {
                request.Content = null;
            }

            var response = client.GetAsync($"https://discord.com/api/v{Config.APIVersion}{endpoint}").GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            client.Send(request);
        }

        public static string SendGet(string endpoint, string? auth, string method = "GET", string? json = null)
        {
            HttpClient client = new();
            if (Config.IsBot == true)
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bot {auth}");
            }
            else
            {
                client.DefaultRequestHeaders.Add("Authorization", auth);
            }

            HttpRequestMessage request = new(new HttpMethod(method), $"https://discord.com/api/v{Config.APIVersion}{endpoint}");
            if (json != null)
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            else
            {
                request.Content = null;
            }

            var response = client.GetAsync($"https://discord.com/api/v{Config.APIVersion}{endpoint}").GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            return new StreamReader(client.Send(request).Content.ReadAsStream()).ReadToEnd();
        }
    }
}