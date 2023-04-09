using System.Net.Http.Headers;
using System.Text;

namespace Phoenix.API
{
    public class Request
    {
        public static string Send(string endpoint, string method, string? auth, string? json = null, bool XAuditLogReason = false)
        {
            HttpClient client = new();
            string? token = Config.IsBot ? $"Bot {auth}" : auth;
            client.DefaultRequestHeaders.Add("Authorization", token);
            if (XAuditLogReason == true) client.DefaultRequestHeaders.Add("X-Audit-Log-Reason", "Phoenix Nuker");
            HttpRequestMessage request = new(new HttpMethod(method), $"https://discord.com/api/v{Config.APIVersion}{endpoint}");
            if (json != null)
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            else request.Content = null;
            if (method != "GET") client.Send(request);
            else
            {
                var response = client.GetAsync($"https://discord.com/api/v{Config.APIVersion}{endpoint}").GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                Thread.Sleep(1000);
                return new StreamReader(client.Send(request).Content.ReadAsStream()).ReadToEnd();
            }
            return "";
        }

        public static bool Check(string token)
        {
            try
            {
                try
                {
                    HttpClient client = new();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    HttpRequestMessage request = new(new HttpMethod("GET"), $"https://discord.com/api/v{Config.APIVersion}/users/@me") { Content = null };
                    var response = client.GetAsync($"https://discord.com/api/v{Config.APIVersion}/users/@me").GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch
                {
                    HttpClient client = new();
                    client.DefaultRequestHeaders.Add("Authorization", $"Bot {token}");
                    HttpRequestMessage request = new(new HttpMethod("GET"), $"https://discord.com/api/v{Config.APIVersion}/users/@me") { Content = null };
                    var response = client.GetAsync($"https://discord.com/api/v{Config.APIVersion}/users/@me").GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    return true;
                }
            }
            catch { return false; }
        }
    }
}
