using Newtonsoft.Json.Linq;
using Console = Colorful.Console;
using System.Drawing;
using static Phoenix.API.Config;

namespace Phoenix.API
{
    public class Guild
    {
        public static void MsgInEveryChannel(string? token, ulong? gid, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/channels", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    if (entry.type == "0")
                    {
                        Request.Send($"/channels/{entry.id}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                        Sleep(Wait.Short);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void LeaveDeleteGuild(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                try { Request.Send($"/guilds/{gid}", "DELETE", token); }
                catch { Request.Send($"/users/@me/guilds/{gid}", "DELETE", token); }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void GuildInformation(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}", "GET", token);
                var id = JObject.Parse(request)["id"];
                var ownerid = JObject.Parse(request)["owner_id"];
                var region = JObject.Parse(request)["region"];
                var vanityurl = JObject.Parse(request)["vanity_url_code"];
                var iconid = JObject.Parse(request)["icon"];
                var iconid2 = iconid == null || string.IsNullOrEmpty(iconid.ToString()) ? "" : iconid.ToString();
                var bannerid = JObject.Parse(request)["banner"];
                var bannerid2 = bannerid == null || string.IsNullOrEmpty(bannerid.ToString()) ? "" : bannerid.ToString();
                string icon;
                if (string.IsNullOrEmpty(iconid2)) icon = "N/A";
                else icon = $"https://cdn.discordapp.com/icons/{id}/{iconid}.webp";
                string banner;
                if (string.IsNullOrEmpty(bannerid2)) banner = "N/A";
                else banner = $"https://cdn.discordapp.com/banners/{id}/{bannerid}.webp?size=240";
                dynamic? verificationlevel = JObject.Parse(request)["verification_level"];
                switch (verificationlevel)
                {
                    case "0":
                        verificationlevel = "None";
                        break;
                    case "1":
                        verificationlevel = "Low";
                        break;
                    case "2":
                        verificationlevel = "Medium";
                        break;
                    case "3":
                        verificationlevel = "High";
                        break;
                    case "4":
                        verificationlevel = "Highest";
                        break;
                }
                var preferredlocale = JObject.Parse(request)["preferred_locale"];
                var nsfw = JObject.Parse(request)["nsfw"];
                var roles = Request.Send($"/guilds/{gid}/roles", "GET", token);
                var array = JArray.Parse(roles);
                int rolescount = 0;
                foreach (dynamic entry in array) rolescount++;
                var channels = Request.Send($"/guilds/{gid}/channels", "GET", token);
                var array2 = JArray.Parse(channels);
                int channelscount = 0;
                foreach (dynamic entry in array2) channelscount++;
                var emojis = Request.Send($"/guilds/{gid}/emojis", "GET", token);
                var array3 = JArray.Parse(emojis);
                int emojiscount = 0;
                foreach (dynamic entry in array3) emojiscount++;
                var stickers = Request.Send($"/guilds/{gid}/stickers", "GET", token);
                var array4 = JArray.Parse(stickers);
                int stickerscount = 0;
                foreach (dynamic entry in array4) stickerscount++;
                var bans = Request.Send($"/guilds/{gid}/bans", "GET", token);
                var array5 = JArray.Parse(bans);
                int banscount = 0;
                foreach (dynamic entry in array5) banscount++;
                var invites = Request.Send($"/guilds/{gid}/invites", "GET", token);
                var array6 = JArray.Parse(invites);
                int invitescount = 0;
                foreach (dynamic entry in array6) invitescount++;
                Console.ForegroundColor = Color.Yellow;
                Console.WriteLine("Guild Information:\n");
                Console.WriteLine($"Guild ID: {id}\nOwner ID: {ownerid}\nRegion: {region}\nRoles Count: {rolescount}\nChannels Count: {channelscount}\nEmojis Count: {emojiscount}\nStickers Count: {stickerscount}\nBans Count: {banscount}\nInvites Count: {invitescount}\nVerification Level: {verificationlevel}\nPreferred Locale: {preferredlocale}\nNSFW: {nsfw}\nVanity Code: {vanityurl}\nGuild Icon: {icon}\nBanner: {banner}");
                Console.WriteLine("\nPress any key to go back.");
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void CreateChannel(string? token, ulong? gid, string name)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/guilds/{gid}/channels", "POST", token, $"{{\"name\":\"{name}\"}}", true);
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void CreateRole(string? token, ulong? gid, string name)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/guilds/{gid}/roles", "POST", token, $"{{\"name\":\"{name}\"}}", true);
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteInvites(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/invites", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/invites/{entry.code}", "DELETE", token, null, true);
                    Console.WriteLine($"Deleted: {entry.code}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteEmojis(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/emojis", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/emojis/{entry.id}", "DELETE", token, null, true);
                    Console.WriteLine($"Deleted: {entry.name}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteChannels(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/channels", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/channels/{entry.id}", "DELETE", token, null, true);
                    Console.WriteLine($"Deleted: {entry.name}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void RemoveBans(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/bans", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/bans/" + entry.user["id"], "DELETE", token, null, true);
                    Console.WriteLine($"Removed: {entry.user["username"]}#{entry.user["discriminator"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteRoles(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/roles", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/roles/" + entry["id"], "DELETE", token, null, true);
                    Console.WriteLine($"Deleted: {entry["name"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteStickers(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/stickers", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/stickers/" + entry["id"], "DELETE", token, null, true);
                    Console.WriteLine($"Deleted: {entry["name"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static string GetGuildName(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                string request = Request.Send($"/guilds/{gid}", "GET", token);
                var name = JObject.Parse(request)["name"];
                if (name != null) return name.ToString();
                return "N/A";
            }
            catch { return "N/A"; }
        }

        public static void PruneMembers(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/guilds/{gid}/prune", "POST", token, $"{{\"days\": 1}}", true);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void RemoveIntegrations(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/integrations", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/integrations/" + entry["id"], "DELETE", token, null, true);
                    Console.WriteLine($"Deleted: {entry["name"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteAllReactions(string? token, ulong? cid, ulong? mid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/channels/{cid}/messages/{mid}/reactions", "DELETE", token);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }
        public static void DeleteAutoModerationRules(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/auto-moderation/rules", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/auto-moderation/rules/" + entry["id"], "DELETE", token, null, true);
                    Console.WriteLine($"Deleted: {entry["name"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void CreateInvite(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/channels", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/channels/{entry.id}/invites", "POST", token, $"{{\"max_age\": 0}}", true);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteGuildScheduledEvents(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/scheduled-events", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/scheduled-events/{entry.id}", "DELETE", token, null, true);
                    Console.WriteLine($"Deleted: {entry["name"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteGuildTemplate(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/templates", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/templates/{entry.code}", "DELETE", token, null, true);
                    Console.WriteLine($"Deleted: {entry["name"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteStageInstances(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/channels", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    if (entry.type == "13")
                    {
                        Request.Send($"/stage-instances/{entry.id}", "DELETE", token, null, true);
                        Console.WriteLine($"Deleted: {entry["name"]}", Color.Lime);
                        Sleep(Wait.Short);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteWebhooks(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/webhooks", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/webhooks/{entry.id}", "DELETE", token, null, true);
                    Console.WriteLine($"Deleted: {entry["name"]}", Color.Lime);
                    Sleep(Wait.Short);
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void DeleteWebhook(string? token, ulong? wid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/webhooks/{wid}", "DELETE", token, null, true);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void SendWebhookMessage(string? token, ulong? wid, string wtoken, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"/webhooks/{wid}/{wtoken}", "POST", token, $"{{\"content\":\"{message}\"}}");
                Sleep(Wait.Short);
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void GrantEveryoneAdmin(string? token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.Send($"/guilds/{gid}/roles", "GET", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    if (entry.name == "@everyone")
                    {
                        Request.Send($"/guilds/{gid}/roles/{entry.id}", "PATCH", token, $"{{\"permissions\":\"8\"}}", true);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }

        public static void ReportMessage(string? token, ulong? gid, ulong? cid, ulong? mid, int reason)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send("/report", "POST", token, $"{{\"channel_id\": \"{cid}\", \"guild_id\": \"{gid}\", \"message_id\": \"{mid}\", \"reason\": {reason}}}");
            }
            catch (Exception e) { Console.WriteLine($"Failed: {e.Message}", Color.Red); Sleep(Wait.Long); }
        }
    }
}
